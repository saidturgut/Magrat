namespace Models.x8Bit.Motorola6809.Decoding;
using Computing;
using Models.x8Bit.Engine;

public static partial class Microcode
{
    private static Signal[] IMPLIED => [];

    private static Signal[] IMMEDIATE(Width width) =>
    [
        ..width is Width.BYTE ? READ_IMM : LOAD_WZ_IMM,
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
        ..LOAD_WZ_IMM,
        ..DEREFERENCE(width),
    ];

    // ------------------------- MACROS ------------------------- //
    
    private static Signal[] READ_IMM =>
    [
        MEM_READ(PC),
        PAIR_INC(PC),
    ];

    private static Signal[] LOAD_WZ_IMM =>
    [
        ..READ_IMM,
        REG_COMMIT(Pointer.MDR, Pointer.Z),
        ..READ_IMM,
        REG_COMMIT(Pointer.MDR, Pointer.W),
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
// ???? ----> EXTENDED
// ?? ----> 8 BIT RELATIVE
// ???? ----> 16 BIT RELATIVE