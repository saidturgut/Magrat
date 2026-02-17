namespace Mos6502.Decoding;
using Signaling;
using Computing;
using Kernel;

public static partial class Microcode
{
    // ------------------------- PUSH & PULL ------------------------- //

    private static Signal[] PUSH(Pointer source) =>
    [
        source is Pointer.FR ? ALU_COMPUTE(Operation.PSR, Pointer.FR, Pointer.NIL, Flag.NONE) : REG_COMMIT(source, Pointer.MDR),
        ..source is Pointer.FR ? [REG_COMMIT(Pointer.TMP, Pointer.MDR)] : NONE,
        MEM_WRITE(SP),
        PAIR_DEC(SP),
    ];
    
    private static Signal[] PULL(Pointer destination) =>
    [
        PAIR_INC(SP),
        MEM_READ(SP),
        destination is Pointer.FR 
            ? ALU_COMPUTE(Operation.SRP, Pointer.MDR, Pointer.NIL, Flag.NONE) 
            : ALU_COMPUTE(Operation.NONE, Pointer.MDR, Pointer.NIL, FlagMasks[FlagMask.ZN]),
        REG_COMMIT(Pointer.TMP, destination),
    ];
    
    private static Signal[] POP(Pointer destination) =>
    [
        PAIR_INC(SP),
        MEM_READ(SP),
        REG_COMMIT(Pointer.MDR, destination),
    ];
    
    // ------------------------- CONTROL FLOW ------------------------- //

    private static Signal[] JUMP =>
    [
        REG_COMMIT(Pointer.W, Pointer.PCL),
        REG_COMMIT(Pointer.Z, Pointer.PCH),
    ];
    
    private static Signal[] CALL =>
    [
        PAIR_DEC(PC),
        ..PUSH(Pointer.PCH),
        ..PUSH(Pointer.PCL),
        ..JUMP,
    ];

    private static Signal[] RETURN(bool rti) =>
    [
        ..rti ? POP(Pointer.FR) : NONE,
        ..POP(Pointer.W),
        ..POP(Pointer.Z),
        ..rti ? NONE : [PAIR_INC(WZ)],
        ..JUMP,
    ];

    private static Signal[] BREAK =>
    [
        PAIR_INC(PC),
        ..PUSH(Pointer.PCH),
        ..PUSH(Pointer.PCL),
        ..PUSH(Pointer.FR),

        ALU_COMPUTE(Operation.SET, Pointer.NIL, Pointer.NIL, Flag.INTERRUPT),
        REG_COMMIT(Pointer.TMP, Pointer.W), // LOAD 0XFF ON WZ
        REG_COMMIT(Pointer.TMP, Pointer.Z),
        
        MEM_READ(WZ),
        REG_COMMIT(Pointer.MDR, Pointer.PCH),
        PAIR_DEC(WZ),
        MEM_READ(WZ),
        REG_COMMIT(Pointer.MDR, Pointer.PCL),
    ];
}