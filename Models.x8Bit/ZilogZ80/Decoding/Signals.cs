namespace Models.x8Bit.ZilogZ80.Decoding;

public enum Pointer
{
    NIL, FR, IR, // CORE REGISTERS
    MDR, TMP, W, Z, // TEMPORARY LATCHES
    PCL, PCH, // PROGRAM COUNTER
    SPL, SPH,  // STACK POINTER
    A, C, B, E, D, L, H, // REGISTER PAIRS
    IXL, IXH, IYL, IYH, // INDEX REGISTERS
    AA, CC, BB, EE, DD, LL, HH, FF, // SHADOW PAIRS
    I, R, // CONTROL REGISTERS
}

public enum State
{
    FETCH, DECODE, HALT, 
    DEC_ED, DEC_CB, DEC_DD, DEC_FD, DEC_DDCB, DEC_FDCB,
    INT_E, INT_D, INT_0, INT_1, INT_2, INT_R,
}

public enum Condition
{
    NONE = -1, NZ, Z, NC, C, PO, PE, P, M, ED,
}