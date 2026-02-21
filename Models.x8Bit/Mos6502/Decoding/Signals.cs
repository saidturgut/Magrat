namespace Models.x8Bit.Mos6502.Decoding;

public enum Pointer
{
    NIL, FR, IR, // CORE REGISTERS
    MDR, TMP, W, Z, // TEMPORARY LATCHES
    PCL, PCH, // PROGRAM COUNTER
    SPL, SPH, // STACK POINTER, HIGH FIXED TO 0x01
    A, IX, IY, // 8 BIT DATA REGISTERS
}

public enum State
{
    FETCH, DECODE, HALT, 
}

public enum Condition
{
    NONE = -1, NE, EQ, CC, CS, VC, VS, PL, MI,
}