namespace Mos6502.Signaling.Microcodes;
using Executing.Computing;

public static partial class Microcode
{
    private static readonly Dictionary<string, Func<Signal[]>> AddressingTable = new()
    {
        ["-"] = () => [],
        ["IMP"] = () => IMPLIED,
        ["IMM"] = () => IMMEDIATE,
        ["ZP"] = () => ZERO_PAGE,
        ["ZPX"] = () => ZERO_PAGE_IDX(Pointer.IX),
        ["ZPY"] = () => ZERO_PAGE_IDX(Pointer.IY),
        ["ABS"] = () => ABSOLUTE,
        ["ABSX"] = () => ABSOLUTE_IDX(Pointer.IX),
        ["ABSY"] = () => ABSOLUTE_IDX(Pointer.IY),
        ["IND"] = () => INDIRECT,
        ["INDX"] = () => INDIRECT_X,
        ["INDY"] = () => INDIRECT_Y,
    };
    
    private static readonly Dictionary<string, Func<Signal[]>> MnemonicTable = new()
    {
        ["-"] = () => [STATE_COMMIT(Cycle.IDLE)],
        ["NOP"] = () => [STATE_COMMIT(Cycle.IDLE)],
        ["HLT"] = () => [STATE_COMMIT(Cycle.HALT)],

        // LOAD & STORE
        ["LDA"] = () => LOAD(Pointer.A), ["LDX"] = () => LOAD(Pointer.IX), ["LDY"] = () => LOAD(Pointer.IY),
        ["STA"] = () => STORE(Pointer.A), ["STX"] = () => STORE(Pointer.IX), ["STY"] = () => STORE(Pointer.IY),

        // TRANSFER
        ["TAX"] = () => TRANSFER(Pointer.A, Pointer.IX, true), ["TAY"] = () => TRANSFER(Pointer.A, Pointer.IY, true),
        ["TXA"] = () => TRANSFER(Pointer.IX, Pointer.A, true), ["TYA"] = () => TRANSFER(Pointer.IY, Pointer.A, true),
        ["TXS"] = () => TRANSFER(Pointer.IX, Pointer.SPL, true), ["TSX"] = () => TRANSFER(Pointer.SPL, Pointer.IX, false),

        //-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-//

        // INC & DEC
        ["INC"] = () => ALU_MEM(Operation.INC, FlagMask.ZN), ["DEC"] = () => ALU_MEM(Operation.DEC, FlagMask.ZN),
        ["INX"] = () => ALU_REG(Operation.INC, Pointer.IX, FlagMask.ZN), ["DEX"] = () => ALU_REG(Operation.DEC, Pointer.IX, FlagMask.ZN),
        ["INY"] = () => ALU_REG(Operation.INC, Pointer.IY, FlagMask.ZN), ["DEY"] = () => ALU_REG(Operation.DEC, Pointer.IY, FlagMask.ZN),
        
        // ARITHMETIC & LOGIC
        ["ADC"] = () => ALU(Operation.ADC, Pointer.A, FlagMask.CZVN, true), ["SBC"] = () => ALU(Operation.SBC, Pointer.A, FlagMask.CZVN, true),
        ["AND"] = () => ALU(Operation.AND, Pointer.A, FlagMask.ZN, true), ["CMP"] = () => ALU(Operation.CMP, Pointer.A, FlagMask.CZN, false),
        ["ORA"] = () => ALU(Operation.OR, Pointer.A, FlagMask.ZN, true), ["CPX"] = () => ALU(Operation.CMP, Pointer.IX, FlagMask.CZN, false),
        ["EOR"] = () => ALU(Operation.EOR, Pointer.A, FlagMask.ZN, true), ["CPY"] = () => ALU(Operation.CMP, Pointer.IY, FlagMask.CZN, false),
        
        // SHIFT & ROTATE
        ["BIT"] = () => ALU(Operation.BIT, Pointer.A, FlagMask.ZVN, false),
        ["ASL"] = () => ALU_MEM(Operation.ASL, FlagMask.CZN), ["ASLA"] = () => ALU_REG(Operation.ASL, Pointer.A, FlagMask.CZN), 
        ["LSR"] = () => ALU_MEM(Operation.LSR, FlagMask.CZN), ["LSRA"] = () => ALU_REG(Operation.LSR, Pointer.A, FlagMask.CZN),
        ["ROL"] = () => ALU_MEM(Operation.ROL, FlagMask.CZN), ["ROLA"] = () => ALU_REG(Operation.ROL, Pointer.A, FlagMask.CZN), 
        ["ROR"] = () => ALU_MEM(Operation.ROR, FlagMask.CZN), ["RORA"] = () => ALU_REG(Operation.ROR, Pointer.A, FlagMask.CZN),

        // FLAG CLEAR & SET
        ["SEC"] = () => FLAG(false, Flag.CARRY), ["SED"] = () => FLAG(false, Flag.DECIMAL), ["SEI"] = () => FLAG(false, Flag.INTERRUPT),
        ["CLC"] = () => FLAG(true, Flag.CARRY), ["CLD"] = () => FLAG(true, Flag.DECIMAL), ["CLI"] = () => FLAG(true, Flag.INTERRUPT), 
        ["CLV"] = () => FLAG(true, Flag.OVERFLOW),
        
        //-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-/-//

        // CONTROL FLOW
        ["JMP"] = () => JUMP, ["JSR"] = () => CALL,
        ["RTI"] = () => RETURN(true), ["RTS"] = () => RETURN(false),
        ["BRK"] = () => BREAK,

        // PUSH & PULL
        ["PHA"] = () => PUSH(Pointer.A), ["PHP"] = () => PUSH(Pointer.F),
        ["PLA"] = () => PULL(Pointer.A), ["PLP"] = () => PULL(Pointer.F),

        // BRANCH CLEAR & SET
        ["BCC"] = () => BRANCH(Condition.CC), ["BCS"] = () => BRANCH(Condition.CS),
        ["BNE"] = () => BRANCH(Condition.NE), ["BEQ"] = () => BRANCH(Condition.EQ),
        ["BPL"] = () => BRANCH(Condition.PL), ["BMI"] = () => BRANCH(Condition.MI),
        ["BVC"] = () => BRANCH(Condition.VC), ["BVS"] = () => BRANCH(Condition.VS),
    };
}