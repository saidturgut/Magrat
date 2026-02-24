namespace Models.Pdp1170.Cpu.Microcodes;

public partial class Microcode
{
    private static Signal[] TWO_OPERAND(Operation operation, bool writeback, Width width)
    {
        Descriptor info = new()
        {
            Modes = [ooooxo(), ooxooo()],
            Regs = [EncodedRegisters[ooooox()], EncodedRegisters[oooxoo()]],
            Operation = operation,
            Mask = FlagMasks[operation is not Operation.PASS ? FlagMask.CVZN : FlagMask.VZN],
            Writeback = writeback, Width = width,
        };
        return
        [
            ..SET_NAME($"{cell} {AddressingModeName(ooxxoo())},{AddressingModeName(ooooxx())}"),
            ..ADDRESS[info.Modes[1]](info.Regs[1], Pointer.SRC, info.Width),
            ..ADDRESS[info.Modes[0]](info.Regs[0], Pointer.DST, info.Width),
            ..EXECUTE(info),
            ..WRITEBACK(info),
        ];
    }
    
    private static Signal[] ONE_OPERAND(Operation operation, Width width)
    {
        Descriptor info = new()
        {
            Modes = [ooooxo()],
            Regs = [EncodedRegisters[ooooox()]],
            Operation = operation,
            Mask = operation switch
            {
                Operation.INC or Operation.DEC => FlagMasks[FlagMask.VZN],
                Operation.SXT => FlagMasks[FlagMask.VZ],
                _ => FlagMasks[FlagMask.CVZN],
            },
            Writeback = operation is not Operation.TST, Width = width,
        };
        return
        [
            ..SET_NAME($"{cell} {AddressingModeName(ooooxx())}"),
            ..ADDRESS[info.Modes[0]](info.Regs[0], Pointer.DST, info.Width),
            ..EXECUTE(info),
            ..WRITEBACK(info),
        ];
    }

    private static Signal[] JUMP() => ooooxo() != 0 ?
        [
            ..SET_NAME($"JMP {AddressingModeName(ooooxx())}"),
            ..ADDRESS[ooooxo()](EncodedRegisters[ooooox()], Pointer.DST, Width.WORD),
            REG_MOVE(Pointer.EA, Pointer.PC),
        ] : NONE;

    private static Signal[] BRANCH(Condition condition) =>
    [
        ..SET_NAME($"B{condition} {(sbyte)(opcode & 0xFF)}"),
        REG_MOVE(Pointer.PSW, Pointer.TMP),
        COND_COMPUTE(condition, State.FETCH),
        ALU_COMPUTE(Operation.BRC, Pointer.PC, Pointer.IR, Flag.NONE),
        REG_MOVE(Pointer.TMP, Pointer.PC),
    ];
}