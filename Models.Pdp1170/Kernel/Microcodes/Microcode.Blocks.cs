namespace Models.Pdp1170.Kernel.Microcodes;

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
    
    private static Signal[] HALF_LONG(Operation first, Operation second, FlagMask mask)
    {
        if (oooxoo() % 2 != 0) return ILLEGAL;
        
        Descriptor info = new()
            { Regs = [EncodedRegisters[ooooox()], EncodedRegisters[oooxoo()], EncodedRegisters[oooxoo() + 1]], };
        return
        [
            ..SET_NAME($"{cell} {AddressingModeName(ooooxx())},{info.Regs[1]}"),
            ..ADDRESS[ooooxo()](info.Regs[0], Pointer.SRC, Width.WORD),
            ALU_COMPUTE(first, info.Regs[1], Pointer.SRC, info.Regs[2], Flag.NONE),
            REG_MOVE(Pointer.TMP, Pointer.DST),
            ALU_COMPUTE(second, info.Regs[1], Pointer.SRC, info.Regs[2], FlagMasks[mask]),
            REG_MOVE(Pointer.TMP, info.Regs[1]),
            REG_MOVE(Pointer.DST, info.Regs[2]),
        ];
    }
    
    private static Signal[] HALF_WORD(Operation first, FlagMask mask)
    {
        Descriptor info = new()
            { Regs = [EncodedRegisters[ooooox()], EncodedRegisters[oooxoo()]], };
        return
        [
            ..SET_NAME($"{cell} {AddressingModeName(ooooxx())},{info.Regs[1]}"),
            ..ADDRESS[ooooxo()](info.Regs[0], Pointer.SRC, Width.WORD),
            ALU_COMPUTE(first, info.Regs[1], Pointer.SRC, FlagMasks[mask]),
            REG_MOVE(Pointer.TMP, info.Regs[1]),
        ];
    }
    
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
}