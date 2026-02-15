namespace ZilogZ80.Signaling.Microcodes;

public partial class Microcode
{
    private static Signal[] SWAP_ALL =>
    [
        ..PAIR_SWAP(BC, [Pointer.CC, Pointer.BB]),
        ..PAIR_SWAP(DE, [Pointer.EE, Pointer.DD]),
        ..PAIR_SWAP(HL, [Pointer.LL, Pointer.HH]),
    ];

    private static Signal[] SWAP_AF =>
    [
        ..PAIR_SWAP(AF, [Pointer.AA, Pointer.FF]),
    ];
    
    private static Signal[] SWAP_XCHG =>
    [
        ..PAIR_SWAP(DE, HL),
    ];
    
    private static Signal[] SWAP_XTHL =>
    [
        MEM_READ(SP),
        REG_COMMIT(Pointer.MDR, Pointer.TMP),
        REG_COMMIT(PointL, Pointer.MDR),
        MEM_WRITE(SP),
        REG_COMMIT(Pointer.TMP, PointL),
        PAIR_INC(SP),
        
        MEM_READ(SP),
        REG_COMMIT(Pointer.MDR, Pointer.TMP),
        REG_COMMIT(PointH, Pointer.MDR),
        MEM_WRITE(SP),
        REG_COMMIT(Pointer.TMP, PointH),
        PAIR_DEC(SP),
    ];
}