namespace Intel8080.Computing;
using Kernel.Devices;
using Decoding;

public class Alu : ZilogZ80.Computing.Alu, IAlu
{
    public AluOutput Compute(AluInput input, byte operation)
    {
        AluOutput output = Operations[operation](input);
        
        output.Flags &= unchecked((byte)~((1 << 5) | (1 << 3) | (1 << 2)));
        
        output.Flags |= (byte)Flag.BIT1;
        if (EvenParity(output.Result)) output.Flags |= (byte)Flag.OVER;
        if ((output.Result & 0x80) != 0) output.Flags |= (byte)Flag.SIGN;
        if (output.Result == 0) output.Flags |= (byte)Flag.ZERO;

        return output;
    }
}
