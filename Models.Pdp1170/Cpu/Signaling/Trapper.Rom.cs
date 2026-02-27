namespace Models.Pdp1170.Cpu.Signaling;

public partial class Trapper
{
    private static readonly TrapInfo[] TrapsTable =
    [
        new (Trap.NONE, 0x00, 7),

        // ABORT TRAPS
        new (Trap.MMU_ABORT, 0xA8, 0),
        new (Trap.PARITY_ABORT, 0x4C, 0),
        new (Trap.BUS_ABORT, 0x04, 1),
        new (Trap.STACK_OVERFLOW, 0x04, 1),

        // POST TRAPS
        new (Trap.ILLEGAL_OPCODE, 0x08, 2),
        new (Trap.BPT, 0x0C, 3),
        new (Trap.IOT, 0x10, 3),
        new (Trap.EMT, 0x18, 3),
        new (Trap.TRAP, 0x1C, 3),
        new (Trap.MMU_TRAP, 0xA8, 4),
        new (Trap.FP_ERROR, 0xA4, 4),
        new (Trap.TRACE, 0x0C, 5),

        // INT TRAPS
        new (Trap.POWER_FAIL, 0x14, 6),
        new (Trap.PIRQ, 0xA0, 6),
        new (Trap.INTERRUPT, 0x00, 6),
    ];
}

public enum Trap : byte
{
    NONE, 
    MMU_ABORT, PARITY_ABORT, 
    BUS_ABORT, STACK_OVERFLOW,
    ILLEGAL_OPCODE, 
    BPT, IOT, EMT, TRAP, 
    MMU_TRAP, FP_ERROR, TRACE,
    POWER_FAIL, PIRQ,
    INTERRUPT,
}

public struct TrapInfo(Trap trap, ushort vector, byte priority)
{
    public readonly Trap Trap = trap;
    public ushort Vector = vector;
    public readonly byte Priority = priority;
    public bool Abort;
}
