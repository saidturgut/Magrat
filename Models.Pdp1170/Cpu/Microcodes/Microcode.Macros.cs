namespace Models.Pdp1170.Cpu.Microcodes;

public partial class Microcode
{
    // ------------------------------------ MACROS ------------------------------------ //

    private static Width StepSize(Pointer encoded, Width width)
        => width is Width.BYTE && encoded is not (Pointer.PC or Pointer.SP)
            ? Width.BYTE : Width.WORD;

    private static Signal[] READ_IMM =>
    [
        MEM_READ(Pointer.PC),
        ..INCREMENT(Pointer.PC, Width.WORD),
    ];

    private static Signal[] INCREMENT(Pointer register, Width width) =>
    [
        ALU_COMPUTE(Operation.ICC, register, Pointer.NIL, Flag.NONE, width),
        REG_MOVE(Pointer.TMP, register),
    ];
    private static Signal[] DECREMENT(Pointer register, Width width) =>
    [
        ALU_COMPUTE(Operation.DCC, register, Pointer.NIL, Flag.NONE, width),
        REG_MOVE(Pointer.TMP, register),
    ];
    
    private static Signal[] DEREFERENCE_EA(Pointer destination, Width width) =>
    [
        MEM_READ(Pointer.EA, width),
        REG_MOVE(Pointer.MDR, destination),
    ];
    
    private static Signal[] PUSH(Pointer source) =>
    [
        ..DECREMENT(Pointer.SP, Width.WORD),
        REG_MOVE(source, Pointer.MDR),
        MEM_WRITE(Pointer.SP),
    ];
    
    private static Signal[] POP(Pointer destination) =>
    [
        MEM_READ(Pointer.SP),
        REG_MOVE(Pointer.MDR, destination),
        ..INCREMENT(Pointer.SP, Width.WORD),
    ];
}