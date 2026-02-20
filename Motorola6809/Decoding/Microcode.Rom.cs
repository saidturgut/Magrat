namespace Motorola6809.Decoding;
using Computing;
using Kernel;

public static partial class Microcode
{
    private static readonly Dictionary<string, Func<Signal[]>> MainPage = new()
    {
        ["-"] = () => [],

        ["NOP"] = () => IDLE, ["SYNC"] = () => [STATE_COMMIT(State.HALT)],
        ["PFX_10"] = () => [STATE_COMMIT(State.DEC_10)], ["PFX_11"] = () => [STATE_COMMIT(State.DEC_11)],
        
        // ---------------------------------------- LOAD & STORE ---------------------------------------- //
        
        // LOAD
        ["LDD"] = () => LOAD_WORD(DD!),
        ["LDA"] = () => LOAD_BYTE(Pointer.A), ["LDB"] = () => LOAD_BYTE(Pointer.B),
        ["LDX"] = () => LOAD_WORD(IX!), ["LDY"] = () => LOAD_WORD(IY!), 
        ["LDS"] = () => LOAD_WORD(SP!), ["LDU"] = () => LOAD_WORD(UP!),
        // STORE
        ["STD"] = () => STORE_WORD(DD!),
        ["STA"] = () => STORE_BYTE(Pointer.A), ["STB"] = () => STORE_BYTE(Pointer.B),
        ["STX"] = () => STORE_WORD(IX!), ["STY"] = () => STORE_WORD(IY!), 
        ["STS"] = () => STORE_WORD(SP!), ["STU"] = () => STORE_WORD(UP!),
        
        // ---------------------------------------- CORE ALU ---------------------------------------- //
        
        // ADD
        ["ADDD"] = () => ALU_WORD(DD!, ADD16!, FlagMask.CVZN),
        ["ADDA"] = () => ALU_BYTE(Pointer.A, Operation.ADD, FlagMask.CVZNH), ["ADDB"] = () => ALU_BYTE(Pointer.B, Operation.ADD, FlagMask.CVZNH),
        ["ADCA"] = () => ALU_BYTE(Pointer.A, Operation.ADC, FlagMask.CVZNH), ["ADCB"] = () => ALU_BYTE(Pointer.B, Operation.ADC, FlagMask.CVZNH),
        // SUBTRACT
        ["SUBD"] = () => ALU_WORD(DD!, SUB16!, FlagMask.CVZN),
        ["SUBA"] = () => ALU_BYTE(Pointer.A, Operation.SUB, FlagMask.CVZNH), ["SUBB"] = () => ALU_BYTE(Pointer.B, Operation.SUB, FlagMask.CVZNH),
        ["SBCA"] = () => ALU_BYTE(Pointer.A, Operation.SBC, FlagMask.CVZNH), ["SBCB"] = () => ALU_BYTE(Pointer.B, Operation.SBC, FlagMask.CVZNH),
        // COMPARE
        ["CMPD"] = () => FRU_WORD(DD!, SUB16!, FlagMask.CVZN),
        ["CMPA"] = () => FRU_BYTE(Pointer.A, Operation.SUB, FlagMask.CVZNH), ["CMPB"] = () => FRU_BYTE(Pointer.B, Operation.SUB, FlagMask.CVZNH),
        ["CMPX"] = () => FRU_WORD(IX!, SUB16!, FlagMask.CVZN), ["CMPY"] = () => FRU_WORD(IY!, SUB16!, FlagMask.CVZN),
        // LOGIC
        ["ANDA"] = () => ALU_BYTE(Pointer.A, Operation.AND, FlagMask.VZN), ["ANDB"] = () => ALU_BYTE(Pointer.B, Operation.AND, FlagMask.VZN),
        ["ORA"] = () => ALU_BYTE(Pointer.A, Operation.OR, FlagMask.VZN), ["ORB"] = () => ALU_BYTE(Pointer.B, Operation.OR, FlagMask.VZN),
        ["EORA"] = () => ALU_BYTE(Pointer.A, Operation.EOR, FlagMask.VZN), ["EORB"] = () => ALU_BYTE(Pointer.B, Operation.EOR, FlagMask.VZN),
        ["BITA"] = () => FRU_BYTE(Pointer.A, Operation.AND, FlagMask.VZN), ["BITB"] = () => FRU_BYTE(Pointer.B, Operation.AND, FlagMask.VZN),
        
        // ---------------------------------------- BITWISE ALU ---------------------------------------- //
        
        // SHIFT
        ["LSRA"] = () => ALU_BYTE(Pointer.A, Operation.LSR, FlagMask.CVZN), ["LSRB"] = () => ALU_BYTE(Pointer.B, Operation.LSR, FlagMask.CVZN), 
        ["LSR"] = () => ALU_MEM(Operation.LSR, FlagMask.CVZN),
        ["LSLA"] = () => ALU_BYTE(Pointer.A, Operation.LSL, FlagMask.CVZN), ["LSLB"] = () => ALU_BYTE(Pointer.B, Operation.LSL, FlagMask.CVZN), 
        ["LSL"] = () => ALU_MEM(Operation.LSL, FlagMask.CVZN),
        ["ASRA"] = () => ALU_BYTE(Pointer.A, Operation.ASR, FlagMask.CVZN), ["ASRB"] = () => ALU_BYTE(Pointer.B, Operation.ASR, FlagMask.CVZN), 
        ["ASR"] = () => ALU_MEM(Operation.ASR, FlagMask.CVZN),
        // ROTATE
        ["RORA"] = () => ALU_BYTE(Pointer.A, Operation.ROR, FlagMask.CVZN), ["RORB"] = () => ALU_BYTE(Pointer.B, Operation.ROR, FlagMask.CVZN), 
        ["ROR"] = () => ALU_MEM(Operation.ROR, FlagMask.CVZN),
        ["ROLA"] = () => ALU_BYTE(Pointer.A, Operation.ROL, FlagMask.CVZN), ["ROLB"] = () => ALU_BYTE(Pointer.B, Operation.ROL, FlagMask.CVZN), 
        ["ROL"] = () => ALU_MEM(Operation.ROL, FlagMask.CVZN),
        // PRIME
        ["NEGA"] = () => ALU_BYTE(Pointer.A, Operation.NEG, FlagMask.CVZN), ["NEGB"] = () => ALU_BYTE(Pointer.B, Operation.NEG, FlagMask.CVZN), 
        ["NEG"] = () => ALU_MEM(Operation.NEG, FlagMask.CVZN),
        ["COMA"] = () => ALU_BYTE(Pointer.A, Operation.COM, FlagMask.CVZN), ["COMB"] = () => ALU_BYTE(Pointer.B, Operation.COM, FlagMask.CVZN),
        ["COM"] = () => ALU_MEM(Operation.COM, FlagMask.CVZN),
        // TEST
        ["TSTA"] = () => FRU_BYTE(Pointer.A, Operation.OR, FlagMask.VZN), ["TSTB"] = () => FRU_BYTE(Pointer.B, Operation.OR, FlagMask.VZN), 
        ["TST"] = () => FRU_MEM(Operation.OR, FlagMask.VZN),
        // CLEAR
        ["CLRA"] = () => ALU_BYTE(Pointer.A, Operation.NONE, FlagMask.CVZN), ["CLRB"] = () => ALU_BYTE(Pointer.B, Operation.NONE, FlagMask.CVZN), 
        ["CLR"] = () => ALU_MEM(Operation.NONE, FlagMask.CVZN),
        // INC 
        ["INCA"] = () => ALU_BYTE(Pointer.A, Operation.INC, FlagMask.VZN), ["INCB"] = () => ALU_BYTE(Pointer.B, Operation.INC, FlagMask.VZN), 
        ["INC"] = () => ALU_MEM(Operation.INC, FlagMask.VZN),
        // DEC
        ["DECA"] = () => ALU_BYTE(Pointer.A, Operation.DEC, FlagMask.VZN), ["DECB"] = () => ALU_BYTE(Pointer.B, Operation.DEC, FlagMask.VZN), 
        ["DEC"] = () => ALU_MEM(Operation.DEC, FlagMask.VZN),
    };
}