namespace Models.x8Bit.Mos6502.Decoding;
using Signaling;
using Computing;
using x8Bit.Engine;

public static partial class Microcode
{
    // ------------------------- FAST MODES ------------------------- //
    
    private static Signal[] IMPLIED => // IMP
    [
    ];

    private static Signal[] IMMEDIATE => // IMM
    [
        REG_COMMIT(Pointer.PCL, Pointer.W),
        REG_COMMIT(Pointer.PCH, Pointer.Z),
        PAIR_INC(PC)
    ];
        
    private static Signal[] ZERO_PAGE => // ZP, ZPX, ZPY
    [
        ..READ_IMM,
        REG_COMMIT(Pointer.MDR, Pointer.W),
    ];

    private static Signal[] ZERO_PAGE_IDX(Pointer index) =>
    [
        ..READ_IMM,
        ..COMMIT_INDEX(Operation.IDX, Pointer.MDR, Pointer.W, index),
    ];
    
    // ------------------------- SLOW MODES ------------------------- //
    
    private static Signal[] ABSOLUTE => // ABS
    [
        ..READ_IMM,
        REG_COMMIT(Pointer.MDR, Pointer.W),
        ..READ_IMM,
        REG_COMMIT(Pointer.MDR, Pointer.Z),
    ];

    private static Signal[] ABSOLUTE_IDX(Pointer index) => // ABS ZP
    [
        ..READ_IMM,
        ..COMMIT_INDEX(Operation.IDX, Pointer.MDR, Pointer.W, index),
        ..READ_IMM,
        ..COMMIT_INDEX(Operation.CRY, Pointer.MDR, Pointer.Z, Pointer.NIL),
    ];
    
    private static Signal[] INDIRECT => // IND
    [
        ..ABSOLUTE,
        ..INDIRECT_ADDRESS(),
    ];
    
    private static Signal[] INDIRECT_X => // INDX
    [
        ..INDIRECT_POINTER,
        ..COMMIT_INDEX(Operation.IDX, Pointer.W, Pointer.W, Pointer.IX),
        ..INDIRECT_ADDRESS(),
    ];

    private static Signal[] INDIRECT_Y => // INDY
    [
        ..INDIRECT_POINTER,
        ..INDIRECT_ADDRESS(),
        ..COMMIT_INDEX(Operation.IDX, Pointer.W, Pointer.W, Pointer.IY), 
        ..COMMIT_INDEX(Operation.CRY, Pointer.Z, Pointer.Z, Pointer.NIL)
    ];
    
    // ------------------------- MACROS ------------------------- //

    private static Signal[] COMMIT_INDEX(Operation operation, Pointer source, Pointer destination, Pointer index) =>
    [
        ALU_COMPUTE(operation, source, index, Flag.NONE),
        REG_COMMIT(Pointer.TMP, destination),
    ];
    
    private static Signal[] READ_IMM => // IMM
    [
        MEM_READ(PC),
        PAIR_INC(PC),
    ];
    
    private static Signal[] INDIRECT_POINTER =>
    [
        ..READ_IMM,
        REG_COMMIT(Pointer.MDR, Pointer.W),
    ];
    
    private static Signal[] INDIRECT_ADDRESS() =>
    [
        MEM_READ(WZ),
        REG_COMMIT(Pointer.MDR, Pointer.TMP),
        PAIR_INC(WZ),
        MEM_READ(WZ),
        REG_COMMIT(Pointer.MDR, Pointer.Z),
        REG_COMMIT(Pointer.TMP, Pointer.W),
    ];
}