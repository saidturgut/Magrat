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
}

public enum Cycle
{
    IDLE, STATE_COMMIT,
    REG_WRITE, MEM_READ, MEM_WRITE,
    ALU_COMPUTE,
}

public enum State
{
    FETCH, DECODE, HALT,
}

public enum Pointer
{
    NIL, IR, PSW, PC, // CORE REGISTERS
    MDR, TMP, SRC, DST, // TEMPORARY LATCHES
    R0, R1, R2, R3, R4, R5, SP, // GENERAL REGISTERS
}
    

public enum Operation
{
    PASS, ICC, DCC, // CORE
    ADD, SUB, BIT, BIC, BIS, // TWO OPERAND
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
    public ushort Opcode = opcode;
    public bool Stall = stall;
}


