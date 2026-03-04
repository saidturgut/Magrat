namespace Models.Pdp1170.Kernel.Microcodes;

public partial class Microcode
{
    // ------------------------------- JUMPS ------------------------------- //

    private static Signal[] JUMP() => ooooxo() != 0
        ?
        [
            ..SET_NAME($"JMP {AddressingModeName(ooooxx())}"),
            ..ADDRESS[ooooxo()](EncodedRegisters[ooooox()], Pointer.DST, Width.WORD),
            REG_MOVE(Pointer.EA, Pointer.PC),
        ]
        : ILLEGAL;

    private static Signal[] JUMP_SR(Pointer register) =>
    [
        ..SET_NAME($"JSR {register},{AddressingModeName(ooooxx())}"),
        ..ADDRESS[ooooxo()](EncodedRegisters[ooooox()], Pointer.SRC, Width.WORD),
        ..PUSH(register),
        REG_MOVE(Pointer.PC, register),
        REG_MOVE(Pointer.EA, Pointer.PC),
    ];

    // ------------------------------- RETURNS ------------------------------- //

    private static Signal[] RET_SR() =>
    [
        ..SET_NAME($"RST {EncodedRegisters[ooooox()]}"),
        REG_MOVE(EncodedRegisters[ooooox()], Pointer.PC),
        ..POP(EncodedRegisters[ooooox()]),
    ];

    private static Signal[] RET_INT(string name) =>
    [
        ..SET_NAME(name),
        ..POP(Pointer.PC),
        ..POP(Pointer.PSW),
    ];

    private static Signal[] RET_TRC() =>
    [
        ..RET_INT("RTT"),
        STATE_COMMIT(State.SUPPRESS)
    ];

    // ------------------------------- BRANCHES ------------------------------- //

    private static Signal[] BRANCH(Condition condition) =>
    [
        ..SET_NAME($"B{condition} {(sbyte)(opcode & 0xFF)}"),
        REG_MOVE(Pointer.PSW, Pointer.TMP),
        COND_COMPUTE(condition, State.FETCH),
        ALU_COMPUTE(Operation.BRC, Pointer.PC, Pointer.IR, Flag.NONE),
        REG_MOVE(Pointer.TMP, Pointer.PC),
    ];
    
    private static Signal[] SOB()
    {
        Descriptor info = new()
            { Regs = [EncodedRegisters[oooxoo()]] };
        return
        [
            ..SET_NAME($"SOB {Tools.Octal(ooooxx())},{info.Regs[0]}"),
            REG_MOVE(Pointer.PSW, Pointer.SRC),
            ALU_COMPUTE(Operation.DEC, info.Regs[0], Pointer.NIL, Flag.ZERO),
            REG_MOVE(Pointer.TMP, info.Regs[0]),
            REG_MOVE(Pointer.PSW, Pointer.TMP),
            REG_MOVE(Pointer.SRC, Pointer.PSW),
            COND_COMPUTE(Condition.NE, State.FETCH),
            ALU_COMPUTE(Operation.SOB, Pointer.PC, Pointer.IR, Flag.NONE),
            REG_MOVE(Pointer.TMP, Pointer.PC),
        ];
    }

    // ------------------------------- MISC ------------------------------- //

    private static Signal[] MARK() =>
    [
        ..SET_NAME("MARK"),
        ALU_COMPUTE(Operation.MARK, Pointer.PC, Pointer.IR, Flag.NONE),
        REG_MOVE(Pointer.TMP, Pointer.SP),
        REG_MOVE(Pointer.R5, Pointer.PC),
        ..POP(Pointer.R5),
    ];
}