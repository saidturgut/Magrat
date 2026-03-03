namespace Models.Pdp1170.Kernel.Microcodes;

public partial class Microcode
{
    private static Signal[] REGISTER(Pointer encoded, Pointer temp, Width width) => 
    [
        REG_MOVE(encoded, temp),
    ];
    private static Signal[] REGISTER_DEFERRED(Pointer encoded, Pointer temp, Width width) =>
    [
        REG_MOVE(encoded, Pointer.EA),
        ..DEREFERENCE_EA(temp, width),
    ];
    
    private static Signal[] AUTO_INC(Pointer encoded, Pointer temp, Width width) =>
    [
        REG_MOVE(encoded, Pointer.EA),
        ..DEREFERENCE_EA(temp, width),
        ..INCREMENT(encoded, StepSize(encoded, width)),
    ];
    private static Signal[] AUTO_INC_DEFERRED(Pointer encoded, Pointer temp, Width width) =>
    [
        MEM_READ(encoded),
        REG_MOVE(Pointer.MDR, Pointer.EA),
        ..DEREFERENCE_EA(temp, width),
        ..INCREMENT(encoded, Width.WORD),
    ];
    
    private static Signal[] AUTO_DEC(Pointer encoded, Pointer temp, Width width) =>
    [
        ..DECREMENT(encoded, StepSize(encoded, width)),
        REG_MOVE(encoded, Pointer.EA),
        ..DEREFERENCE_EA(temp, width),
    ];
    private static Signal[] AUTO_DEC_DEFERRED(Pointer encoded, Pointer temp, Width width) =>
    [
        ..DECREMENT(encoded, Width.WORD),
        MEM_READ(encoded),
        REG_MOVE(Pointer.MDR, Pointer.EA),
        ..DEREFERENCE_EA(temp, width),
    ];
    
    private static Signal[] INDEX(Pointer encoded, Pointer temp, Width width) =>
    [
        ..READ_IMM,
        ALU_COMPUTE(Operation.IDX, encoded, Pointer.MDR, Flag.NONE),
        REG_MOVE(Pointer.TMP, Pointer.EA),
        ..DEREFERENCE_EA(temp, width),
    ];
    private static Signal[] INDEX_DEFERRED(Pointer encoded, Pointer temp, Width width) =>
    [
        ..READ_IMM,
        ALU_COMPUTE(Operation.IDX, encoded, Pointer.MDR, Flag.NONE),
        MEM_READ(Pointer.TMP),
        REG_MOVE(Pointer.MDR, Pointer.EA),
        ..DEREFERENCE_EA(temp, width),
    ];
    
    private static readonly Func<Pointer, Pointer, Width, Signal[]>[] ADDRESS =
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
            ? input.Modes[0] != 0
                ? (Signal[])[REG_MOVE(Pointer.TMP, Pointer.MDR), MEM_WRITE(Pointer.EA, input.Width)]
                : [REG_MOVE(Pointer.TMP, input.Regs[0])]
            : NONE,
    ];
}