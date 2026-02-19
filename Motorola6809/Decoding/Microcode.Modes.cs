namespace Motorola6809.Decoding;
using Computing;
using Kernel;

public static partial class Microcode
{
    private static Signal[] IMPLIED => [];
    
    private static Signal[] IMMEDIATE =>
    [
        ..READ_IMM,
        REG_COMMIT(Pointer.MDR, Pointer.W),
    ];
    
    private static Signal[] DIRECT_PAGE(Width width) =>
    [
        ..READ_IMM,
        REG_COMMIT(Pointer.MDR, Pointer.W),
        REG_COMMIT(Pointer.DP, Pointer.Z),
        ..DEREFERENCE(width),
    ];
    
    private static Signal[] EXTENDED(Width width) =>
    [
        ..READ_IMM,
        REG_COMMIT(Pointer.MDR, Pointer.W),
        ..READ_IMM,
        REG_COMMIT(Pointer.MDR, Pointer.Z),
        ..DEREFERENCE(width),
    ];

    // ------------------------- MACROS ------------------------- //
    
    private static Signal[] READ_IMM =>
    [
        MEM_READ(PC),
        PAIR_INC(PC),
    ];
    
    private static Signal[] DEREFERENCE(Width width) => width switch
    {
        Width.ADDR => NONE,
        Width.BYTE => DEREF_BYTE,
        Width.WORD => DEREF_WORD,
    };
    
    private static Signal[] DEREF_BYTE =>
    [
        MEM_READ(WZ),
        REG_COMMIT(Pointer.MDR, Pointer.W),
    ];
    
    private static Signal[] DEREF_WORD =>
    [
        MEM_READ(WZ),
        REG_COMMIT(Pointer.MDR, Pointer.TMP),
        PAIR_INC(WZ),
        MEM_READ(WZ),
        REG_COMMIT(Pointer.MDR, Pointer.W),
        REG_COMMIT(Pointer.TMP, Pointer.Z),
    ];
}

public enum Width
{
    ADDR, BYTE, WORD,
}


// imp ----> IMPLIED
// #?? ----> 8 BIT IMMEDIATE
// #???? ----> 16 BIT IMMEDIATE
// <?? ----> DIRECT PAGE
// ???? ----> ABSOLUTE
// ?? ----> 8 BIT RELATIVE
// ???? ----> 16 BIT RELATIVE