namespace Models.Pdp1170.Cpu;

public struct Signal()
{
    public string Name = "";
    public Cycle Cycle = Cycle.IDLE;
    public State State = State.FETCH;
    public Pointer First = Pointer.NIL;
    public Pointer Second = Pointer.NIL;
    public Operation Operation = Operation.PASS;
    public Flag Mask = Flag.NONE;
    public Width Width = Width.WORD;
    public Condition Condition = Condition.R;
}

public enum Cycle
{
    IDLE, STATE_COMMIT,
    REG_MOVE, MEM_READ, MEM_WRITE,
    ALU_COMPUTE, COND_COMPUTE
}

public enum State
{
    FETCH, DECODE, HALT,
}

public enum Pointer
{
    NIL, IR, PSW, PC, // CORE REGISTERS
    MDR, TMP, EA, SRC, DST, // TEMPORARY LATCHES
    R0, R1, R2, R3, R4, R5, SP, // GENERAL REGISTERS
}

public enum Operation
{
    NONE,
    PASS, ICC, DCC, IDX, // CORE
    ADD, SUB, BIT, BIC, BIS, // TWO OPERAND
    CLR, COM, INC, DEC, 
    NEG, ADC, SBC, TST,
    ROR, ROL, ASR, ASL, SXT, SWAB,
}

public enum Condition
{
    R, NE, EQ, GE, LT, GT, LE, PL, MI, HI, LOS, VC, VS, CC, CS 
}

[Flags]
public enum Flag
{
    NONE = 0,
    CARRY = 1 << 0,
    OVERFLOW = 1 << 1,
    ZERO = 1 << 2,
    NEGATIVE = 1 << 3,
    TRACE = 1 << 4,
    RES5 = 1 << 5, RES6 = 1 << 6,
    PR0 = 1 << 7, PR1 = 1 << 8, PR2 = 1 << 9,
    RES10 = 1 << 10,
    RS = 1 << 11,
    PM0 = 1 << 12, PM1 = 1 << 13,
    CM0 = 1 << 14, CM1 = 1 << 15,
}

public enum Width
{
    BYTE, WORD,
}

public struct ControlSignal(ushort opcode, bool stall)
{
    public readonly ushort Opcode = opcode;
    public readonly bool Stall = stall;
}


