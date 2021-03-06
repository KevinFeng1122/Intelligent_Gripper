//----------【硬件配置说明】↓------------------------------------------------------------
//舵机：波特率19200，电源DC12V，ID：0和1
//电源线和控制线的电平互不干扰
//电源2根，RS485两根
//UART0用于MCU和PC的通信，波特率19200
//UART1用于MCU和舵机的通信，波特率19200

//----------【头文件】↓------------------------------------------------------------
#include <iom128v.h>
#include <macros.h>
#include <string.h>

//----------【宏定义】↓------------------------------------------------------------
#define  uchar unsigned char
#define  uint  unsigned int

#define  mclk   8000000 //时钟频率8.0MHz

//----------【常量】↓------------------------------------------------------------
const uchar Ratio1=1;
const uchar Ratio2=2;
const uchar Force=3;


const uchar no0release[]={0xff,0xff,0x00,0x05,0x03,0x20,0xff,0x07,0xd1};//0号舵机松开（顺时针，力矩100%）
const uchar no0stop[]={0xff,0xff,0x00,0x05,0x03,0x20,0x00,0x00,0xd7}; //0号舵机停止（逆时针，力矩0%）
const uchar no1release[]={0xff,0xff,0x01,0x05,0x03,0x20,0xff,0x07,0xd0};//1号舵机松开（顺时针，力矩100%）
const uchar no1stop[]={0xff,0xff,0x01,0x05,0x03,0x20,0x00,0x00,0xd6}; //1号舵机停止（逆时针，力矩0%）

//----------【全局变量定义】↓------------------------------------------------------------

uchar gripper_mood=0;//夹持器模式：0-未指定；1-一般工作模式；2-参数配置模式

uchar cage0_state,cage1_state;//表明夹持器两端限位情况

/*以下变量用于建立上位机指令处理机制*/
uchar uart0_rdata_byte;//UART0每次接收到的单字节信息
uchar uart0_r_instr_chk=0;//uart0接收到的字符串开头中x字符的个数
uchar uart0_instr[5];//存储PC发给MCU的指令，不包含xx，仅包含四位指令代码，最后一位为NULL
uchar uart0_instr_flag=0;//UART0中断接收到开头和长度符合要求的instr时置为1

//夹持器与外部碰撞报警（中断）允许变量
uchar ext_collision_alert_allow_int0=0;//上侧
uchar ext_collision_alert_allow_int1=0;//下侧
uchar ext_collision_alert_allow_int4=0;//指尖

uchar force_high8;//应变片返回值高8位aa，long型 00 aa bb cc
uchar force_judge;//保存存在EEPROM中的应变片的有效值高八位，用于判断夹紧力是否到达要求
unsigned long force_ulong;//当前应变片测出的数值（占用4 byte，实际有效值3 byte）


//--------------【软件延时函数】--------------------------------------------------------------------

//延时函数，参数为要延时的毫秒数
void delay(uint ms)
{
    uint i,j;
	for(i=0;i<ms;i++)
	{
	 for(j=0;j<1141;j++);
    }
}


//----------【UART0的相关函数】↓---------------------------------------------------------------

/*UART0的串口初始化函数*/
void uart0_init(uint baud)
{
   UCSR0B=0x00; 
   UCSR0A=0x00; 		   //控制寄存器清零
   UCSR0C=(0<<UPM00)|(3<<UCSZ00); //选择UCSRC，异步模式，禁止校验，1位停止位，8位数据位                       
   
   baud=mclk/16/baud-1;    //波特率最大为65K
   UBRR0L=baud; 					     	  
   UBRR0H=baud>>8; 		   //设置波特率

   UCSR0B|=(1<<TXEN0);   //UART0发送使能
   SREG=BIT(7);	           //全局中断开放
   DDRE|=BIT(1);	           //配置TX为输出（很重要），似乎对于MEGA1280来说没用
}

/*UART0的串口发送函数，每次发送一个字节（Byte）*/
void uart0_sendB(uchar data)
{
   while(!(UCSR0A&(BIT(UDRE0))));//判断准备就绪否
   UDR0=data;
   while(!(UCSR0A&(BIT(TXC0))));//判断完成发送否
   UCSR0A|=BIT(TXC0);//TXC0标志位手动清零，通过将TXC0置1实现
}

#pragma interrupt_handler uart0_rx:19

/*UART0的串口接收函数，每次接收一个字节（Byte）*/
void uart0_rx(void)
{
 	uchar uart0_r_byte;//UART0每次中断接收到的字符（1byte）
	UCSR0B&=~BIT(RXCIE0);//关闭RXCIE1，其余位保持不变
	uart0_r_byte=UDR0;
	if(uart0_instr_make(uart0_r_byte)==0)//通过接受字符串，产生符合要求的instr
	    {UCSR0B|=BIT(RXCIE0);}//使能RXCIE1，其余位保持不变
}

//UART0的指令识别函数，从接收到的字符中提取出以xx开头的指令字符串
uchar uart0_instr_make(uchar r_byte)
{
    uchar instr_num;//instr中已有的字符数
	uchar fun_ret;//存储本函数返回值 
    switch(uart0_r_instr_chk)//根据已有x的个数进行操作
	{
	    case 0:
			 {
			 if(r_byte=='x')
			     {				 
				 uart0_r_instr_chk=1;
				 }
			 fun_ret=0;
			 break;
			 }
		case 1:
			 {
			 if(r_byte=='x')
			     {uart0_r_instr_chk=2;}
			 else
			 	 {uart0_r_instr_chk=0;}
			 fun_ret=0;
			 break;
			 }
		case 2:
			 {
			 instr_num=strlen(uart0_instr);
			 if(instr_num==3)
			 {
			     uart0_instr[instr_num]=r_byte;
				 uart0_instr_flag=1;//instr已经满足开头xx和长度要求，flag置1，进行命令处理
			     fun_ret=1;				 
			 }
			 else
			 {
			     uart0_instr[instr_num]=r_byte;
				 fun_ret=0;
			 }
			 break;
			 }
		default:break;
	}
	return fun_ret;
}

/*UART0字符串发送函数*/
void uart0_send_string(uchar *str_send)//形参：待发送字符串
{
 	 uchar str_send_num=strlen(str_send);//待发送字符串包含的字符数，
	 	   								//数组str_send最后一位值为NULL
	 uchar i=0;
	 while(i<str_send_num)
	 {
	   uart0_sendB(*(str_send+i));
	   i+=1;
	 }
}

void uart0_send_string_with_num(uchar *str_send,uchar char_num)//形参：待发送字符串，字符串字符数
{
	 uchar i=0;
	 while(i<char_num)
	 {
	   uart0_sendB(*(str_send+i));
	   i+=1;
	 }
}

//------------【UART1的相关函数】↓-------------------------------------------------------------

/*UART1的串口初始化函数*/
void uart1_init(uint baud)
{
    UCSR1B=0x00; 
    UCSR1A=0x00; 		   //控制寄存器清零
    UCSR1C=(0<<UPM10)|(3<<UCSZ10); //选择UCSRC，异步模式，禁止校验，1位停止位，8位数据位                       
   
    baud=mclk/16/baud-1;    //波特率最大为65K
    UBRR1L=baud; 					     	  
    UBRR1H=baud>>8; 		   //设置波特率
   
    UCSR1B|=(1<<TXEN1)|(1<<RXEN1)|(1<<RXCIE1);   //接收、发送使能，接收中断使能
    SREG=BIT(7);	           //全局中断开放
    DDRD|=BIT(3);	           //配置TX为输出（很重要），似乎对于MEGA1280来说没用
   
   	//RS485芯片设置为发送，DE=PD5=1
	//注意！该芯片为半双工通信，不可同时收和发，配置引脚时应注意这一点
    DDRD|=BIT(5);
    PORTD|=BIT(5);

	DDRD|=BIT(4);
    PORTD|=BIT(4);
}


