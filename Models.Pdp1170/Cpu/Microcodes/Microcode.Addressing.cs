namespace Models.Pdp1170.Cpu.Microcodes;

public partial class Microcode
{
    private static Signal[] REGISTER(byte index, Pointer target) => 
    [
        REG_WRITE(EncodedRegister(index), target),
    ];
    private static Signal[] REGISTER_DEFERRED(byte index, Pointer target) =>
    [
        MEM_READ(EncodedRegister(index)),
        REG_WRITE(Pointer.MDR, target),
    ];
    
    private static Signal[] AUTO_INC(byte index, Pointer target) =>
    [
        REG_WRITE(EncodedRegister(index), target),
        ..INCREMENT(EncodedRegister(index), StepSize(index)),
    ];
    private static Signal[] AUTO_INC_DEFERRED(byte index, Pointer target) =>
    [
        MEM_READ(EncodedRegister(index)),
        REG_WRITE(Pointer.MDR, target),
        ..INCREMENT(EncodedRegister(index), Width.WORD),
    ];
    
    private static Signal[] AUTO_DEC(byte index, Pointer target) =>
    [
        ..DECREMENT(EncodedRegister(index), StepSize(index)),
        REG_WRITE(EncodedRegister(index), target),
    ];
    private static Signal[] AUTO_DEC_DEFERRED(byte index, Pointer target) =>
    [
        ..DECREMENT(EncodedRegister(index), Width.WORD),
        MEM_READ(EncodedRegister(index)),
        REG_WRITE(Pointer.MDR, target),
    ];
    
    private static Signal[] INDEX(byte index, Pointer target) =>
    [
        ..READ_IMM,
        ALU_COMPUTE(Operation.ADD, EncodedRegister(index), Pointer.MDR, Flag.NONE),
        REG_WRITE(Pointer.TMP, target),
    ];
    private static Signal[] INDEX_DEFERRED(byte index, Pointer target) =>
    [
        ..READ_IMM,
        ALU_COMPUTE(Operation.ADD, EncodedRegister(index), Pointer.MDR, Flag.NONE),
        MEM_READ(Pointer.TMP),
        REG_WRITE(Pointer.MDR, target),
    ];
}