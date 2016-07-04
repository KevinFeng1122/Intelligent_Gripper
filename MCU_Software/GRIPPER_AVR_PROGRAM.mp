NOTE: AVR Code addresses are word addresses
All other addresses (including ones in FLASH) are byte addresses

Area                               Addr   Size   Decimal Bytes (Attributes)
--------------------------------   ----   ----   ------- ----- ------------
                           idata   008C   00F4 =    244. bytes (rel,con,rom)

       Addr  Global Symbol
      -----  --------------------------------
       008C  __idata_start
       0180  __idata_end

Area                               Addr   Size   Decimal Bytes (Attributes)
--------------------------------   ----   ----   ------- ----- ------------
                            text   0180   1570 =   5488. bytes (rel,con,rom)

       Addr  Global Symbol
      -----  --------------------------------
       00C0  __start
       00C0  __text_start
       00E2  _exit
       00E3  _delay
       00F8  _uart0_init
       0126  _uart0_sendB
       012D  _uart0_rx
       0165  _uart0_instr_make
       01A9  _uart0_send_string
       01BD  _uart0_send_string_with_num
       01CE  _uart1_init
       0207  _uart1_sendB
       0217  _uart1_send_string
       0228  _array_copy
       0242  _array_cmp
       026D  _Type
       027A  _force_data_init
       027E  _ulong_to_uchar_array
       02A6  _ReadCount
       02EC  _command_data_save_force_high8
       02F9  _command_data_read_force_high8
       0306  _timer1_init
       030F  _timer1_interrupt_handler
       035C  _ratio_command_check
       0367  _command_data_save_finger_0_ratio_1
       037E  _command_data_save_finger_0_ratio_2
       0395  _command_data_save_finger_0_ratio_3
       03AC  _command_data_read_finger_0_ratio_1
       03C3  _command_data_read_finger_0_ratio_2
       03DA  _command_data_read_finger_0_ratio_3
       03F1  _command_data_save_finger_1_ratio_1
       0408  _command_data_save_finger_1_ratio_2
       041F  _command_data_save_finger_1_ratio_3
       0436  _command_data_read_finger_1_ratio_1
       044D  _command_data_read_finger_1_ratio_2
       0464  _command_data_read_finger_1_ratio_3
       047B  _ext_interrupt_init
       0490  _interrupt_0_handler
       04CA  _interrupt_1_handler
       0504  _interrupt_4_handler
       053E  _main
       0AB2  _strlen
       0ABE  push_arg4
       0AC0  push_arg2
       0AC3  div32u
       0AC5  mod32u
       0AC9  div32s
       0ACB  mod32s
       0AF7  long_div_prolog
       0B0A  long_div_epilog
       0B1A  tst_R16_R19
       0B1F  tst_R24_R27
       0B24  neg_R24_R27
       0B2D  pop_xgset00FC
       0B34  push_xgsetF0FC
       0B36  push_xgset30FC
       0B38  push_xgset00FC
       0B3A  push_xgset003C
       0B3F  push_xgsetF000
       0B44  pop_xgsetF000
       0B49  push_xgsetF00C
       0B50  pop_xgsetF00C
       0B57  neg32
       0B60  asgnblk
       0B78  __text_end

Area                               Addr   Size   Decimal Bytes (Attributes)
--------------------------------   ----   ----   ------- ----- ------------
                            data   0100   00F4 =    244. bytes (rel,con,ram)

       Addr  Global Symbol
      -----  --------------------------------
       0100  _Ratio1
       0100  __data_start
       0101  _Ratio2
       0102  _Force
       0103  _no0release
       010C  _no0stop
       0115  _no1release
       011E  _no1stop
       0127  _gripper_mood
       0128  _uart0_r_instr_chk
       0129  _uart0_instr_flag
       012A  _ext_collision_alert_allow_int0
       012B  _ext_collision_alert_allow_int1
       012C  _ext_collision_alert_allow_int4
       01F4  __data_end

Area                               Addr   Size   Decimal Bytes (Attributes)
--------------------------------   ----   ----   ------- ----- ------------
                             bss   01F4   0011 =     17. bytes (rel,con,ram)

       Addr  Global Symbol
      -----  --------------------------------
       01F4  Gripper_AVR_program.c:type_name
       01F4  __bss_start
       01F7  _force_ulong
       01FB  _force_judge
       01FC  _force_high8
       01FD  _uart0_instr
       0202  _uart0_rdata_byte
       0203  _cage1_state
       0204  _cage0_state
       0205  __bss_end

Area                               Start  End    Decimal Bytes (Attributes)
--------------------------------   ----   ----   ------- ----- ------------
                          vector   0000   004C =     76. bytes (abs,ovr,rom)

Files Linked      [ module(s) ]

C:\iccv7avr\lib\crtatmega.o	[ crtatmega.s ]
Gripper_AVR_program.o	[ Gripper_AVR_program.c ]
<library>	[ strlen.s, asave.s, div32.s, gpop3.s, gpush.s, gpushy.s, gpushz.s, lneg.s, struct.s ]

User Global Definitions

ram_end = 0x10ff
hwstk_size = 0x1e

User Base Address Definitions

func_lit = 0x8c
data = 0x100
eeprom:0.4096

