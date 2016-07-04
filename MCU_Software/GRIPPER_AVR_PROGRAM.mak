CC = iccavr
LIB = ilibw
CFLAGS =  -e -D__ICC_VERSION=722 -DATMega128  -l -g -MLongJump -MHasMul -MEnhanced -Wf-use_elpm 
ASFLAGS = $(CFLAGS) 
LFLAGS =  -g -e:0x20000 -ucrtatmega.o -bfunc_lit:0x8c.0x20000 -dram_end:0x10ff -bdata:0x100.0x10ff -dhwstk_size:30 -beeprom:0.4096 -fihx_coff -S2
FILES = Gripper_AVR_program.o 

GRIPPER_AVR_PROGRAM:	$(FILES)
	$(CC) -o GRIPPER_AVR_PROGRAM $(LFLAGS) @GRIPPER_AVR_PROGRAM.lk   -lstudio -lcatm128
Gripper_AVR_program.o: C:\iccv7avr\include\iom128v.h C:\iccv7avr\include\macros.h C:\iccv7avr\include\AVRdef.h C:\iccv7avr\include\string.h C:\iccv7avr\include\_const.h
Gripper_AVR_program.o:	Gripper_AVR_program.c
	$(CC) -c $(CFLAGS) Gripper_AVR_program.c
