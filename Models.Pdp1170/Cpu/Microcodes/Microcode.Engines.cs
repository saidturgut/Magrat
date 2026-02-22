namespace Models.Pdp1170.Cpu.Microcodes;

public partial class Microcode
{
    private static Signal[] DEREFERENCE(Pointer source, Pointer destination) =>
    [
        MEM_READ(source),
        REG_WRITE(Pointer.MDR, destination),
    ];
    
    private static Signal[] REGISTER(Pointer encoded, Pointer temp, bool byteMode) => 
    [
        REG_WRITE(encoded, temp),
    ];
    private static Signal[] REGISTER_DEFERRED(Pointer encoded, Pointer temp, bool byteMode) =>
    [
        MEM_READ(encoded),
        REG_WRITE(Pointer.MDR, temp),
    ];
    
    private static Signal[] AUTO_INC(Pointer encoded, Pointer temp, bool byteMode) =>
    [
        ..DEREFERENCE(encoded, temp),
        ..INCREMENT(encoded, StepSize(encoded, byteMode)),
    ];
    private static Signal[] AUTO_INC_DEFERRED(Pointer encoded, Pointer temp, bool byteMode) =>
    [
        MEM_READ(encoded),
        ..DEREFERENCE(Pointer.MDR, temp),
        ..INCREMENT(encoded, Width.WORD),
    ];
    
    private static Signal[] AUTO_DEC(Pointer encoded, Pointer temp, bool byteMode) =>
    [
        ..DECREMENT(encoded, StepSize(encoded, byteMode)),
        ..DEREFERENCE(encoded, temp),
    ];
    private static Signal[] AUTO_DEC_DEFERRED(Pointer encoded, Pointer temp, bool byteMode) =>
    [
        ..DECREMENT(encoded, Width.WORD),
        MEM_READ(encoded),
        ..DEREFERENCE(Pointer.MDR, temp),
    ];
    
    private static Signal[] INDEX(Pointer encoded, Pointer temp, bool byteMode) =>
    [
        ..READ_IMM,
        ALU_COMPUTE(Operation.ADD, encoded, Pointer.MDR, Flag.NONE),
        ..DEREFERENCE(Pointer.TMP, temp),
    ];
    private static Signal[] INDEX_DEFERRED(Pointer encoded, Pointer temp, bool byteMode) =>
    [
        ..READ_IMM,
        ALU_COMPUTE(Operation.ADD, encoded, Pointer.MDR, Flag.NONE),
        MEM_READ(Pointer.TMP),
        ..DEREFERENCE(Pointer.MDR, temp),
    ];
    
    private static readonly Func<Pointer, Pointer, bool, Signal[]>[] ADDRESS =
    [
        REGISTER, REGISTER_DEFERRED, 
        AUTO_INC, AUTO_INC_DEFERRED, AUTO_DEC, AUTO_DEC_DEFERRED, 
        INDEX, INDEX_DEFERRED
    ];
    
    private static Signal[] EXECUTE(Descriptor input) =>
    [
        ALU_COMPUTE(input.Operation, Pointer.DST, Pointer.SRC, input.Mask, input.Width),
    ];

    private static Signal[] WRITEBACK(Descriptor input) =>
    [
        ..input.Writeback
            ? input.Modes[1] != 0
                ? (Signal[])[REG_WRITE(Pointer.TMP, Pointer.MDR), MEM_WRITE(Pointer.DST, input.Width)]
                : [REG_WRITE(Pointer.TMP, input.Regs[1], input.Width)]
            : NONE,
    ];
}