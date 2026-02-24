namespace Models.Pdp1170.Cpu.Microcodes;

public partial class Microcode
{
    private static Signal[] NOP()
    {
        name = "NOP";
        return [];
    }
    private static Signal[] STATE_CHANGE(State state, string nam)
    {
        name = nam;
        return [STATE_COMMIT(state)];
    }
}