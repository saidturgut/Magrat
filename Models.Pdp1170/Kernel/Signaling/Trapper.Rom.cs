namespace Models.Pdp1170.Kernel.Signaling;

public partial class Trapper
{
    private static readonly Trap[] TrapsTable =
    [
        new (Vector.NONE, 0x00, 6),

        // ABORT TRAPS
        new (Vector.MMU_ABORT, 0xA8, 0),
        new (Vector.PARITY_ABORT, 0x4C, 0),
        new (Vector.BUS_ABORT, 0x04, 0),
        new (Vector.ODD_ADDRESS, 0x04, 0),
        new (Vector.STACK_OVERFLOW, 0x04, 0),

        // POST TRAPS
        new (Vector.ILLEGAL, 0x08, 1),
        new (Vector.BPT, 0x0C, 2),
        new (Vector.IOT, 0x10, 2),
        new (Vector.EMT, 0x18, 2),
        new (Vector.TRAP, 0x1C, 2),
        new (Vector.PDR_ERROR, 0xA8, 3),
        new (Vector.FP_ERROR, 0xA4, 3),
        new (Vector.TRACE, 0x0C, 4),

        // INT TRAPS
        new (Vector.POWER_FAIL, 0x14, 5),
        new (Vector.PIRQ, 0xA0, 5),
        new (Vector.INTERRUPT, 0x00, 5),
    ];
}

public struct Trap(Vector vector, ushort address, byte priority)
{
    public readonly Vector Vector = vector;
    public ushort Address = address;
    public readonly byte Priority = priority;
    public bool Abort;
}
