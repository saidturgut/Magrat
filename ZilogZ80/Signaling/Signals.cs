namespace ZilogZ80.Signaling;
using Executing.Computing;

public struct Signal()
{
    public string Name = "";
    public State State = State.FETCH;
    public Cycle Cycle = Cycle.IDLE;
    public Pointer First = Pointer.NIL;
    public Pointer Second = Pointer.NIL;
    public Operation Operation = Operation.NONE;
    public Condition Condition = Condition.NONE;
    public Flag Mask = Flag.NONE;
}

public enum Cycle
{
    IDLE, STATE_COMMIT,
    REG_COMMIT, MEM_READ, MEM_WRITE,
    ALU_COMPUTE, COND_COMPUTE,
    PAIR_INC, PAIR_DEC,
}

public enum Pointer
{
    NIL, // ZERO LATCH
    PCL, PCH, // PROGRAM COUNTER
    SPL, SPH,  // STACK POINTER
    IXL, IXH, IYL, IYH, // INDEX REGISTERS
    F, A, C, B, E, D, L, H, // REGISTER PAIRS
    FF, AA, CC, BB, EE, DD, LL, HH, // SHADOW PAIRS
    W, Z, TMP, MDR, // TEMPORARY LATCHES
    IR, I, R, V, // CONTROL REGISTERS
}
