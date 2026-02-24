namespace Models.Pdp1170.Cpu.Microcodes;

public partial class Microcode
{
        private static readonly Dictionary<string, Func<Signal[]>> BlockCellTable = new()
    {
        ["-"] = () => NONE,
        ["MOV"] = () => TWO_OPERAND(Operation.PASS, true, Width.WORD), ["MOVB"] = () => TWO_OPERAND(Operation.PASS, true, Width.BYTE),
        ["CMP"] = () => TWO_OPERAND(Operation.SUB, false, Width.WORD), ["CMPB"] = () => TWO_OPERAND(Operation.SUB, false, Width.BYTE),
        ["BIT"] = () => TWO_OPERAND(Operation.BIT, false, Width.WORD), ["BITB"] = () => TWO_OPERAND(Operation.BIT, false, Width.BYTE),
        ["BIC"] = () => TWO_OPERAND(Operation.BIC, true, Width.WORD), ["BICB"] = () => TWO_OPERAND(Operation.BIC, true, Width.BYTE),
        ["BIS"] = () => TWO_OPERAND(Operation.BIS, true, Width.WORD), ["BISB"] = () => TWO_OPERAND(Operation.BIS, true, Width.BYTE),
        ["ADD"] = () => TWO_OPERAND(Operation.ADD, true, Width.WORD), ["SUB"] = () => TWO_OPERAND(Operation.SUB, true, Width.WORD),

        ["CLR"] = () => ONE_OPERAND(Operation.CLR, Width.WORD), ["CLRB"] = () => ONE_OPERAND(Operation.CLR, Width.BYTE),
        ["COM"] = () => ONE_OPERAND(Operation.COM, Width.WORD), ["COMB"] = () => ONE_OPERAND(Operation.COM, Width.BYTE),
        ["INC"] = () => ONE_OPERAND(Operation.INC, Width.WORD), ["INCB"] = () => ONE_OPERAND(Operation.INC, Width.BYTE),
        ["DEC"] = () => ONE_OPERAND(Operation.DEC, Width.WORD), ["DECB"] = () => ONE_OPERAND(Operation.DEC, Width.BYTE),
        ["NEG"] = () => ONE_OPERAND(Operation.NEG, Width.WORD), ["NEGB"] = () => ONE_OPERAND(Operation.NEG, Width.BYTE),
        ["ADC"] = () => ONE_OPERAND(Operation.ADC, Width.WORD), ["ADCB"] = () => ONE_OPERAND(Operation.ADC, Width.BYTE),
        ["SBC"] = () => ONE_OPERAND(Operation.SBC, Width.WORD), ["SBCB"] = () => ONE_OPERAND(Operation.SBC, Width.BYTE),
        ["TST"] = () => ONE_OPERAND(Operation.TST, Width.WORD), ["TSTB"] = () => ONE_OPERAND(Operation.ZRO, Width.BYTE),
        ["ROR"] = () => ONE_OPERAND(Operation.ROR, Width.WORD), ["RORB"] = () => ONE_OPERAND(Operation.ROR, Width.BYTE),
        ["ROL"] = () => ONE_OPERAND(Operation.ROL, Width.WORD), ["ROLB"] = () => ONE_OPERAND(Operation.ROL, Width.BYTE),
        ["ASR"] = () => ONE_OPERAND(Operation.ASR, Width.WORD), ["ASRB"] = () => ONE_OPERAND(Operation.ASR, Width.BYTE),
        ["ASL"] = () => ONE_OPERAND(Operation.ASL, Width.WORD), ["ASLB"] = () => ONE_OPERAND(Operation.ASL, Width.BYTE),
        ["SXT"] = () => ONE_OPERAND(Operation.SXT, Width.WORD), ["SWAB"] = () => ONE_OPERAND(Operation.SWAB, Width.WORD),

        ["BRC"] = () => BRANCH((Condition)((opcode >> 8) & 0xF)), ["JMP"] = JUMP,
        ["JSR"] = () => NONE,
        ["MFPI"] = () => NONE, ["MTPI"] = () => NONE,
        ["MFPD"] = () => NONE, ["MTPD"] = () => NONE,
        ["MUL"] = () => NONE, ["DIV"] = () => NONE,
        ["ASH"] = () => NONE, ["ASHC"] = () => NONE,
        ["XOR"] = () => NONE, ["SOB"] = () => NONE,
        ["EMT"] = () => NONE, ["TRAP"] = () => NONE, ["MARK"] = () => NONE,
    };
    
    private static readonly Dictionary<ushort, Func<Signal[]>> FixedOpcodeTable = new()
    {
        [0x0000] = () => STATE_CHANGE(State.HALT, "HALT"), // HALT
        [0x00A0] = () => STATE_CHANGE(State.FETCH, "NOP"), // NOP
        
        [0x0098] = () => SET_PRIORITY(0, 0, 0), [0x0099] = () => SET_PRIORITY(0, 0, 1),
        [0x009A] = () => SET_PRIORITY(0, 1, 0), [0x009B] = () => SET_PRIORITY(0, 1, 1),
        [0x009C] = () => SET_PRIORITY(1, 0, 0), [0x009D] = () => SET_PRIORITY(1, 0, 1),
        [0x009E] = () => SET_PRIORITY(1, 1, 0), [0x009F] = () => SET_PRIORITY(1, 1, 1),
        
        [0x00A1] = () => PSW_WRITE(Flag.CARRY, false, "CLC"), [0x00B1] = () => PSW_WRITE(Flag.CARRY, true, "SEC"),
        [0x00A2] = () => PSW_WRITE(Flag.OVERFLOW, false, "CLV"), [0x00B2] = () => PSW_WRITE(Flag.OVERFLOW, true, "SEV"),
        [0x00A4] = () => PSW_WRITE(Flag.ZERO, false, "CLZ"), [0x00B4] = () => PSW_WRITE(Flag.ZERO, true, "SEZ"),
        [0x00A8] = () => PSW_WRITE(Flag.NEGATIVE, false, "CLN"), [0x00B8] = () => PSW_WRITE(Flag.NEGATIVE, true, "SEN"),
        [0x00AF] = () => PSW_WRITE(FlagMasks![FlagMask.CVZN], false, "CCC"), 
        [0x00BF] = () => PSW_WRITE(FlagMasks![FlagMask.CVZN], true, "SCC"),
    };
    
    private static readonly Dictionary<FlagMask, Flag> FlagMasks = new()
    {
        { FlagMask.NONE, Flag.NONE },
        { FlagMask.CVZN, Flag.CARRY | Flag.OVERFLOW | Flag.ZERO | Flag.NEGATIVE },
        { FlagMask.VZN, Flag.OVERFLOW | Flag.ZERO | Flag.NEGATIVE },
        { FlagMask.VZ, Flag.OVERFLOW | Flag.ZERO },
        { FlagMask.ZN, Flag.ZERO | Flag.NEGATIVE },
    };
}
public enum FlagMask
{
    NONE, CVZN, VZN, VZ, ZN,
}