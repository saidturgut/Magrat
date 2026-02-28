namespace Models.Pdp1170.Cpu.Microcodes;

public partial class Microcode
{
        private static readonly Dictionary<string, Func<Signal[]>> BlockCellTable = new()
    {
        ["-"] = () => ILLEGAL,
        ["MOV"] = () => TWO_OPR(Operation.PASS, true, Width.WORD), ["MOVB"] = () => TWO_OPR(Operation.PASS, true, Width.BYTE),
        ["CMP"] = () => TWO_OPR(Operation.SUB, false, Width.WORD), ["CMPB"] = () => TWO_OPR(Operation.SUB, false, Width.BYTE),
        ["BIT"] = () => TWO_OPR(Operation.BIT, false, Width.WORD), ["BITB"] = () => TWO_OPR(Operation.BIT, false, Width.BYTE),
        ["BIC"] = () => TWO_OPR(Operation.BIC, true, Width.WORD), ["BICB"] = () => TWO_OPR(Operation.BIC, true, Width.BYTE),
        ["BIS"] = () => TWO_OPR(Operation.BIS, true, Width.WORD), ["BISB"] = () => TWO_OPR(Operation.BIS, true, Width.BYTE),
        ["ADD"] = () => TWO_OPR(Operation.ADD, true, Width.WORD), ["SUB"] = () => TWO_OPR(Operation.SUB, true, Width.WORD),

        ["CLR"] = () => ONE_OPR(Operation.CLR, Width.WORD), ["CLRB"] = () => ONE_OPR(Operation.CLR, Width.BYTE),
        ["COM"] = () => ONE_OPR(Operation.COM, Width.WORD), ["COMB"] = () => ONE_OPR(Operation.COM, Width.BYTE),
        ["INC"] = () => ONE_OPR(Operation.INC, Width.WORD), ["INCB"] = () => ONE_OPR(Operation.INC, Width.BYTE),
        ["DEC"] = () => ONE_OPR(Operation.DEC, Width.WORD), ["DECB"] = () => ONE_OPR(Operation.DEC, Width.BYTE),
        ["NEG"] = () => ONE_OPR(Operation.NEG, Width.WORD), ["NEGB"] = () => ONE_OPR(Operation.NEG, Width.BYTE),
        ["ADC"] = () => ONE_OPR(Operation.ADC, Width.WORD), ["ADCB"] = () => ONE_OPR(Operation.ADC, Width.BYTE),
        ["SBC"] = () => ONE_OPR(Operation.SBC, Width.WORD), ["SBCB"] = () => ONE_OPR(Operation.SBC, Width.BYTE),
        ["TST"] = () => ONE_OPR(Operation.TST, Width.WORD), ["TSTB"] = () => ONE_OPR(Operation.TST, Width.BYTE),
        ["ROR"] = () => ONE_OPR(Operation.ROR, Width.WORD), ["RORB"] = () => ONE_OPR(Operation.ROR, Width.BYTE),
        ["ROL"] = () => ONE_OPR(Operation.ROL, Width.WORD), ["ROLB"] = () => ONE_OPR(Operation.ROL, Width.BYTE),
        ["ASR"] = () => ONE_OPR(Operation.ASR, Width.WORD), ["ASRB"] = () => ONE_OPR(Operation.ASR, Width.BYTE),
        ["ASL"] = () => ONE_OPR(Operation.ASL, Width.WORD), ["ASLB"] = () => ONE_OPR(Operation.ASL, Width.BYTE),
        ["SXT"] = () => ONE_OPR(Operation.SXT, Width.WORD), ["SWAB"] = () => ONE_OPR(Operation.SWAB, Width.WORD),

        ["BRC"] = () => BRANCH((Condition)((opcode >> 8) > 7 ? (((opcode >> 8) & 0xF) + 8) : (((opcode >> 8) & 0xF) - 1))), 
        ["JMP"] = JUMP, ["JSR"] = () => JUMP_SR(EncodedRegisters![oooxoo()]),
        ["MFPI"] = () => ILLEGAL, ["MTPI"] = () => ILLEGAL,
        ["MFPD"] = () => ILLEGAL, ["MTPD"] = () => ILLEGAL,
        ["MUL"] = () => ILLEGAL, ["DIV"] = () => ILLEGAL,
        ["ASH"] = () => ILLEGAL, ["ASHC"] = () => ILLEGAL,
        ["XOR"] = () => ILLEGAL, ["SOB"] = () => ILLEGAL,
        ["TRAP"] = () => TRAP_REQUEST(Trap.TRAP, "TRAP"), ["EMT"] = () => TRAP_REQUEST(Trap.EMT, "EMT"), 
        ["MARK"] = () => ILLEGAL,
    };
    
    private static readonly Dictionary<ushort, Func<Signal[]>> FixedOpcodeTable = new()
    {
        [0x0000] = () => STATE_CHANGE(State.HALT, "HALT"), // HALT
        [0x0001] = () => STATE_CHANGE(State.WAIT, "WAIT"), // HALT
        [0x00A0] = () => STATE_CHANGE(State.FETCH, "NOP"), // NOP

        [0x0003] = () => TRAP_REQUEST(Trap.BPT, "BPT"), [0x0004] = () => TRAP_REQUEST(Trap.IOT, "IOT"),
        
        [0x0002] = () => RET_INT("RTI"), [0x0006] = RET_TRC,
        
        [0x0080] = RET_SR, [0x0081] = RET_SR, [0x0082] = RET_SR, [0x0083] = RET_SR,
        [0x0084] = RET_SR, [0x0085] = RET_SR, [0x0086] = RET_SR, [0x0087] = RET_SR,
        
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
    
    private static readonly Pointer[] EncodedRegisters =
    [
        Pointer.R0, Pointer.R1, Pointer.R2, 
        Pointer.R3, Pointer.R4, Pointer.R5, 
        Pointer.SP, Pointer.PC,
    ];
    
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