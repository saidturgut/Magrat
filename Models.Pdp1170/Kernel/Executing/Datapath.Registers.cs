namespace Models.Pdp1170.Kernel.Executing;

public partial class Datapath
{
    private readonly CommitRegister[] CoreRegisters =  new CommitRegister[coreRegisters];
    private readonly CommitRegister[] TemporaryLatches =  new CommitRegister[tempLatches];
    private readonly CommitRegister[] GeneralRegisters =  new CommitRegister[12];
    private readonly CommitRegister[] StackPointers =  new CommitRegister[4];

    private const byte coreRegisters = 4;
    private const byte tempLatches = 6;

    private CommitRegister Point(Pointer pointer) => pointer switch
    {
        Pointer.NIL or Pointer.IR or Pointer.PSW or Pointer.PC => CoreRegisters[(byte)pointer],
        Pointer.R0 or Pointer.R1 or Pointer.R2 or Pointer.R3 or Pointer.R4 or Pointer.R5
            => GeneralRegisters[(byte)pointer - coreRegisters - tempLatches + (statusWord.RegisterSet * 6)],
        Pointer.SP => StackPointers[(byte)statusWord.CurrentMode],
        _ => TemporaryLatches[(byte)pointer - coreRegisters]
    };
    
    private CommitRegister PointCore(Pointer pointer)
        => CoreRegisters[(byte)pointer];
    private CommitRegister PointLatch(Pointer pointer)
        => TemporaryLatches[(byte)pointer - CoreRegisters.Length];

    private CommitRegister DecodeAddress(uint address) => address switch
    {
        0x7FFFFE => PointCore(Pointer.PSW), // PSW
        /*
        0x7FFFFC => ControlRegisters[(byte)Mapping.STACK_LIMIT],
        0x7FFFFA => ControlRegisters[(byte)Mapping.PIRQ],
        0x7FFFF8 => ControlRegisters[(byte)Mapping.BREAK],
        0x7FFFF6 => ControlRegisters[(byte)Mapping.CPU_ERROR],
        0x7FFFF4 => ControlRegisters[(byte)Mapping.SYSTEM_ID],
        0x7FFFF2 => ControlRegisters[(byte)Mapping.UP_SIZE],
        0x7FFFF0 => ControlRegisters[(byte)Mapping.LOW_SIZE],
        0x7FFFEA => ControlRegisters[(byte)Mapping.HIT_MISS],
        0x7FFFE8 => ControlRegisters[(byte)Mapping.MAINTENANCE],
        0x7FFFE6 => ControlRegisters[(byte)Mapping.CONTROL],
        0x7FFFE4 => ControlRegisters[(byte)Mapping.MEM_SYS],
        0x7FFFE2 => ControlRegisters[(byte)Mapping.HIGH_ERR],
        0x7FFFE0 => ControlRegisters[(byte)Mapping.LOW_ERR],
        */
        
        0x7FFFCF => StackPointers[3], // SP_U
        0x7FFFCE => StackPointers[1], // SP_S
        0x7FFFCD => GeneralRegisters[11], // R5_1
        0x7FFFCC => GeneralRegisters[10], // R4_1
        0x7FFFCB => GeneralRegisters[9], // R3_1
        0x7FFFCA => GeneralRegisters[8], // R2_1
        0x7FFFC9 => GeneralRegisters[7], // R1_1
        0x7FFFC8 => GeneralRegisters[6], // R0_1
        
        0x7FFFC7 => PointCore(Pointer.PC), // PC
        0x7FFFC6 => StackPointers[0], // SP_K
        0x7FFFC5 => GeneralRegisters[5], // R5_0
        0x7FFFC4 => GeneralRegisters[4], // R4_0
        0x7FFFC3 => GeneralRegisters[3], // R3_0
        0x7FFFC2 => GeneralRegisters[2], // R2_0
        0x7FFFC1 => GeneralRegisters[1], // R1_0
        0x7FFFC0 => GeneralRegisters[0], // R0_0
    };

    private ushort ReadRegister()
        => DecodeAddress(addressLatch).Get();

    private void WriteRegister(ushort data)
        => DecodeAddress(addressLatch).Set(data);
}