namespace Motorola6809.Decoding;

public enum Pointer
{
    NIL, FR, IR, // CORE REGISTERS
    MDR, TMP, W, Z, // TEMPORARY LATCHES
    PCL, PCH, // PROGRAM COUNTER
    SPL, SPH, UPL, UPH, DP, // PAGE REGISTERS
    IXL, IXH, IYL, IYH, // INDEX REGISTERS
    A, B //ACCUMULATORS
}

public enum State
{
    FETCH, DECODE, HALT,
    DEC_10, DEC_11, DEC_POST,
}
