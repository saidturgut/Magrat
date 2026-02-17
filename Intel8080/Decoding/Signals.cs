namespace Intel8080.Decoding;

public enum Pointer
{
    NIL, FR, IR, // CORE REGISTERS
    MDR, TMP, W, Z, // TEMPORARY LATCHES
    PCL, PCH, // PROGRAM COUNTER
    SPL, SPH,  // STACK POINTER
    A, C, B, E, D, L, H, // REGISTER PAIRS
}

public enum State
{
    FETCH, DECODE, HALT, 
    INT_E, INT_D,
}

public enum Condition
{
    NONE, NZ, Z, NC, C, PO, PE, P, M,
}