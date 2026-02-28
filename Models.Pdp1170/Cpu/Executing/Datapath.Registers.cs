namespace Models.Pdp1170.Cpu.Executing;

public partial class Datapath
{
    private readonly Register[] CoreRegisters =  new Register[4];
    private readonly Register[] TemporaryLatches =  new Register[6];
    private readonly Register[] GeneralRegisters =  new Register[12];
    private readonly Register[] StackPointers =  new Register[4];
    
    private Register Point(Pointer pointer) => pointer switch
    {
        Pointer.R0 or Pointer.R1 or Pointer.R2 or Pointer.R3 or Pointer.R4 or Pointer.R5 
            => GeneralRegisters[(byte)pointer - CoreRegisters.Length + (statusWord.RegisterSet * 6)],
        Pointer.SP => StackPointers[(byte)statusWord.CurrentMode],
        Pointer.NIL or Pointer.IR or Pointer.PSW or Pointer.PC => CoreRegisters[(byte)pointer],
        _ => TemporaryLatches[(byte)pointer]
    };

    private Register AddressRegister(uint address) => address switch
    {
        0x7FFFFE => CoreRegisters[(byte)Pointer.PSW],
        
        //0x7FFFFC => ControlRegisters[(byte)Mapping.STACK_LIMIT],
        //0x7FFFFA => ControlRegisters[(byte)Mapping.PIRQ],
        //0x7FFFF8 => ControlRegisters[(byte)Mapping.BREAK],
        //0x7FFFF6 => ControlRegisters[(byte)Mapping.CPU_ERROR],
        //0x7FFFF4 => ControlRegisters[(byte)Mapping.SYSTEM_ID],
        //0x7FFFF2 => ControlRegisters[(byte)Mapping.UP_SIZE],
        //0x7FFFF0 => ControlRegisters[(byte)Mapping.LOW_SIZE],
        //0x7FFFEA => ControlRegisters[(byte)Mapping.HIT_MISS],
        //0x7FFFE8 => ControlRegisters[(byte)Mapping.MAINTENANCE],
        //0x7FFFE6 => ControlRegisters[(byte)Mapping.CONTROL],
        //0x7FFFE4 => ControlRegisters[(byte)Mapping.MEM_SYS],
        //0x7FFFE2 => ControlRegisters[(byte)Mapping.HIGH_ERR],
        //0x7FFFE0 => ControlRegisters[(byte)Mapping.LOW_ERR],
        
        0x7FFFCF => StackPointers[3], // SP_U
        0x7FFFCE => StackPointers[1], // SP_S
        0x7FFFCD => GeneralRegisters[11], // R5
        0x7FFFCC => GeneralRegisters[10], // R4
        0x7FFFCB => GeneralRegisters[9], // R3
        0x7FFFCA => GeneralRegisters[8], // R2
        0x7FFFC9 => GeneralRegisters[7], // R1
        0x7FFFC8 => GeneralRegisters[6], // R0
        
        0x7FFFC7 => CoreRegisters[(byte)Pointer.PC], // R7
        0x7FFFC6 => StackPointers[0], // SP_K
        0x7FFFC5 => GeneralRegisters[5], // R5
        0x7FFFC4 => GeneralRegisters[4], // R4
        0x7FFFC3 => GeneralRegisters[3], // R3
        0x7FFFC2 => GeneralRegisters[2], // R2
        0x7FFFC1 => GeneralRegisters[1], // R1
        0x7FFFC0 => GeneralRegisters[0], // R0
    };

    private ushort ReadRegister()
        => AddressRegister(addressLatch).Get();

    private void WriteRegister(ushort data)
        => AddressRegister(addressLatch).Set(data);
}