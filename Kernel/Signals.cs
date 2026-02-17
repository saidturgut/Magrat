namespace Kernel;

public struct Signal()
{
    public string Name = "";
    public Cycle Cycle = Cycle.IDLE;
    public byte State = 0;
    public byte First = 0;
    public byte Second = 0;
    public byte Operation = 0;
    public byte Condition = 0;
    public byte Mask = 0;
}

public enum Cycle
{
    IDLE, STATE_COMMIT,
    REG_COMMIT, MEM_READ, MEM_WRITE,
    ALU_COMPUTE, COND_COMPUTE,
    PAIR_INC, PAIR_DEC,
}

public enum PointerK
{
    NIL, FR, IR, // CORE REGISTERS
    MDR, TMP, W, Z, // TEMPORARY LATCHES
}
