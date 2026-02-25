namespace Models.Pdp1170.Cpu.Computing;

public partial class Alu
{
    private static AluOutput CLR(AluInput input) => new()
        { Result = 0x0000, Flags = 0x0000, ByteMask = ByteMask.PRES_HIGH };
    
    private static AluOutput COM(AluInput input)
    {
        input.A = MaskHigh(input.A);
        AluOutput output = new() { Result = MaskHigh((ushort)(~input.A)), ByteMask = ByteMask.PRES_HIGH };
        output.Flags |= (ushort)Flag.CARRY;
        return output;
    }
    
    private static AluOutput INC(AluInput input)
    {
        AluOutput output = new() { Result = MaskHigh((ushort)(MaskHigh(input.A) + 1)), ByteMask = ByteMask.PRES_HIGH };
        if (output.Result == x8000) output.Flags |= (ushort)Flag.OVERFLOW;
        return output;
    }
    private static AluOutput DEC(AluInput input)
    {
        AluOutput output = new() { Result = MaskHigh((ushort)(MaskHigh(input.A) - 1)), ByteMask = ByteMask.PRES_HIGH };
        if (output.Result == x8000 - 1) output.Flags |= (ushort)Flag.OVERFLOW;
        return output;
    }
    
    private static AluOutput NEG(AluInput input)
    {
        input.A = MaskHigh(input.A);
        AluOutput output = new() { Result = MaskHigh((ushort)(0 - input.A)), ByteMask = ByteMask.PRES_HIGH };
        if (input.A != 0) output.Flags |= (ushort)Flag.CARRY;
        if (input.A == x8000) output.Flags |= (ushort)Flag.OVERFLOW;
        return output;
    }
    
    private static AluOutput ADC(AluInput input)
    {
        input.A = MaskHigh(input.A);
        Flags flags = new Flags(input.Psw);
        uint result = (uint)(input.A + CarryIn(flags));
        AluOutput output = new() { Result = MaskHigh((ushort)result), ByteMask = ByteMask.PRES_HIGH };
        if((result & x10000) != 0) output.Flags |= (ushort)Flag.CARRY;
        if (OverflowAdd(input.A, CarryIn(flags), output.Result)) output.Flags |= (ushort)Flag.OVERFLOW;
        return output;
    }
    private static AluOutput SBC(AluInput input)
    {
        input.A = MaskHigh(input.A);
        Flags flags = new Flags(input.Psw);
        AluOutput output = new() { Result = MaskHigh((ushort)(input.A - CarryIn(flags))), ByteMask = ByteMask.PRES_HIGH };
        if (input.A >= CarryIn(flags)) output.Flags |= (ushort)Flag.CARRY;
        if (OverflowSub(input.A, CarryIn(flags), output.Result)) output.Flags |= (ushort)Flag.OVERFLOW;
        return output;
    }
    
    private static AluOutput TST(AluInput input)
        => new() { Result = MaskHigh(input.A), ByteMask = ByteMask.PRES_HIGH };
    
    private static AluOutput ROR(AluInput input)
    {
        input.A = MaskHigh(input.A);
        Flags flags = new Flags(input.Psw);
        AluOutput output = new() { Result = MaskHigh((ushort)((input.A >> 1) | (flags.Carry ? x8000 : 0))), ByteMask = ByteMask.PRES_HIGH };
        if (CarryBit(input.A, 1)) output.Flags |= (ushort)Flag.CARRY;
        if (OverflowBit(output.Result, CarryBit(input.A, 1))) output.Flags |= (ushort)Flag.OVERFLOW;
        return output;
    }
    private static AluOutput ROL(AluInput input)
    {
        input.A = MaskHigh(input.A);
        Flags flags = new Flags(input.Psw);
        AluOutput output = new() { Result = MaskHigh((ushort)((input.A << 1) | CarryIn(flags))), ByteMask = ByteMask.PRES_HIGH };
        if (CarryBit(input.A, x8000)) output.Flags |= (ushort)Flag.CARRY;
        if (OverflowBit(output.Result, CarryBit(input.A, x8000))) output.Flags |= (ushort)Flag.OVERFLOW;
        return output;
    }
    
    private static AluOutput ASR(AluInput input)
    {
        input.A = MaskHigh(input.A);
        AluOutput output = new() { Result = MaskHigh((ushort)((input.A >> 1) | (input.A & x8000))), ByteMask = ByteMask.PRES_HIGH };
        if (CarryBit(input.A, 1)) output.Flags |= (ushort)Flag.CARRY;
        if (OverflowBit(output.Result, CarryBit(input.A, 1))) output.Flags |= (ushort)Flag.OVERFLOW;
        return output;
    }
    private static AluOutput ASL(AluInput input)
    {
        input.A = MaskHigh(input.A);
        AluOutput output = new() { Result = MaskHigh((ushort)(input.A << 1)), ByteMask = ByteMask.PRES_HIGH };
        if (CarryBit(input.A, x8000)) output.Flags |= (ushort)Flag.CARRY;
        if (OverflowBit(output.Result, CarryBit(input.A, x8000))) output.Flags |= (ushort)Flag.OVERFLOW;
        return output;
    }

    private static AluOutput SXT(AluInput input) => new()
        { Result = (ushort)(new Flags(input.Psw).Negative ? 0xFFFF : 0x0000) };
    
    private static AluOutput SWAB(AluInput input)
    {
        AluOutput output = new() { Result = (ushort)(((input.A & 0x00FF) << 8) | ((input.A & 0xFF00) >> 8)), Custom = true };
        byte low = (byte)(output.Result & 0xFF);
        if((low & 0x80) != 0) output.Flags |= (ushort)Flag.NEGATIVE;
        if(low == 0) output.Flags |= (ushort)Flag.ZERO;
        return output;
    }
}