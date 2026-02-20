using Motorola6809.Computing;

namespace Motorola6809.Decoding;
using Kernel;

public static partial class Microcode
{
    private static Signal[] IDLE =>
        [new ()];

    public static Signal[] FETCH =>
    [
        MEM_READ(PC),
        PAIR_INC(PC),
        REG_COMMIT(Pointer.MDR, Pointer.IR),
        STATE_COMMIT(State.DECODE),
    ];
    
    private static Signal[] LOAD_BYTE(Pointer destination) =>
    [
        REG_COMMIT(Pointer.MDR, destination),
        ALU_COMPUTE(Operation.OR, destination, Pointer.NIL, FlagMasks[FlagMask.VZN]),
    ];
    private static Signal[] LOAD_WORD(Pointer[] destinations) =>
    [
        REG_COMMIT(Pointer.W, destinations[0]),
        REG_COMMIT(Pointer.Z, destinations[1]),
        ALU_COMPUTE(Operation.OR, destinations[0], destinations[1], FlagMasks[FlagMask.VZN]),
    ];
    
    private static Signal[] STORE_BYTE(Pointer source) =>
    [
        REG_COMMIT(source, Pointer.MDR),
        MEM_WRITE(WZ),
        ALU_COMPUTE(Operation.OR, source, Pointer.NIL, FlagMasks[FlagMask.VZN]),
    ];
    private static Signal[] STORE_WORD(Pointer[] sources) =>
    [
        REG_COMMIT(sources[1], Pointer.MDR),
        MEM_WRITE(WZ),
        PAIR_INC(WZ),
        REG_COMMIT(sources[0], Pointer.MDR),
        MEM_WRITE(WZ),
        ALU_COMPUTE(Operation.OR, sources[0], sources[1], FlagMasks[FlagMask.VZN]),
    ];
    
    private static Signal[] ALU_BYTE(Pointer source, Operation operation, FlagMask mask) =>
    [
        ..FRU_BYTE(source, operation, mask),
        REG_COMMIT(Pointer.TMP, source),
    ];
    private static Signal[] ALU_WORD(Pointer[] sources, Operation[] operations, FlagMask mask) =>
    [
        ALU_COMPUTE(operations[0], sources[0], Pointer.W, Flag.CARRY),
        REG_COMMIT(Pointer.TMP, sources[0]),
        ALU_COMPUTE(operations[1], sources[1], Pointer.Z, FlagMasks[mask]),
        REG_COMMIT(Pointer.TMP, sources[1]),
    ];
    private static Signal[] ALU_MEM(Operation operation, FlagMask mask)=>
    [
        ..FRU_MEM(operation, mask),
        REG_COMMIT(Pointer.TMP, Pointer.MDR),
        MEM_WRITE(WZ),
    ];
    
    private static Signal[] FRU_BYTE(Pointer source, Operation operation, FlagMask mask) =>
    [
        ALU_COMPUTE(operation, source, Pointer.MDR, FlagMasks[mask]),
    ];
    private static Signal[] FRU_WORD(Pointer[] sources, Operation[] operations, FlagMask mask) =>
    [
        ALU_COMPUTE(operations[0], sources[0], Pointer.W, Flag.CARRY),
        ALU_COMPUTE(operations[1], sources[1], Pointer.Z, FlagMasks[mask]),
    ];
    private static Signal[] FRU_MEM(Operation operation, FlagMask mask)=>
    [
        MEM_READ(WZ),
        ALU_COMPUTE(operation, Pointer.MDR, Pointer.NIL, FlagMasks[mask]),
    ];
}