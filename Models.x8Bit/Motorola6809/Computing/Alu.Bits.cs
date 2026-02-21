namespace Models.x8Bit.Motorola6809.Computing;
using Engine.Units;

public partial class Alu
{
    private static AluOutput LSR(AluInput input)
    {
        AluOutput output = new()
            { Result = (byte)(input.A >> 1) };
        if (Carry(input.A, 0)) output.Flags |= (byte)Flag.CARRY;
        if (Carry(input.A, 0)) output.Flags |= (byte)Flag.OVERFLOW;
        return output;
    }

    private static AluOutput LSL(AluInput input)
    {
        AluOutput output = new() 
            { Result = (byte)((input.A << 1) & 0xFF) };
        if (Carry(input.A, 7)) output.Flags |= (byte)Flag.CARRY;
        if (SignedOverflowBit(output.Result, input.A, 7)) output.Flags |= (byte)Flag.OVERFLOW;
        return output;
    }
    private static AluOutput ASR(AluInput input)
    {
        AluOutput output = new()
            { Result = (byte)((input.A >> 1) | (input.A & 0x80)) };
        if (Carry(input.A, 0)) output.Flags |= (byte)Flag.CARRY;
        return output;
    }

    private static AluOutput ROR(AluInput input)
    {
        AluOutput output = new()
            { Result = (byte)((input.A >> 1) | (input.C << 7)) };
        if (Carry(input.A, 0)) output.Flags |= (byte)Flag.CARRY;
        if (SignedOverflowBit(output.Result, input.A, 0)) output.Flags |= (byte)Flag.OVERFLOW;
        return output;
    }
    private static AluOutput ROL(AluInput input)
    {
        AluOutput output = new()
            { Result = (byte)(((input.A << 1) | input.C) & 0xFF) };
        if (Carry(input.A, 7)) output.Flags |= (byte)Flag.CARRY;
        if (SignedOverflowBit(output.Result, input.A, 7)) output.Flags |= (byte)Flag.OVERFLOW;
        return output;
    }

    private static AluOutput NEG(AluInput input)
    {
        AluOutput output = new()
            { Result = (byte)(0 - input.A) };
        if(input.A != 0) output.Flags |= (byte)Flag.CARRY;
        if(input.A == 0x80) output.Flags |= (byte)Flag.OVERFLOW;
        return output;
    }
    private static AluOutput COM(AluInput input)
    {
        AluOutput output = new()
            { Result = (byte)~input.A };
        output.Flags |= (byte)Flag.CARRY;
        return  output;
    }
    
    private static AluOutput INC(AluInput input)
    {
        AluOutput output = new() 
            { Result = (byte)(input.A + 1) };
        if(input.A == 0x7F) output.Flags |= (byte)Flag.OVERFLOW;
        return output; 
    }
    private static AluOutput DEC(AluInput input)
    {
        AluOutput output = new() 
            { Result = (byte)(input.A - 1) };
        if(input.A == 0x80) output.Flags |= (byte)Flag.OVERFLOW;
        return output; 
    }

    private static bool SignedOverflowBit(byte result, byte a, byte bit)
        => ((result & 0x80) ^ ((a >> bit) & 1)) != 0;
}