/*UART1的串口发送函数，每次发送一个字节（Byte）*/
void uart1_sendB(uchar data)
{
   while(!(UCSR1A&(BIT(UDRE1))));//判断准备就绪否
   UDR1=data;
   while(!(UCSR1A&(BIT(TXC1))));//判断完成发送否
   UCSR1A|=BIT(TXC1);//TXC1标志位手动清零，通过将TXC1置1实现
}

/*UART1字符串发送函数*/
void uart1_send_string(uchar *str_send,uchar str_num)//形参：待发送字符串
{
	 uchar i=0;
	 while(i<str_num)
	 {
	   uart1_sendB(*(str_send+i));
	   i+=1;
	   //delay(10);
	 }
}

//#pragma interrupt_handler uart1_rx:31

/*
void uart1_rx(void)
{	
    UCSR1B&=~BIT(RXCIE1);
	//rdata=UDR1;
	//flag=1;
	UCSR1B|=BIT(RXCIE1);
}
*/


//------------------【字符串处理函数】↓-------------------------------------------------------

//数组元素拷贝函数
void array_copy(uchar *array1,uchar start_index,uchar *array2,uchar copy_num)
//将array1中自第start_index位起的copy_num个元素拷贝到array2的第0到copy_num-1位
//array1的元素数目不应小于start_index+copy_num+1个，array2的元素数目不应小于copy_num个
{
    uchar i;
	for(i=0;i<copy_num;i++)
	{
	    array2[i]=array1[start_index+i];
	}
}

//字符数组或字符串比较函数：若返回0，则表示相等，否则不等
//*str0或*str1可以使数组(例如array_eg[])，也可以是字符串常量(例如"abcd")
int array_cmp(char * str0, char * str1)
{
    int i;
    for(i=0;str0[i]!=0 && str1[i]!=0 && str0[i]==str1[i];i++);
    return str0[i]-str1[i];
}

uchar* Type(uchar* Instruction)
{
    static uchar type_name[3];//static关键词很重要！否则子程序调用完成后，数组内容消失
	type_name[0]=Instruction[0];
	type_name[1]=Instruction[1];
	type_name[2]=0;
	return type_name;
}

//----------------【应变片相关函数】↓--------------------------------

//应变片读取配置
void force_data_init(void)
{
	/*A0-DT ADDO：单片机从DT读取数据;A1-SCK ADSK：单片机输出高低电平到SCK*/
	//PA0配置成高阻态输入
	DDRA&=(~BIT(0));//DDRA0=0
	PORTA&=(~BIT(0));//PORTA0=0

	//PA1配置成输出
	DDRA|=BIT(1);//DDRA1=1
}

//unsigned long型数字转成字符串，用于将应变片采集回的数据上传
uchar* ulong_to_uchar_array(unsigned long data_num)
{
 	//long型在内存中的存储 0x12345678 →低地址78+56+34+12高地址
	uchar* pNum;
	uchar force_data[5];
	pNum=(uchar *)&data_num;
	force_data[3]=*pNum;
	force_data[2]=*(++pNum);
	force_data[1]=*(++pNum);
	force_data[0]=*(++pNum);
	force_data[4]=0;
	force_high8=0x7f-force_data[1];//有效值高八位存入全局变量中
	//注意根据应变片情况调整force_high8的值，即是否用0x7f减去有效值高八位
	
	return force_data;
}

//应变采集模块数据读取程序，参照卖方示例编写
unsigned long ReadCount(void)
{
    unsigned long Count;
    unsigned char i;
	uchar* ptr_count;
	PORTA&=(~BIT(1));//ADSK=PORTA1=0
    Count=0;
    while(PINA&BIT(0));//读取PINA0=ADDO
    for(i=0;i<24;i++)
    {
        PORTA|=BIT(1);//ADSK=PORTA1=1
        Count=Count<<1;
        PORTA&=(~BIT(1));//ADSK=PORTA1=0
        if(PINA&BIT(0)) Count++;
    }
    PORTA|=BIT(1);//ADSK=PORTA1=1
    Count=Count^0x800000;
    PORTA&=(~BIT(1));//ADSK=PORTA1=0
    return(Count);
}

//保存应变片数据有效值的高八位
void command_data_save_force_high8(force_save)
{
    SREG &=(~BIT(3));//关闭全局中断
	
	while(EECR & BIT(EEWE));//判断EEWE是否为0
	EEAR=0x0030;//写地址
	EEDR=force_save;//写数据
	EECR|=BIT(EEMWE);//EEMWE置1
	EECR&=(~BIT(EEWE));//EEWE置0
	EECR|=BIT(EEWE);//EEWE置1
	
	SREG |= BIT(3);//打开全局中断
}

//舵机控制指令数据读取函数，从EEPROM中读取控制舵机所需的PARA2和PARA3，手指1移动第一阶段
void command_data_read_force_high8(uchar* PARA)
{
    SREG &=(~BIT(3));//关闭全局中断
	
	while(EECR & BIT(EEWE));//等待前面的“写”操作完成
	EEAR=0x0030;//写地址
	EECR|=BIT(EERE);//读允许位置1
	*PARA=EEDR;//读出EEDR中的数据

	SREG |= BIT(3);//打开全局中断
}

//定时计数器初始化函数
void timer1_init(void)
{
    TCCR1B=0X04;//256内部分频
	TCNT1=0xC2F6;//定时周期500ms，计算方法见开发文档
	TIFR=0x04;//定时计数器1溢出标志位清除，置1清除，上电默认为0
}

//声明中断函数
#pragma interrupt_handler timer1_interrupt_handler:15

//定时计数器1中断处理函数
void timer1_interrupt_handler(void)
{
    uchar msg_force_array[]="zz21w";
	force_ulong=ReadCount();//读取数据放入全局变量
	ulong_to_uchar_array(force_ulong);//数据类型转换
	msg_force_array[4]=force_high8;
	uart0_send_string(msg_force_array);//向上位机发送夹紧力实时数值（仅有效值高八位）
	TCNT1=0xC2F6;//需要重新设定周期500ms
}

//----------------【舵机控制相关函数】↓---------------------------------------------

//指令校验码生成函数，公式由舵机使用说明书指定
uchar ratio_command_check(uchar ID,uchar PARA2,uchar PARA3)
{
    uchar check_sum;
	check_sum=0x05+0x03+0x20+ID+PARA2+PARA3;
	return ~check_sum;
}

//舵机控制指令数据存储函数，舵机ENDLESS TURN模式下PARA2和PARA3存放到EEPROM中，手指移动0第一阶段
void command_data_save_finger_0_ratio_1(uchar PARA2,uchar PARA3)
{
    SREG &=(~BIT(3));//关闭全局中断
	
	while(EECR & BIT(EEWE));//判断EEWE是否为0
	EEAR=0x0000;//写地址
	EEDR=PARA2;//写数据
	EECR|=BIT(EEMWE);//EEMWE置1
	EECR&=(~BIT(EEWE));//EEWE置0
	EECR|=BIT(EEWE);//EEWE置1
	
	while(EECR & BIT(EEWE));//判断EEWE是否为0
	EEAR=0x0001;//写地址
	EEDR=PARA3;//写数据
	EECR|=BIT(EEMWE);//EEMWE置1
	EECR&=(~BIT(EEWE));//EEWE置0
	EECR|=BIT(EEWE);//EEWE置1
	
	SREG |= BIT(3);//打开全局中断
}

