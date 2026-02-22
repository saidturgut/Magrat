namespace Models.Pdp1170.Cpu.Microcodes;

public partial class Microcode
{
    // ------------------------------------ MACROS ------------------------------------ //

    private static Width StepSize(Pointer encoded, bool byteMode)
        => byteMode && encoded is not (Pointer.PC or Pointer.SP)
            ? Width.BYTE : Width.WORD;

    private static Signal[] READ_IMM =>
    [
        MEM_READ(Pointer.PC),
        ..INCREMENT(Pointer.PC, Width.WORD),
    ];

    private static Signal[] INCREMENT(Pointer register, Width width) =>
    [
        ALU_COMPUTE(Operation.ICC, register, Pointer.NIL, Flag.NONE, width),
        REG_WRITE(Pointer.TMP, register),
    ];
    private static Signal[] DECREMENT(Pointer register, Width width) =>
    [
        ALU_COMPUTE(Operation.DCC, register, Pointer.NIL, Flag.NONE, width),
        REG_WRITE(Pointer.TMP, register),
    ];
    
    private static readonly Pointer[] EncodedRegisters =
    [
        Pointer.R0, Pointer.R1, Pointer.R2, 
        Pointer.R3, Pointer.R4, Pointer.R5, 
        Pointer.SP, Pointer.PC,
    ];
}