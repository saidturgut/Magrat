namespace Models.Pdp1170.Kernel.Computing;

public partial class Alu
{
    private static AluOutput MUL_L(AluInput input) => new()
        { Result = (ushort)(((short)input.A * (short)input.B) & 0xFFFF), };
    
    private static AluOutput MUL_H(AluInput input)
    {
        int result = (short)input.A * (short)input.B;
        var output = new AluOutput
            { Result = (ushort)((result >> 16) & 0xFFFF), Custom = true, };

        if (result < 0) output.Flags |= (ushort)Flag.NEGATIVE;
        if (result == 0) output.Flags |= (ushort)Flag.ZERO;
        if(input.A != (ushort)((short)input.B < 0 ? 0xFFFF : 0x0000)) output.Flags |= (ushort)Flag.OVERFLOW;

        return output;
    }
    
    private static (ushort Quotient, ushort Remainder, ushort Flags)
        Divide(ushort high, short divisor, ushort low)
    {
        ushort flags = 0;
        if (divisor == 0)
            return (high, low, (ushort)Flag.CARRY);

        int dividend = ((short)high << 16) | low;
        int quotient  = dividend / divisor;
        int remainder = dividend % divisor;

        if (quotient is < short.MinValue or > short.MaxValue)
            return (high, low, (ushort)Flag.OVERFLOW);
        if (quotient < 0) flags |= (ushort)Flag.NEGATIVE;
        if (quotient == 0) flags |= (ushort)Flag.ZERO;

        return ((ushort)quotient, (ushort)remainder, flags);
    }
    private static AluOutput DIV_L(AluInput input)
    {
        var (quotient, remainder, flags) = Divide(input.A, (short)input.B, input.C);
        return new AluOutput
            { Result = remainder, Custom = true, };
    }
    private static AluOutput DIV_H(AluInput input)
    {
        var (quotient, remainder, flags) = Divide(input.A, (short)input.B, input.C);
        return new AluOutput
            { Result = quotient, Flags = flags, Custom = true, };
    }

    private static (short Count, int value, int Result) ArithmeticShift(AluInput input)
    {
        int value = ((short)input.A << 16) | (ushort)input.C;
        int raw = input.B & 0x3F;
        short count = (short)((raw & 0x20) != 0 ? raw - 64 : raw);

        int result;

        if (count > 0)
        {
            if (count >= 32) result = 0;
            else result = value << count;
        }
        else if (count < 0)
        {
            int rcount = -count;
            if (rcount >= 32) result = value < 0 ? -1 : 0;
            else result = value >> rcount;
        }
        else result = value;

        return (count, value, result);
    }
    private static AluOutput ASH_L(AluInput input)
    {
        var (count, value, result) = ArithmeticShift(input);
        return new AluOutput
            { Result = (ushort)(result & 0xFFFF) };
    }
    private static AluOutput ASH_H(AluInput input)
    {
        var (count, value, result) = ArithmeticShift(input);
        var output = new AluOutput
            { Result = (ushort)((result >> 16) & 0xFFFF), Custom = true };

        if (result < 0) output.Flags |= (ushort)Flag.NEGATIVE;
        if (result == 0) output.Flags |= (ushort)Flag.ZERO;
        if (count != 0)
        {
            if (count > 0) output.Flags |= (ushort)(((value << (count - 1)) & 0x80000000) != 0 ? Flag.CARRY : 0);
            else output.Flags |= (ushort)(((value >> (-count - 1)) & 1) != 0 ? Flag.CARRY : 0);
        }
        if (count > 0 && value < 0 != result < 0)
            output.Flags |= (ushort)Flag.OVERFLOW;

        return output;
    }
    
    private static AluOutput ASH(AluInput input)
    {
        short value = (short)input.A;
        int raw = input.B & 0x3F;
        short count = (short)((raw & 0x20) != 0 ? raw - 64 : raw);
        
        short result;
        ushort flags = 0;

        if (count > 0)
        {
            if (count >= 16) result = 0;
            else result = (short)(value << count);

            if (count <= 16 && ((value << (count - 1)) & 0x8000) != 0)
                flags |= (ushort)Flag.CARRY;

            if ((value < 0) != (result < 0)) flags |= (ushort)Flag.OVERFLOW;
        }
        else if (count < 0)
        {
            int rcount = -count;
            if (rcount >= 16) result = value < 0 ? (short)-1 : (short)0;
            else result = (short)(value >> rcount);

            if (rcount <= 16 && ((value >> (rcount - 1)) & 1) != 0)
                flags |= (ushort)Flag.CARRY;
        }
        else result = value;

        if (result < 0) flags |= (ushort)Flag.NEGATIVE;
        if (result == 0) flags |= (ushort)Flag.ZERO;

        return new AluOutput
            { Result = (ushort)result, Flags = flags };
    }
    
    private static AluOutput XOR(AluInput input)
        => new() { Result = (ushort)(input.A ^ input.B) };
}