//舵机控制指令数据存储函数，舵机ENDLESS TURN模式下PARA2和PARA3存放到EEPROM中，手指0移动第二阶段
void command_data_save_finger_0_ratio_2(uchar PARA2,uchar PARA3)
{
    SREG &=(~BIT(3));//关闭全局中断
	
	while(EECR & BIT(EEWE));//判断EEWE是否为0
	EEAR=0x0002;//写地址
	EEDR=PARA2;//写数据
	EECR|=BIT(EEMWE);//EEMWE置1
	EECR&=(~BIT(EEWE));//EEWE置0
	EECR|=BIT(EEWE);//EEWE置1
	
	while(EECR & BIT(EEWE));//判断EEWE是否为0
	EEAR=0x0003;//写地址
	EEDR=PARA3;//写数据
	EECR|=BIT(EEMWE);//EEMWE置1
	EECR&=(~BIT(EEWE));//EEWE置0
	EECR|=BIT(EEWE);//EEWE置1
	
	SREG |= BIT(3);//打开全局中断
}

//舵机控制指令数据存储函数，舵机ENDLESS TURN模式下PARA2和PARA3存放到EEPROM中，手指0移动松开阶段
void command_data_save_finger_0_ratio_3(uchar PARA2,uchar PARA3)
{
    SREG &=(~BIT(3));//关闭全局中断
	
	while(EECR & BIT(EEWE));//判断EEWE是否为0
	EEAR=0x0004;//写地址
	EEDR=PARA2;//写数据
	EECR|=BIT(EEMWE);//EEMWE置1
	EECR&=(~BIT(EEWE));//EEWE置0
	EECR|=BIT(EEWE);//EEWE置1
	
	while(EECR & BIT(EEWE));//判断EEWE是否为0
	EEAR=0x0005;//写地址
	EEDR=PARA3;//写数据
	EECR|=BIT(EEMWE);//EEMWE置1
	EECR&=(~BIT(EEWE));//EEWE置0
	EECR|=BIT(EEWE);//EEWE置1
	
	SREG |= BIT(3);//打开全局中断
}

//舵机控制指令数据读取函数，从EEPROM中读取控制舵机所需的PARA2和PARA3，手指0移动第一阶段
void command_data_read_finger_0_ratio_1(uchar* PARA2,uchar* PARA3)
{
    SREG &=(~BIT(3));//关闭全局中断
	
	while(EECR & BIT(EEWE));//等待前面的“写”操作完成
	EEAR=0x0000;//写地址
	EECR|=BIT(EERE);//读允许位置1
	*PARA2=EEDR;//读出EEDR中的数据

	while(EECR & BIT(EEWE));//等待前面的“写”操作完成
	EEAR=0x0001;//写地址
	EECR|=BIT(EERE);//读允许位置1
	*PARA3=EEDR;//读出EEDR中的数据
	
	SREG |= BIT(3);//打开全局中断
}

//舵机控制指令数据读取函数，从EEPROM中读取控制舵机所需的PARA2和PARA3，手指0移动第二阶段
void command_data_read_finger_0_ratio_2(uchar* PARA2,uchar* PARA3)
{
    SREG &=(~BIT(3));//关闭全局中断
	
	while(EECR & BIT(EEWE));//等待前面的“写”操作完成
	EEAR=0x0002;//写地址
	EECR|=BIT(EERE);//读允许位置1
	*PARA2=EEDR;//读出EEDR中的数据

	while(EECR & BIT(EEWE));//等待前面的“写”操作完成
	EEAR=0x0003;//写地址
	EECR|=BIT(EERE);//读允许位置1
	*PARA3=EEDR;//读出EEDR中的数据
	
	SREG |= BIT(3);//打开全局中断
}

//舵机控制指令数据读取函数，从EEPROM中读取控制舵机所需的PARA2和PARA3，手指0移动移动阶段
void command_data_read_finger_0_ratio_3(uchar* PARA2,uchar* PARA3)
{
    SREG &=(~BIT(3));//关闭全局中断
	
	while(EECR & BIT(EEWE));//等待前面的“写”操作完成
	EEAR=0x0004;//写地址
	EECR|=BIT(EERE);//读允许位置1
	*PARA2=EEDR;//读出EEDR中的数据

	while(EECR & BIT(EEWE));//等待前面的“写”操作完成
	EEAR=0x0005;//写地址
	EECR|=BIT(EERE);//读允许位置1
	*PARA3=EEDR;//读出EEDR中的数据
	
	SREG |= BIT(3);//打开全局中断
}

//舵机控制指令数据存储函数，舵机ENDLESS TURN模式下PARA2和PARA3存放到EEPROM中，手指1移动第一阶段
void command_data_save_finger_1_ratio_1(uchar PARA2,uchar PARA3)
{
    SREG &=(~BIT(3));//关闭全局中断
	
	while(EECR & BIT(EEWE));//判断EEWE是否为0
	EEAR=0x0010;//写地址
	EEDR=PARA2;//写数据
	EECR|=BIT(EEMWE);//EEMWE置1
	EECR&=(~BIT(EEWE));//EEWE置0
	EECR|=BIT(EEWE);//EEWE置1
	
	while(EECR & BIT(EEWE));//判断EEWE是否为0
	EEAR=0x0011;//写地址
	EEDR=PARA3;//写数据
	EECR|=BIT(EEMWE);//EEMWE置1
	EECR&=(~BIT(EEWE));//EEWE置0
	EECR|=BIT(EEWE);//EEWE置1
	
	SREG |= BIT(3);//打开全局中断
}

//舵机控制指令数据存储函数，舵机ENDLESS TURN模式下PARA2和PARA3存放到EEPROM中，手指1移动第二阶段
void command_data_save_finger_1_ratio_2(uchar PARA2,uchar PARA3)
{
    SREG &=(~BIT(3));//关闭全局中断
	
	while(EECR & BIT(EEWE));//判断EEWE是否为0
	EEAR=0x0012;//写地址
	EEDR=PARA2;//写数据
	EECR|=BIT(EEMWE);//EEMWE置1
	EECR&=(~BIT(EEWE));//EEWE置0
	EECR|=BIT(EEWE);//EEWE置1
	
	while(EECR & BIT(EEWE));//判断EEWE是否为0
	EEAR=0x0013;//写地址
	EEDR=PARA3;//写数据
	EECR|=BIT(EEMWE);//EEMWE置1
	EECR&=(~BIT(EEWE));//EEWE置0
	EECR|=BIT(EEWE);//EEWE置1
	
	SREG |= BIT(3);//打开全局中断
}

//舵机控制指令数据存储函数，舵机ENDLESS TURN模式下PARA2和PARA3存放到EEPROM中，手指1移动松开阶段
void command_data_save_finger_1_ratio_3(uchar PARA2,uchar PARA3)
{
    SREG &=(~BIT(3));//关闭全局中断
	
	while(EECR & BIT(EEWE));//判断EEWE是否为0
	EEAR=0x0014;//写地址
	EEDR=PARA2;//写数据
	EECR|=BIT(EEMWE);//EEMWE置1
	EECR&=(~BIT(EEWE));//EEWE置0
	EECR|=BIT(EEWE);//EEWE置1
	
	while(EECR & BIT(EEWE));//判断EEWE是否为0
	EEAR=0x0015;//写地址
	EEDR=PARA3;//写数据
	EECR|=BIT(EEMWE);//EEMWE置1
	EECR&=(~BIT(EEWE));//EEWE置0
	EECR|=BIT(EEWE);//EEWE置1
	
	SREG |= BIT(3);//打开全局中断
}

//舵机控制指令数据读取函数，从EEPROM中读取控制舵机所需的PARA2和PARA3，手指1移动第一阶段
void command_data_read_finger_1_ratio_1(uchar* PARA2,uchar* PARA3)
{
    SREG &=(~BIT(3));//关闭全局中断
	
	while(EECR & BIT(EEWE));//等待前面的“写”操作完成
	EEAR=0x0010;//写地址
	EECR|=BIT(EERE);//读允许位置1
	*PARA2=EEDR;//读出EEDR中的数据

	while(EECR & BIT(EEWE));//等待前面的“写”操作完成
	EEAR=0x0011;//写地址
	EECR|=BIT(EERE);//读允许位置1
	*PARA3=EEDR;//读出EEDR中的数据
	
	SREG |= BIT(3);//打开全局中断
}

