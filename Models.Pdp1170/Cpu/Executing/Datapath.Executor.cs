namespace Models.Pdp1170.Cpu.Executing;
using Translating;
using Computing;
using Bus;

public partial class Datapath
{
    private readonly Mmu Mmu = new Mmu();

    private readonly Alu Alu = new Alu();
    private readonly Psw Psw = new Psw();

    private uint addressLatch;
    
    private const byte priority = 0;

    private void RegisterMove()
        => Point(signal.Second).Set(Point(signal.First).Get());

    private void MemoryRead(Unibus unibus, Space space)
    {
        if(!MemoryProtocol(unibus, space)) return;
        switch (signal.Width)
        {
            case Width.WORD: Point(Pointer.MDR).Set(unibus.ReadWord(addressLatch)); break;
            case Width.BYTE: Point(Pointer.MDR).Set(unibus.ReadByte(addressLatch)); break;
        }
    }

    private void MemoryWrite(Unibus unibus)
    {
        if(!MemoryProtocol(unibus, Space.Data)) return;
        switch (signal.Width)
        {
            case Width.WORD: unibus.WriteWord(addressLatch, Point(Pointer.MDR).Get()); break;
            case Width.BYTE: unibus.WriteByte(addressLatch, (byte)Point(Pointer.MDR).Get()); break;
        }
    }

    private bool MemoryProtocol(Unibus unibus, Space space)
    {
        if(!stall) addressLatch = Mmu.Translate(Point(signal.First).Get(), space);

        bool granted = unibus.Request(priority);
        stall = !granted;
        return granted;
    }

    private void AluCompute()
    {
        ushort psw = Point(Pointer.PSW).Get();
        AluOutput output =
            Alu.Compute(
                new AluInput(Point(signal.First).Get(), Point(signal.Second).Get(), psw, signal.Width is Width.BYTE),
                signal.Operation);
        Point(Pointer.TMP).Set(output.Result);
        Point(Pointer.PSW).Set((ushort)((psw & (ushort)~signal.Mask) | (output.Flags & (ushort)signal.Mask)));
    }

    private void CondCompute()
        => abort = !Psw.CheckCondition(signal.Condition, new Flags(Point(Pointer.TMP).Get()));
}