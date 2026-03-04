namespace Models.Pdp1170.Kernel;

public struct Signal()
{
    public Cycle Cycle = Cycle.IDLE;
    public State State = State.FETCH;
    public Pointer First = Pointer.NIL;
    public Pointer Second = Pointer.NIL;
    public Pointer Third = Pointer.NIL;
    public Operation Operation = Operation.ZRO;
    public Flag Mask = Flag.NONE;
    public Width Width = Width.WORD;
    public Condition Condition = Condition.R;
    public Vector Vector = Vector.NONE;
}

public enum Cycle : byte
{
    IDLE, REG_MOVE, 
    MEM_FETCH, MEM_READ, MEM_WRITE,
    ALU_COMPUTE, COND_COMPUTE
}

public enum State : byte
{
    FETCH, DECODE, RESET, WAIT, HALT,
    TRAP, REQUEST, SUPPRESS
}

public enum Pointer : byte
{
    NIL, IR, PSW, PC, // CORE REGISTERS
    MDR, TMP, EA, VEC, SRC, DST, // TEMPORARY LATCHES
    R0, R1, R2, R3, R4, R5, SP, // GENERAL REGISTERS
}

public enum Operation : byte
{
    ZRO, SET, ICC, DCC, IDX, // CORE OPS
    PASS, ADD, SUB, BIT, BIC, BIS, // TWO OPR
    CLR, COM, NEG, TST, // ONE OPR 1
    INC, DEC, ADC, SBC, // ONE OPR 2
    ROR, ROL, ASR, ASL, SXT, SWAB, // ONE OPR 3
    BRC, MARK, SOB, // MISC
    MUL_L, MUL_H, DIV_L, DIV_H, ASH_L, ASH_H, // HALF LONG OPR
    XOR, ASH, // HALF WORD OPR
}

public enum Condition : byte
{
    R, NE, EQ, GE, LT, GT, LE, EMT,
    PL, MI, HI, LOS, VC, VS, CC, CS
}

[Flags]
public enum Flag : ushort
{
    NONE = 0,
    CARRY = 1 << 0,
    OVERFLOW = 1 << 1,
    ZERO = 1 << 2,
    NEGATIVE = 1 << 3,
    TRACE = 1 << 4,
    IP0 = 1 << 5, IP1 = 1 << 6, IP2 = 1 << 7,
    RES8 = 1 << 8, RES9 = 1 << 9, RES10 = 1 << 10,
    RS = 1 << 11,
    PM0 = 1 << 12, PM1 = 1 << 13,
    CM0 = 1 << 14, CM1 = 1 << 15,
}

public enum Width : byte
{
    BYTE, WORD,
}

public enum Vector : byte
{
    NONE, 
    MMU_ABORT, PARITY_ABORT, 
    BUS_ABORT, ODD_ADDRESS, STACK_OVERFLOW,
    ILLEGAL, BPT, IOT, EMT, TRAP, 
    PDR_ERROR, FP_ERROR, TRACE,
    POWER_FAIL, PIRQ,
    INTERRUPT,
}

public struct ControlSignal(ushort opcode, bool stall, bool skip)
{
    public readonly ushort Opcode = opcode;
    public readonly bool Stall = stall;
    public readonly bool Skip = skip;
}

public enum Mode : byte
{
    KERNEL, SUPERVISOR, RESERVED, USER,
}

public enum Space : byte
{
    Instruction, Data
}