//舵机控制指令数据读取函数，从EEPROM中读取控制舵机所需的PARA2和PARA3，手指1移动第二阶段
void command_data_read_finger_1_ratio_2(uchar* PARA2,uchar* PARA3)
{
    SREG &=(~BIT(3));//关闭全局中断
	
	while(EECR & BIT(EEWE));//等待前面的“写”操作完成
	EEAR=0x0012;//写地址
	EECR|=BIT(EERE);//读允许位置1
	*PARA2=EEDR;//读出EEDR中的数据

	while(EECR & BIT(EEWE));//等待前面的“写”操作完成
	EEAR=0x0013;//写地址
	EECR|=BIT(EERE);//读允许位置1
	*PARA3=EEDR;//读出EEDR中的数据
	
	SREG |= BIT(3);//打开全局中断
}

//舵机控制指令数据读取函数，从EEPROM中读取控制舵机所需的PARA2和PARA3，手指1移动移动阶段
void command_data_read_finger_1_ratio_3(uchar* PARA2,uchar* PARA3)
{
    SREG &=(~BIT(3));//关闭全局中断
	
	while(EECR & BIT(EEWE));//等待前面的“写”操作完成
	EEAR=0x0014;//写地址
	EECR|=BIT(EERE);//读允许位置1
	*PARA2=EEDR;//读出EEDR中的数据

	while(EECR & BIT(EEWE));//等待前面的“写”操作完成
	EEAR=0x0015;//写地址
	EECR|=BIT(EERE);//读允许位置1
	*PARA3=EEDR;//读出EEDR中的数据
	
	SREG |= BIT(3);//打开全局中断
}


//----------------【外部中断向量定义与外部中断处理函数】↓--------------------------------

void ext_interrupt_init(void)
{
    //PD0=INT0=夹持器上方触碰，带上拉电阻输入
    DDRD&=(~BIT(0));//意思是DDRD0=0，其余位不变。但注意不可按注释的方式写！
    PORTD|=BIT(0);//意思是PORTD0=1，其余位不变。但注意不可按注释的方式写！
	
    //PD1=INT1=夹持器下方触碰，带上拉电阻输入
    DDRD&=(~BIT(1));
    PORTD|=BIT(1);

	//PE4=INT4=夹持器前方触碰，带上拉电阻输入
    DDRE&=(~BIT(4));
    PORTE|=BIT(4);
	
	//INT0和INT1的外部中断寄存器配置
    EICRA|=BIT(ISC01)|BIT(ISC11);//中断触发方式：下降沿触发
    EIMSK|=BIT(0)|BIT(1);//中断使能	

    //INT4的外部中断寄存器配置
    EICRB|=BIT(ISC41);//中断触发方式：下降沿触发
    EIMSK|=BIT(4);//中断使能
}

//夹持器上方触碰（INT0）中断向量定义 
#pragma interrupt_handler interrupt_0_handler:2

//夹持器上方触碰（INT0）中断处理函数
void interrupt_0_handler(void)
{
	if(ext_collision_alert_allow_int0)
	{
	    delay(50);
		uart0_send_string("zz41");
        ext_collision_alert_allow_int0=0;//全局变量
	}
}

//夹持器下方触碰（INT1）中断向量定义 
#pragma interrupt_handler interrupt_1_handler:3

//夹持器下方触碰（INT1）中断处理函数
void interrupt_1_handler(void)
{
	if(ext_collision_alert_allow_int1)
	{
	    delay(50);
		uart0_send_string("zz42");
        ext_collision_alert_allow_int1=0;//全局变量
	}
}

//夹持器指尖触碰（INT4）中断向量定义 
#pragma interrupt_handler interrupt_4_handler:6

//夹持器指尖触碰（INT4）中断处理函数
void interrupt_4_handler(void)
{
	if(ext_collision_alert_allow_int4)
	{
	    delay(50);
		uart0_send_string("zz43");
        ext_collision_alert_allow_int4=0;//全局变量
	}
}



