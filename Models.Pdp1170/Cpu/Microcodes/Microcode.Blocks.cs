namespace Models.Pdp1170.Cpu.Microcodes;

public partial class Microcode
{
    private static Signal[] TWO_OPR(Operation operation, bool writeback, Width width)
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
    
    private static Signal[] ONE_OPR(Operation operation, Width width)
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
}