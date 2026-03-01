namespace Models.Pdp1170.Cpu.Executing;
using Addressing;
using Computing;
using Bus;

public partial class Datapath
{
    private readonly Alu Alu = new Alu();
    private readonly Psw Psw = new Psw();
    
    private void RegisterMove()
        => Point(signal.Second).Set(Point(signal.First).Get());
    
    private void AluCompute()
    {
        ushort psw = PointCore(Pointer.PSW).Get();
        var output = Alu.Compute(
            new AluInput(Point(signal.First).Get(), Point(signal.Second).Get(), Point(signal.Third).Get(), 
                psw, signal.Width is Width.BYTE), signal.Operation);
        PointLatch(Pointer.TMP).Set(output.Result);
        PointCore(Pointer.PSW).Set((ushort)((psw & (ushort)~signal.Mask) | (output.Flags & (ushort)signal.Mask)));
    }

    private void CondCompute()
        => skip = !Psw.CheckCondition(signal.Condition, new Flags(PointLatch(Pointer.TMP).Get()));
}