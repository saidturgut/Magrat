namespace Models.x8Bit.ZilogZ80.Decoding;
using Signaling;
using Computing;
using Models.x8Bit.Engine;

public partial class Microcode
{
    // ------------------------- INTERRUPTS ------------------------- //

    private static Signal[] INT_AA(Pointer source) =>
    [
        ALU_COMPUTE(Operation.IOP, source, Pointer.NIL, FlagMasks[FlagMask.N3H5ZS]),
        REG_COMMIT(source, Pointer.A),
    ];
    
    private static Signal[] INT_RI(Pointer source) =>
    [
        REG_COMMIT(Pointer.A, source),
    ];
    
    // ------------------------- 8 BIT ARITHMETIC ------------------------- //

    private static Signal[] NEG =>
    [
        ALU_COMPUTE(Operation.SUB, Pointer.NIL, Pointer.A, FlagMasks[FlagMask.CNV3H5ZS]),
        REG_COMMIT(Pointer.TMP, Pointer.A),
    ];

    private static Signal[] DECIMAL_ROT(Operation low, Operation high) =>
    [
        MEM_READ(HL),
        REG_COMMIT(Pointer.A, Pointer.W),
        ALU_COMPUTE(low, Pointer.A, Pointer.MDR, FlagMasks[FlagMask.NV3H5ZS]),
        REG_COMMIT(Pointer.TMP, Pointer.A),
        ALU_COMPUTE(high, Pointer.W, Pointer.MDR, Flag.NONE),
        REG_COMMIT(Pointer.TMP, Pointer.MDR),
        MEM_WRITE(HL),
    ];
    
    // ------------------------- INPUT & OUTPUT ------------------------- //
    
    private static Signal[] INPUT_OUTPUT_BC(bool input) =>
    [
        ..input
            ? (Signal[])
            [
                MEM_READ(BC),
                ALU_COMPUTE(Operation.IOP, Pointer.MDR, Pointer.NIL, FlagMasks[FlagMask.NVHZS]),
                REG_COMMIT(Pointer.MDR, EncodedRegisters[aa_XXX_aaa]),
            ]
            : aa_XXX_aaa != 6 
                ? new[] { REG_COMMIT(EncodedRegisters[aa_XXX_aaa], Pointer.MDR), MEM_WRITE(BC), } 
                : new[] { REG_COMMIT(Pointer.NIL, Pointer.MDR), MEM_WRITE(BC), }
    ];

    private static Signal[] INPUT_OUTPUT_BLK(bool input, bool inc, bool loop) =>
    [
        ..input
            ? (Signal[])[MEM_READ(BC), MEM_WRITE(HL)] 
            : (Signal[])[MEM_READ(HL), MEM_WRITE(BC)],
        
        inc ? PAIR_INC(HL) : PAIR_DEC(HL),
        ALU_COMPUTE(Operation.DEC, Pointer.B, Pointer.NIL, FlagMasks[FlagMask.NZ]),
        REG_COMMIT(Pointer.TMP, Pointer.B),
        REG_COMMIT(Pointer.FR, Pointer.TMP),
        ..loop ? [COND_COMPUTE(State.DECODE, Condition.Z)] : NONE,
    ];
    
    // ------------------------- TRANSFER & COMPARE ------------------------- //

    private static Signal[] TRANSFER(bool inc, bool loop) =>
    [
        MEM_READ(HL),
        MEM_WRITE(DE),
        inc ? PAIR_INC(HL) : PAIR_DEC(HL),
        inc ? PAIR_INC(DE) : PAIR_DEC(DE),
        PAIR_DEC(BC),
        ALU_COMPUTE(Operation.BLK, Pointer.B, Pointer.C, FlagMasks[FlagMask.NV3H5]),
        REG_COMMIT(Pointer.FR, Pointer.TMP),
        ..loop ? [COND_COMPUTE(State.DECODE, Condition.PE)] : NONE,
    ];
    
    private static Signal[] COMPARE(bool inc, bool loop) =>
    [
        MEM_READ(HL),
        ALU_COMPUTE(Operation.SUB, Pointer.A, Pointer.MDR, FlagMasks[FlagMask.N3H5ZS]),
        inc ? PAIR_INC(HL) : PAIR_DEC(HL),
        PAIR_DEC(BC),
        ALU_COMPUTE(Operation.BLK, Pointer.B, Pointer.C, Flag.OVERFLOW),
        REG_COMMIT(Pointer.FR, Pointer.TMP),
        ..loop ? [COND_COMPUTE(State.DECODE, Condition.ED)] : NONE,
    ];
}