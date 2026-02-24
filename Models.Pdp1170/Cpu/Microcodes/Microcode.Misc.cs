namespace Models.Pdp1170.Cpu.Microcodes;

public partial class Microcode
{
    private static Signal[] STATE_CHANGE(State state, string nam) =>
    [
        ..SET_NAME(nam),
        STATE_COMMIT(state)
    ];

    private static Signal[] SET_PRIORITY(byte bit2, byte bit1, byte bit0) =>
    [
        ..SET_NAME($"SPL {bit2 << 2 | bit1 << 1 | bit0}"),
        ALU_COMPUTE(Operation.ZRO, Flag.PR0 | Flag.PR1 | Flag.PR2),
        ALU_COMPUTE(Operation.SET,
            (bit0 != 0 ? Flag.PR0 : Flag.NONE) | (bit1 != 0 ? Flag.PR1 : Flag.NONE) |
            (bit2 != 0 ? Flag.PR2 : Flag.NONE))
    ];
    
    private static Signal[] PSW_WRITE(Flag flag, bool set, string nam) =>
    [
        ..SET_NAME(nam),
        ALU_COMPUTE(set ? Operation.SET : Operation.ZRO, flag)
    ];
}