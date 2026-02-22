namespace Models.Pdp1170.Cpu.Microcodes;

public partial class Microcode
{
    // ------------------------------------ MACROS ------------------------------------ //

    private static Width StepSize(byte index)
        => encodedWidth == Width.BYTE && EncodedRegister(index) is not (Pointer.PC or Pointer.SP)
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
    
    private static Pointer EncodedRegister(byte index)
        => EncodedRegisters[encodedRegs[index]];

    private static readonly Pointer[] EncodedRegisters =
    [
        Pointer.R0, Pointer.R1, Pointer.R2, 
        Pointer.R3, Pointer.R4, Pointer.R5, 
        Pointer.SP, Pointer.PC,
    ];

    private static readonly Func<byte, Pointer, Signal[]>[] AddressingModes =
    [
        REGISTER, REGISTER_DEFERRED, 
        AUTO_INC, AUTO_INC_DEFERRED, AUTO_DEC, AUTO_DEC_DEFERRED, 
        INDEX, INDEX_DEFERRED
    ];

    private static bool MemoryWriteback()
        => encodedModes[1] != 0;
}