//---------------------------【主函数】↓-----------------------------------------------------------
void main(void)
{

 	//.....................[函数体内变量声明]...............................

 	uchar i;//清空指令存储变量时所用循环的计数变量
	
	//向舵机发送移动命令时所用的变量（下面2个）↓
	uchar motor_command[9]={0xff,0xff,0x00,0x05,0x03,0x20,0x00,0x00,0x00};//
	uchar CHECK;//舵机指令最后一位校验码
	
	//上电手指复位阶段用到的变量（下面2个）↓
	//保证舵机停止命令只进行一次
	uchar cage_0=1;
	uchar cage_1=1;
	
	//正常工作模式（模式一）手指松开阶段用到的变量（下面2个）↓
	//保证每次夹紧操作时舵机停止命令只进行一次	
	uchar approach_0;
	uchar approach_1;
	
	//夹紧第二阶段的while循环跳出指示
	uchar hold_stage_2_continue=1;
	
	//大循环中查询两端限位和空夹所用到的变量（下面7个）↓
	//while大循环从两端限位后开始，因此不允许再次发送舵机停止命令，而是等待限位结束
	uchar stop_allow_cage_0=0;
	uchar stop_allow_cage_1=0;
	//不允许手指向端部移动，允许手指向中间移动，设置四个变量是因为考虑到两端限位跟空夹
	uchar release_allow_motor_0=0;
	uchar release_allow_motor_1=0;
	uchar hold_allow_motor_0=1;
	uchar hold_allow_motor_1=1;
	//允许上电复位后就开始检测是否空夹
	uchar stop_allow_empty=1;
	
	//调试模式手指移动速度，初始值为通过串口设定前的默认值
	uchar PARA2=0x10,PARA3=0x01;//保证低速，仅允许通过串口命令更改其值！
	
	//一般模式手指移动速度，仅允从EEPROM中获取数值
	uchar com_finger0_ratio_1_PARA2,com_finger0_ratio_1_PARA3;//手指0，Ratio 1
	uchar com_finger0_ratio_2_PARA2,com_finger0_ratio_2_PARA3;//手指0，Ratio 2
	uchar com_finger0_ratio_3_PARA2,com_finger0_ratio_3_PARA3;//手指0，Ratio 3
	uchar com_finger1_ratio_1_PARA2,com_finger1_ratio_1_PARA3;//手指1，Ratio 1
	uchar com_finger1_ratio_2_PARA2,com_finger1_ratio_2_PARA3;//手指1，Ratio 2
	uchar com_finger1_ratio_3_PARA2,com_finger1_ratio_3_PARA3;//手指1，Ratio 3
	
	uchar msg_eeprom_array[17];//向上位机返回EEPROM中数值
	uchar msg_interrupt_array[7]={'z','z','4','4','0','0','0'};//向上位机返回夹持器与外部碰撞报警（中断）允许变量的值
	
	
	//.......................[初始化配置].........................
	
    uart0_init(19200);//串口0（与上位机通信）初始化，波特率均为19200
    uart1_init(19200);//串口1（与舵机通信）初始化，波特率均为19200
	timer1_init();//定时计数器1初始化
	force_data_init();//应变片读取初始化
	
	
	//限位的配置
	
	//PE2=接近开关0，高阻态输入
	DDRE&=(~BIT(2));//DDRE2=0
	PORTE&=(~BIT(2));//PORTE2=0
	
	//PE3=接近开关1，高阻态输入
	DDRE&=(~BIT(3));//DDRE2=0
	PORTE&=(~BIT(3));//PORTE2=0
	
    //PE5=INT5=限位0，带上拉电阻输入
    DDRE&=(~BIT(5));//意思是DDRE5=0，其余位不变。但注意不可按注释的方式写！
    PORTE|=BIT(5);//意思是PORTE5=1，其余位不变。但注意不可按注释的方式写！
	
    //PE6=INT6=限位1，带上拉电阻输入
    DDRE&=(~BIT(6));
    PORTE|=BIT(6);

	//PE7=空夹，带上拉电阻输入
    DDRE&=(~BIT(7));
    PORTE|=BIT(7);
	
	
	//................[功能：上电后手指复位]....................................

    SREG |= 0X80;//打开全局中断
    
    //相关变量初始化
    cage0_state=0;
    cage1_state=0;

    //命令舵机停止转动
    uart1_send_string((uchar*)no0stop,9);
	delay(50);
	uart1_send_string((uchar*)no1stop,9);
    delay(50);
	
    //使手爪松开
    uart1_send_string((uchar*)no0release,9);
	delay(50);
    uart1_send_string((uchar*)no1release,9);

    //等待两端限位触发	
	while(cage_0|cage_1)
	{
	    if(cage_0)
		{
    	    if(!(PINE & BIT(5)))//PE5=0进入
	    	{
		        delay(50);
				if(!(PINE & BIT(5)))
				{
			        uart1_send_string((uchar*)no0stop,9);
					//uart0_send_string("zz30");
					cage_0=0;
			    }
			
		    }
		}
		
	    if(cage_1)
		{
    	    if(!(PINE & BIT(6)))//PE6=0进入
	    	{
		        delay(50);
				if(!(PINE & BIT(6)))
				{
			        uart1_send_string((uchar*)no1stop,9);
					//uart0_send_string("zz31");
					cage_1=0;
			    }
			
		    }
		}
	}
	
    uart0_send_string("zz00");//向上位机报告准备就绪
	
	UCSR0B|=(1<<RXEN0)|(1<<RXCIE0);   //UART0接收使能，接收中断使能
	
	
//........................[while(1)大循环]............................................
	
    while(1)
	{
	 	 if(uart0_instr_flag==1)
		 {
	         switch(gripper_mood)
		     {
	             case 0:
			     {
			         if(array_cmp(uart0_instr,"0100")==0)
				     {
				         //uart0_send_string(" mood 0: enter 1-regular working mood! ");
					 	 gripper_mood=1;
						 ext_interrupt_init();//外部中断（夹持器与外部环境碰撞）初始化
						 //初始化后会立即出发一次INT0和INT1，所以报警允许变量需要先置0再置1
						 
						 //夹持器与外部碰撞报警（中断）允许变量
						 ext_collision_alert_allow_int0=1;//上侧
						 ext_collision_alert_allow_int1=1;//下侧
						 ext_collision_alert_allow_int4=1;//指尖

						 
						 //获取EEPROM中存储的RATIO值
    					 command_data_read_finger_0_ratio_1(&com_finger0_ratio_1_PARA2,&com_finger0_ratio_1_PARA3);
						 command_data_read_finger_0_ratio_2(&com_finger0_ratio_2_PARA2,&com_finger0_ratio_2_PARA3);
						 command_data_read_finger_0_ratio_3(&com_finger0_ratio_3_PARA2,&com_finger0_ratio_3_PARA3);
						 command_data_read_finger_1_ratio_1(&com_finger1_ratio_1_PARA2,&com_finger1_ratio_1_PARA3);
						 command_data_read_finger_1_ratio_2(&com_finger1_ratio_2_PARA2,&com_finger1_ratio_2_PARA3);
						 command_data_read_finger_1_ratio_3(&com_finger1_ratio_3_PARA2,&com_finger1_ratio_3_PARA3);
	
						 //获取EEPROM中存储的夹紧力阈值有效值高八位
						 command_data_read_force_high8(&force_judge);
						 
	
				     }
				 
				 	 if(array_cmp(uart0_instr,"0200")==0)
				 	 {
				         //uart0_send_string(" mood 0: enter 2-configuration mood! ");
					 	 gripper_mood=2;
						 ext_interrupt_init();//外部中断（夹持器与外部环境碰撞）初始化
						 //初始化后会立即出发一次INT0和INT1，所以报警允许变量需要先置0再置1
						 
						 //夹持器与外部碰撞报警（中断）允许变量
						 ext_collision_alert_allow_int0=1;//上侧
						 ext_collision_alert_allow_int1=1;//下侧
						 ext_collision_alert_allow_int4=1;//指尖
				 	 }
				 
				 	 break;
			     }
			 
		         case 1:
			     {
			     	 if(array_cmp(uart0_instr,"1000")==0)//调试模式·读取EEPROM中存储的RATIO和力阈值
				 	 {
						 //更新ratio变量值、力阈值和消息数组的值
						 
						 //获取EEPROM中存储的RATIO值
    					 command_data_read_finger_0_ratio_1(&com_finger0_ratio_1_PARA2,&com_finger0_ratio_1_PARA3);
						 command_data_read_finger_0_ratio_2(&com_finger0_ratio_2_PARA2,&com_finger0_ratio_2_PARA3);
						 command_data_read_finger_0_ratio_3(&com_finger0_ratio_3_PARA2,&com_finger0_ratio_3_PARA3);
						 command_data_read_finger_1_ratio_1(&com_finger1_ratio_1_PARA2,&com_finger1_ratio_1_PARA3);
						 command_data_read_finger_1_ratio_2(&com_finger1_ratio_2_PARA2,&com_finger1_ratio_2_PARA3);
						 command_data_read_finger_1_ratio_3(&com_finger1_ratio_3_PARA2,&com_finger1_ratio_3_PARA3);
	
						 msg_eeprom_array[0]='z';
						 msg_eeprom_array[1]='z';
						 msg_eeprom_array[2]='3';
						 msg_eeprom_array[3]='3';
						 msg_eeprom_array[4]=com_finger0_ratio_1_PARA2;
						 msg_eeprom_array[5]=com_finger0_ratio_1_PARA3;
						 msg_eeprom_array[6]=com_finger0_ratio_2_PARA2;
						 msg_eeprom_array[7]=com_finger0_ratio_2_PARA3;
						 msg_eeprom_array[8]=com_finger0_ratio_3_PARA2;
						 msg_eeprom_array[9]=com_finger0_ratio_3_PARA3;
						 msg_eeprom_array[10]=com_finger1_ratio_1_PARA2;
						 msg_eeprom_array[11]=com_finger1_ratio_1_PARA3;
						 msg_eeprom_array[12]=com_finger1_ratio_2_PARA2;
						 msg_eeprom_array[13]=com_finger1_ratio_2_PARA3;
						 msg_eeprom_array[14]=com_finger1_ratio_3_PARA2;
						 msg_eeprom_array[15]=com_finger1_ratio_3_PARA3;
	
						 //获取EEPROM中存储的夹紧力阈值有效值高八位
						 command_data_read_force_high8(&force_judge);
						 msg_eeprom_array[16]=force_judge;
						 
						 delay(50);
						 uart0_send_string_with_num(msg_eeprom_array,17);//上传EEPROM中存储的数值
	
					 }
	
					 
					 if(array_cmp(uart0_instr,"1100")==0)//松开
				 	 {
				         if(release_allow_motor_0)
						 {
						     //构造舵机指令
							 motor_command[2]=0x00;//ID=0
						 	 motor_command[6]=com_finger0_ratio_3_PARA2;
						 	 motor_command[7]=com_finger0_ratio_3_PARA3+0x04;
						 	 CHECK=ratio_command_check(0,com_finger0_ratio_3_PARA2,com_finger0_ratio_3_PARA3+0x04);
						 	 motor_command[8]=CHECK;
						 	 delay(50);
						 	 uart1_send_string(motor_command,9);//下发指令
						 }
						 
						 if(release_allow_motor_1)
						 {
						     //构造舵机指令
							 motor_command[2]=0x01;//ID=1
						 	 motor_command[6]=com_finger1_ratio_3_PARA2;
						 	 motor_command[7]=com_finger1_ratio_3_PARA3+0x04;
						 	 CHECK=ratio_command_check(1,com_finger1_ratio_3_PARA2,com_finger1_ratio_3_PARA3+0x04);
						 	 motor_command[8]=CHECK;
						 	 delay(50);
						 	 uart1_send_string(motor_command,9);//下发指令
						 }
				 	 }
				 
				 	 if(array_cmp(uart0_instr,"1200")==0)//夹紧
				 	 {
				         if(hold_allow_motor_0 & hold_allow_motor_1)
						 {
						     //第一阶段
							 //构造舵机指令
							 motor_command[2]=0x00;//ID=0
						 	 motor_command[6]=com_finger0_ratio_1_PARA2;
						 	 motor_command[7]=com_finger0_ratio_1_PARA3;
						 	 CHECK=ratio_command_check(0,com_finger0_ratio_1_PARA2,com_finger0_ratio_1_PARA3);
						 	 motor_command[8]=CHECK;
						 	 delay(50);
						 	 uart1_send_string(motor_command,9);//下发指令
							 
							 //构造舵机指令
							 motor_command[2]=0x01;//ID=1
						 	 motor_command[6]=com_finger1_ratio_1_PARA2;
						 	 motor_command[7]=com_finger1_ratio_1_PARA3;
						 	 CHECK=ratio_command_check(1,com_finger1_ratio_1_PARA2,com_finger1_ratio_1_PARA3);
						 	 motor_command[8]=CHECK;
						 	 delay(50);
						 	 uart1_send_string(motor_command,9);//下发指令
	
							 approach_0=1;
							 approach_1=1;
							 
							 //等接近开关触发，同时查询检测是否空夹
							 while((approach_0|approach_1)&&(PINE & BIT(7)))
							 {
	   						     if(approach_0)
								 {
								     if(!(PINE & BIT(2)))//PE2=0进入
									 {
		        					     delay(50);
										 if(!(PINE & BIT(2)))
										 {
			        					     uart1_send_string((uchar*)no0stop,9);
											 //uart0_send_string(" interrupt 5 ");
											 approach_0=0;
			    						 }
			
		    						 }
							 	 }
		
	    					 	 if(approach_1)
								 {
    	    					     if(!(PINE & BIT(3)))//PE3=0进入
	    						 	 {
		       					          delay(50);
								  	  	  if(!(PINE & BIT(3)))
								  	  	  {
			        			  	          uart1_send_string((uchar*)no1stop,9);
								  		  	  //uart0_send_string(" interrupt 6 ");
								  		  	  approach_1=0;
			    				          }
		    					     }
								 }
							 }
							 
							 if(PINE & BIT(7))//不空夹则进行第二阶段
							 {
							    //第二阶段
							 	//构造舵机指令
							 	motor_command[2]=0x00;//ID=0
						 	 	motor_command[6]=com_finger0_ratio_2_PARA2;
						 	 	motor_command[7]=com_finger0_ratio_2_PARA3;
						 	 	CHECK=ratio_command_check(0,com_finger0_ratio_2_PARA2,com_finger0_ratio_2_PARA3);
						 	 	motor_command[8]=CHECK;
						 	 	delay(50);
						 	 	uart1_send_string(motor_command,9);//下发指令
							 
							 	//构造舵机指令
								 motor_command[2]=0x01;//ID=1
						 	 	 motor_command[6]=com_finger1_ratio_2_PARA2;
						 		 motor_command[7]=com_finger1_ratio_2_PARA3;
						 	 	 CHECK=ratio_command_check(1,com_finger1_ratio_2_PARA2,com_finger1_ratio_2_PARA3);
						 		 motor_command[8]=CHECK;
						 	 	 delay(50);
						 	 	 uart1_send_string(motor_command,9);//下发指令
							 
							 	 //等待夹紧力达到阈值并继续判断是否空夹
							 	 force_high8=0;
								 hold_stage_2_continue=1;
							 	 while(hold_stage_2_continue && (PINE & BIT(7)))
							 	 {
								     force_ulong=ReadCount();
								 	 ulong_to_uchar_array(force_ulong);//目的是获取force_high8
									 if(force_high8>=force_judge)
									 {
									     hold_stage_2_continue=0;//下次循环跳出
										 
										 //停止舵机运行
							 	 		 delay(50);
							 	 		 uart1_send_string((uchar*)no0stop,9);
							 	 		 delay(50);
							 	 		 uart1_send_string((uchar*)no1stop,9);
							 
							 	 		 hold_allow_motor_0=0;//禁止0号手指向端部靠近
							 	 		 hold_allow_motor_1=0;//禁止1号手指向端部靠近
							 
							 	 		 //报告上位机已经夹紧
							 	 		 delay(50);
							 	 		 uart0_send_string("zz10");
										 delay(50);
							 	 		 uart0_send_string("zz10");
									 }
							 	 }
							 
							 	 
							 }
							 
						 }
				 	 }
					 
					 if(array_cmp(Type(uart0_instr),"13")==0)//设定手指0，Ratio 1
				 	 {
					     com_finger0_ratio_1_PARA2=uart0_instr[2];
						 com_finger0_ratio_1_PARA3=uart0_instr[3];
						 command_data_save_finger_0_ratio_1(com_finger0_ratio_1_PARA2,com_finger0_ratio_1_PARA3);
					 }
					 
					 if(array_cmp(Type(uart0_instr),"14")==0)//设定手指0，Ratio 2
				 	 {
					     com_finger0_ratio_2_PARA2=uart0_instr[2];
						 com_finger0_ratio_2_PARA3=uart0_instr[3];
						 command_data_save_finger_0_ratio_2(com_finger0_ratio_2_PARA2,com_finger0_ratio_2_PARA3);
					 }
					 
					 if(array_cmp(Type(uart0_instr),"15")==0)//设定手指0，Ratio 3
				 	 {
					     com_finger0_ratio_3_PARA2=uart0_instr[2];
						 com_finger0_ratio_3_PARA3=uart0_instr[3];
						 command_data_save_finger_0_ratio_3(com_finger0_ratio_3_PARA2,com_finger0_ratio_3_PARA3);
					 }
					 
					 if(array_cmp(Type(uart0_instr),"16")==0)//设定手指1，Ratio 1
				 	 {
					     com_finger1_ratio_1_PARA2=uart0_instr[2];
						 com_finger1_ratio_1_PARA3=uart0_instr[3];
						 command_data_save_finger_1_ratio_1(com_finger1_ratio_1_PARA2,com_finger1_ratio_1_PARA3);
					 }
					 
					 if(array_cmp(Type(uart0_instr),"17")==0)//设定手指1，Ratio 2
				 	 {
					     com_finger1_ratio_2_PARA2=uart0_instr[2];
						 com_finger1_ratio_2_PARA3=uart0_instr[3];
						 command_data_save_finger_1_ratio_2(com_finger1_ratio_2_PARA2,com_finger1_ratio_2_PARA3);
					 }
					 
					 if(array_cmp(Type(uart0_instr),"18")==0)//设定手指1，Ratio 3
				 	 {
					     com_finger1_ratio_3_PARA2=uart0_instr[2];
						 com_finger1_ratio_3_PARA3=uart0_instr[3];
						 command_data_save_finger_1_ratio_3(com_finger1_ratio_3_PARA2,com_finger1_ratio_3_PARA3);
					 }
					 
					 if(array_cmp(Type(uart0_instr),"19")==0)//设定force_judge
				 	 {
					     force_judge=uart0_instr[2];
						 command_data_save_force_high8(force_judge);
					 }
				 
				 	 break;
			     }
			 
		         case 2:
			     {
					 if(array_cmp(uart0_instr,"2100")==0)//调试模式·手指0停止
				 	 {
					     TIMSK&=(~BIT(2));//屏蔽定时计数1中断，停止向上返回夹持力值
						 uart1_send_string((uchar*)no0stop,9);
				 	 }

					 if(array_cmp(uart0_instr,"2101")==0)//调试模式·手指0松开方向移动
				 	 {
					     if(release_allow_motor_0)
						 {
						     TIMSK&=(~BIT(2));//屏蔽定时计数1中断，停止向上返回夹持力值
						 	 motor_command[2]=0x00;//ID=0
						 	 motor_command[6]=PARA2;
						 	 motor_command[7]=PARA3+0x04;//顺时针，绝对不可在此更改PARA3的值！
						 	 CHECK=ratio_command_check(0,PARA2,PARA3+0x04);
						 	 motor_command[8]=CHECK;
						 	 delay(50);
						 	 uart1_send_string(motor_command,9);
						 	 //delay(50);
						 	 //uart0_send_string_with_num(motor_command,9);
						 	 TIMSK|=BIT(2);//打开定时计数1中断，向上返回夹持力值
						 }
				 	 }
					 
					 if(array_cmp(uart0_instr,"2102")==0)//调试模式·手指0夹紧方向移动
				 	 {
					     if(hold_allow_motor_0)
						 {
						     TIMSK&=(~BIT(2));//屏蔽定时计数1中断，停止向上返回夹持力值
						 	 motor_command[2]=0x00;//ID=0
						 	 motor_command[6]=PARA2;
						 	 motor_command[7]=PARA3;
						 	 CHECK=ratio_command_check(0,PARA2,PARA3);
						 	 motor_command[8]=CHECK;
						 	 delay(50);
						 	 uart1_send_string(motor_command,9);
						 	 //delay(50);
						 	 //uart0_send_string_with_num(motor_command,9);
						 	 TIMSK|=BIT(2);//打开定时计数1中断，向上返回夹持力值
						 }
				 	 }

					 if(array_cmp(uart0_instr,"2110")==0)//调试模式·手指1停止
				 	 {
					     TIMSK&=(~BIT(2));//屏蔽定时计数1中断，停止向上返回夹持力值
						 uart1_send_string((uchar*)no1stop,9);
				 	 }

					 if(array_cmp(uart0_instr,"2111")==0)//调试模式·手指1松开方向移动
				 	 {
					     if(release_allow_motor_1)
						 {
						     TIMSK&=(~BIT(2));//屏蔽定时计数1中断，停止向上返回夹持力值
						 	 motor_command[2]=0x01;//ID=1
						 	 motor_command[6]=PARA2;
						 	 motor_command[7]=PARA3+0x04;//顺时针，绝对不可在此更改PARA3的值！
						 	 CHECK=ratio_command_check(1,PARA2,PARA3+0x04);
						 	 motor_command[8]=CHECK;
						 	 delay(50);
						 	 uart1_send_string(motor_command,9);
						 	 //delay(50);
						 	 //uart0_send_string_with_num(motor_command,9);
						 	 TIMSK|=BIT(2);//打开定时计数1中断，向上返回夹持力值
						 }
				 	 }
					 
					 if(array_cmp(uart0_instr,"2112")==0)//调试模式·手指1夹紧方向移动
				 	 {
					     if(hold_allow_motor_1)
						 {
						     TIMSK&=(~BIT(2));//屏蔽定时计数1中断，停止向上返回夹持力值
						 	 motor_command[2]=0x01;//ID=1
						 	 motor_command[6]=PARA2;
						 	 motor_command[7]=PARA3;
						 	 CHECK=ratio_command_check(1,PARA2,PARA3);
						 	 motor_command[8]=CHECK;
						 	 delay(50);
						 	 uart1_send_string(motor_command,9);
						 	 //delay(50);
						 	 //uart0_send_string_with_num(motor_command,9);
						 	 TIMSK|=BIT(2);//打开定时计数1中断，向上返回夹持力值
						 }
				 	 }

					 if(array_cmp(Type(uart0_instr),"22")==0)
				 	 {
						 TIMSK&=(~BIT(2));//屏蔽定时计数1中断，停止向上返回夹持力值
						 delay(50);
						 uart1_send_string((uchar*)no0stop,9);
						 delay(50);
						 uart1_send_string((uchar*)no1stop,9);
						 PARA2=uart0_instr[2];
						 PARA3=uart0_instr[3];
						 //delay(50);
						 //uart0_send_string("ratio changed");
				 	 }
					 
					 if(array_cmp(uart0_instr,"2301")==0)//调试模式·设置手指0速度1(两指相同)夹紧第一阶段
				 	 {
					     command_data_save_finger_0_ratio_1(PARA2,PARA3);
						 //delay(50);
						 //uart0_send_string(" Finger-0 Ratio-1 Set Successfully! ");
					 }
					 
					 if(array_cmp(uart0_instr,"2302")==0)//调试模式·设置手指0速度2(两指相同)夹紧第二阶段
				 	 {
					     command_data_save_finger_0_ratio_2(PARA2,PARA3);
						 //delay(50);
						 //uart0_send_string(" Finger-0 Ratio-2 Set Successfully! ");
					 }
					 
					 if(array_cmp(uart0_instr,"2303")==0)//调试模式·设置手指0速度3(两指相同)松开阶段
				 	 {
					     command_data_save_finger_0_ratio_3(PARA2,PARA3);
						 //delay(50);
						 //uart0_send_string(" Finger-0 Ratio-3 Set Successfully! ");
					 }
					 
					 if(array_cmp(uart0_instr,"2311")==0)//调试模式·设置手指1速度1(两指相同)夹紧第一阶段
				 	 {
					     command_data_save_finger_1_ratio_1(PARA2,PARA3);
						 //delay(50);
						 //uart0_send_string(" Finger-1 Ratio-1 Set Successfully! ");
					 }
					 
					 if(array_cmp(uart0_instr,"2312")==0)//调试模式·设置手指1速度2(两指相同)夹紧第二阶段
				 	 {
					     command_data_save_finger_1_ratio_2(PARA2,PARA3);
						 //delay(50);
						 //uart0_send_string(" Finger-1 Ratio-2 Set Successfully! ");
					 }
					 
					 if(array_cmp(uart0_instr,"2313")==0)//调试模式·设置手指1速度3(两指相同)松开阶段
				 	 {
					     command_data_save_finger_1_ratio_3(PARA2,PARA3);
						 //delay(50);
						 //uart0_send_string(" Finger-1 Ratio-3 Set Successfully! ");
					 }
					 
					 if(array_cmp(Type(uart0_instr),"24")==0)//调试模式·设置夹紧力阈值
				 	 {
					     force_judge=uart0_instr[2];
						 command_data_save_force_high8(force_judge);
						 //delay(50);
						 //uart0_send_string(" Force Set Successfully! ");
					 }
					 
					 
					 if(array_cmp(uart0_instr,"2500")==0)//调试模式·读取EEPROM中存储的RATIO和力阈值
				 	 {
						 //更新ratio变量值、力阈值和消息数组的值
						 
						 //获取EEPROM中存储的RATIO值
    					 command_data_read_finger_0_ratio_1(&com_finger0_ratio_1_PARA2,&com_finger0_ratio_1_PARA3);
						 command_data_read_finger_0_ratio_2(&com_finger0_ratio_2_PARA2,&com_finger0_ratio_2_PARA3);
						 command_data_read_finger_0_ratio_3(&com_finger0_ratio_3_PARA2,&com_finger0_ratio_3_PARA3);
						 command_data_read_finger_1_ratio_1(&com_finger1_ratio_1_PARA2,&com_finger1_ratio_1_PARA3);
						 command_data_read_finger_1_ratio_2(&com_finger1_ratio_2_PARA2,&com_finger1_ratio_2_PARA3);
						 command_data_read_finger_1_ratio_3(&com_finger1_ratio_3_PARA2,&com_finger1_ratio_3_PARA3);
	
						 msg_eeprom_array[0]='z';
						 msg_eeprom_array[1]='z';
						 msg_eeprom_array[2]='3';
						 msg_eeprom_array[3]='3';
						 msg_eeprom_array[4]=com_finger0_ratio_1_PARA2;
						 msg_eeprom_array[5]=com_finger0_ratio_1_PARA3;
						 msg_eeprom_array[6]=com_finger0_ratio_2_PARA2;
						 msg_eeprom_array[7]=com_finger0_ratio_2_PARA3;
						 msg_eeprom_array[8]=com_finger0_ratio_3_PARA2;
						 msg_eeprom_array[9]=com_finger0_ratio_3_PARA3;
						 msg_eeprom_array[10]=com_finger1_ratio_1_PARA2;
						 msg_eeprom_array[11]=com_finger1_ratio_1_PARA3;
						 msg_eeprom_array[12]=com_finger1_ratio_2_PARA2;
						 msg_eeprom_array[13]=com_finger1_ratio_2_PARA3;
						 msg_eeprom_array[14]=com_finger1_ratio_3_PARA2;
						 msg_eeprom_array[15]=com_finger1_ratio_3_PARA3;
	
						 //获取EEPROM中存储的夹紧力阈值有效值高八位
						 command_data_read_force_high8(&force_judge);
						 msg_eeprom_array[16]=force_judge;
						 
						 delay(50);
						 uart0_send_string_with_num(msg_eeprom_array,17);//上传EEPROM中存储的数值
	
					 }
					 
				     break;
			     }
			 
		         default:break;
			 }
			 
			 if(array_cmp(uart0_instr,"3100")==0)//恢复触碰报警 ext interrupt 0 
			 {
			     ext_collision_alert_allow_int0=1;//上侧
			 }
			 
			 if(array_cmp(uart0_instr,"3200")==0)//恢复触碰报警 ext interrupt 1 
			 {
			     ext_collision_alert_allow_int1=1;//下侧
			 }
			 
			 if(array_cmp(uart0_instr,"3300")==0)//恢复触碰报警 ext interrupt 4 
			 {
			     ext_collision_alert_allow_int4=1;//指尖
			 }
			 
			 if(array_cmp(uart0_instr,"3400")==0)//读取报警允许变量的状态
			 {
			     msg_interrupt_array[4]=ext_collision_alert_allow_int0;
				 msg_interrupt_array[5]=ext_collision_alert_allow_int1;
				 msg_interrupt_array[6]=ext_collision_alert_allow_int4;
				 uart0_send_string_with_num(msg_interrupt_array,7);
			 }
			 
			 /*末尾应完成命令执行后的还原工作A-D*/
			 uart0_instr_flag=0; //A.命令接收标志位置0
			 uart0_r_instr_chk=0;//B.命令构造字符数计数置0
			 for(i=0;i<12;i++)//C.命令清除
			 {
		         uart0_instr[i]=0;
			 }
			 UCSR0B|=BIT(RXCIE0);//D.恢复UART0的接收中断			
	     }
		 

		/*
		    【偏向撞击保护的编程】
			（1）一旦手指碰撞到限位开关产生低电平，哪怕是抖动、不稳定的低电平，
			也要禁止手指继续向碰撞位置移动，此时不需要延迟防抖的处理；
			（2）只有当手指真正完全地离开了碰撞地点，限位开关IO成为稳定的高电平，
			才允许手指再次向碰撞的方向移动。
		*/
		
		if(!(PINE & BIT(5)))//检测手指0是否复位
		{
			if(stop_allow_cage_0)
			{
			    release_allow_motor_0=0;//禁止1号手指向端部靠近
				hold_allow_motor_0=1;//允许1号手指向中间靠近
				TIMSK&=(~BIT(2));//屏蔽定时计数1中断，停止向上返回夹持力值
				delay(50);
				uart1_send_string((uchar*)no0stop,9);
				delay(50);
				uart0_send_string("zz30");
				delay(50);
				uart0_send_string("zz30");
				stop_allow_cage_0=0;
			}
		}
		else
		{
		    if(!stop_allow_cage_0)
			{
			    delay(500);
			    if(PINE & BIT(5))
				{
				    release_allow_motor_0=1;//允许1号手指向端部靠近
					stop_allow_cage_0=1;
				}
			}
		}
		
		if(!(PINE & BIT(6)))//检测手指1是否复位
		{
			if(stop_allow_cage_1)
			{
			    release_allow_motor_1=0;//禁止1号手指向端部靠近
				hold_allow_motor_1=1;//允许1号手指向中间靠近
				TIMSK&=(~BIT(2));//屏蔽定时计数1中断，停止向上返回夹持力值
				delay(50);
				uart1_send_string((uchar*)no1stop,9);
				delay(50);
				uart0_send_string("zz31");
				delay(50);
				uart0_send_string("zz31");
				stop_allow_cage_1=0;
			}
		}
		else
		{
			if(!stop_allow_cage_1)
			{
			    delay(500);
			    if(PINE & BIT(6))
				{
				    release_allow_motor_1=1;//允许1号手指向端部靠近
					stop_allow_cage_1=1;
				}
			}
		}

		if(!(PINE & BIT(7)))//检测是否空夹
		{
			if(stop_allow_empty)
			{
			    hold_allow_motor_0=0;//禁止0号手指向端部靠近
				hold_allow_motor_1=0;//禁止1号手指向端部靠近
				TIMSK&=(~BIT(2));//屏蔽定时计数1中断，停止向上返回夹持力值
				delay(50);
				uart1_send_string((uchar*)no1stop,9);
				delay(50);
				uart1_send_string((uchar*)no0stop,9);
				delay(50);
				uart0_send_string("zz32");
				delay(50);
				uart0_send_string("zz32");
				stop_allow_empty=0;
			}
		}
		else
		{
			if(!stop_allow_empty)
			{
			    delay(500);
			    if(PINE & BIT(7))
				{
				    hold_allow_motor_0=1;//允许0号手指向中间靠近
					hold_allow_motor_1=1;//允许1号手指向中间靠近
					stop_allow_empty=1;
				}
			}
		}
		
	}
}

/*
【开发者说】

大循环限位检测时，为了避免做必要的检测，特意使用了一些变量进行限制，这些变量
是从“何时才能”检测这个角度设置的，因此这些变量的值并不能完全表明手指运动的
状态。这样做的缺点是不可避免有人特意扰乱夹持器执行动作，例如：当夹持器手指向
中间方向移动时（即夹紧物体时），若有人故意按下两端限位开关，则夹持器会认为手
指已经移动到端部。但很明显这样是不可能的，因为舵机根本没有往端部方向移动。如
果要防止有人故意运行，则建议将那些用于对检测做限制的变量改为反映手指运行状态
的变量，根据手指动作状态来决定是否检测某些限位。

另外，不使用中断是因为始终没有解决多次触发的问题，即使在使用软件消抖的前提下。

调试配置模式下何时决定停止读取夹紧力值也存在bug——两指均运行，此时停止其中一
个，另外一个保持运动时，读取已经停止了。但这个bug在实际配置测量力阈值时应该不
会出现，因为我们往往先终止一个手指的运动，靠调节另一个手指的位置来设置力阈值。

*/