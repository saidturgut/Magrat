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
    private ushort flagLatch;
    
    private const byte priority = 0;

    private void RegisterWrite()
    {
        switch (signal.Width)
        {
            case Width.WORD: Point(signal.Second).Set(Point(signal.First).Get()); break;
            case Width.BYTE: Point(signal.Second).Set((ushort)((Point(signal.Second).Get() & 0xFF00) | (Point(signal.First).Get() & 0xFF))); break;
        }
    }

    private void MemoryRead(Unibus unibus)
    {
        if(!MemoryProtocol(unibus)) return;
        switch (signal.Width)
        {
            case Width.WORD: Point(Pointer.MDR).Set(unibus.ReadWord(addressLatch)); break;
            case Width.BYTE: Point(Pointer.MDR).Set(unibus.ReadByte(addressLatch)); break;
        }
    }

    private void MemoryWrite(Unibus unibus)
    {
        if(!MemoryProtocol(unibus)) return;
        switch (signal.Width)
        {
            case Width.WORD: unibus.WriteWord(addressLatch, Point(Pointer.MDR).Get()); break;
            case Width.BYTE: unibus.WriteByte(addressLatch, (byte)Point(Pointer.MDR).Get()); break;
        }
    }

    private bool MemoryProtocol(Unibus unibus)
    {
        if(!stall) addressLatch = Mmu.Translate(Point(signal.First).Get());

        bool granted = unibus.Request(priority);
        stall = !granted;
        return granted;
    }

    private void AluCompute()
    {
        ushort psw = Point(Pointer.PSW).Get();
        Flags flags = new Flags(psw);
        AluOutput output = Alu.Compute(new AluInput(
                Point(signal.First).Get(),
                Point(signal.Second).Get(),
                signal.Width is Width.BYTE),
            signal.Operation);

        flagLatch = output.Flags;
        
        Point(Pointer.TMP).Set(output.Result);
        Point(Pointer.PSW).Set((byte)((psw & (byte)~signal.Mask) | (flagLatch & (ushort)signal.Mask)));
    }
}