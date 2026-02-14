namespace ZilogZ80.Executing;
using Computing;
using Signaling;

public partial class Datapath
{
    private readonly Alu Alu = new ();
    private readonly Fru Fru = new ();
    
    private byte flagLatch;
    
    private void AluCompute()
    {
        var flagReg = Point(Pointer.F).Get();
        var flagRegister = Fru.Flags(flagReg);
        
        var output = Alu.Compute(new AluInput
        {
            A = Point(signal.First).Get(),
            B = Point(signal.Second).Get(),
            C = (byte)(flagRegister.Carry ? 1 : 0),
            FL = flagLatch,
            FR = flagRegister,
        }, signal.Operation);

        flagLatch = output.Flags;
        
        Point(Pointer.TMP).Set(output.Result);
        Point(Pointer.F).Set((byte)((flagReg & (byte)~signal.Mask) | (flagLatch & (byte)signal.Mask)));
    }
}