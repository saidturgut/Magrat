namespace Kernel;
using Devices;

public partial class Datapath
{
    private byte flagLatch;
    
    private void AluCompute()
    {
        var flagReg = PointK(PointerK.FR).Get();
        var flagRegister = new Flags(flagReg);
        
        var output = Alu.Compute(new AluInput
        {
            A = Point(signal.First).Get(),
            B = Point(signal.Second).Get(),
            C = (byte)(flagRegister.Bit0 ? 1 : 0),
            FL = flagLatch,
            FR = flagRegister,
        }, signal.Operation);

        flagLatch = output.Flags;
        
        PointK(PointerK.TMP).Set(output.Result);
        PointK(PointerK.FR).Set((byte)((flagReg & (byte)~signal.Mask) | (flagLatch & signal.Mask)));
    }
}