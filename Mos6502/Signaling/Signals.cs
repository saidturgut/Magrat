namespace Mos6502.Signaling;
using Executing.Computing;

public struct Signal()
{
    public string Name = "";
    public Cycle Cycle = Cycle.IDLE;
    public Pointer First = Pointer.NIL;
    public Pointer Second = Pointer.NIL;
    public Operation Operation = Operation.NONE;
    public Condition Condition = Condition.NONE;
    public Flag Mask = Flag.NONE;
}

public enum Cycle
{
    IDLE, DECODE, HALT, 
    REG_COMMIT, MEM_READ, MEM_WRITE,
    ALU_COMPUTE, PAIR_INC, PAIR_DEC,
}

public enum Pointer
{
    NIL, // ZERO LATCH
    PCL, PCH, // PROGRAM COUNTER
    SPL, SPH,  // STACK POINTER, HIGH FIXED TO 0x01
    A, IX, IY, // 8 BIT DATA REGISTERS
    W, Z, TMP, MDR, // TEMPORARY LATCHES
    IR, F, // OPCODE AND STATUS REGISTERS
} 
