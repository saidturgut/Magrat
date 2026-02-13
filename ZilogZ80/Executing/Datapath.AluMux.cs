namespace ZilogZ80.Executing;
using Computing;
using Signaling;

public partial class Datapath
{
    private readonly Alu Alu = new ();
    private readonly Fru Fru = new ();
    
    private void AluCompute()
    {
        AluOutput output = Alu.Compute(new AluInput
        {
            A = Point(signal.First).Get(),
            B = Point(signal.Second).Get(),
            C = (byte)(Fru.Flags.Carry ? 1 : 0),
            F = Fru.Flags,
        }, signal.Operation);
        
        Point(Pointer.TMP).Set(output.Result);
        Point(Pointer.F).Set((byte)
            ((Point(Pointer.F).Get() & (byte)~signal.Mask) | (output.Flags & (byte)signal.Mask)));
    }
}