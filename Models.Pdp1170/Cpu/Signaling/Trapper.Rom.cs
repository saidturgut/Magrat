namespace Models.Pdp1170.Cpu.Signaling;

public partial class Trapper
{
    private static readonly TrapInfo[] TrapsTable =
    [
        new (Trap.NONE, 0x00, 6),

        // ABORT TRAPS
        new (Trap.MMU_ABORT, 0xA8, 0),
        new (Trap.PARITY_ABORT, 0x4C, 0),
        new (Trap.BUS_ABORT, 0x04, 0),
        new (Trap.STACK_OVERFLOW, 0x04, 0),

        // POST TRAPS
        new (Trap.ILLEGAL, 0x08, 1),
        new (Trap.BPT, 0x0C, 2),
        new (Trap.IOT, 0x10, 2),
        new (Trap.EMT, 0x18, 2),
        new (Trap.TRAP, 0x1C, 2),
        new (Trap.PDR_ERROR, 0xA8, 3),
        new (Trap.FP_ERROR, 0xA4, 3),
        new (Trap.TRACE, 0x0C, 4),

        // INT TRAPS
        new (Trap.POWER_FAIL, 0x14, 5),
        new (Trap.PIRQ, 0xA0, 5),
        new (Trap.INTERRUPT, 0x00, 5),
    ];
}

public struct TrapInfo(Trap trap, ushort vector, byte priority)
{
    public readonly Trap Trap = trap;
    public ushort Vector = vector;
    public readonly byte Priority = priority;
    public bool Abort;
}
