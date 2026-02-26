namespace Models.Pdp1170.Cpu.Executing;
using Addressing;

public partial class Datapath
{
    private readonly Mmu Mmu = new();
    private readonly Biu Biu = new();
    
    private uint addressLatch;
    private const byte priority = 0;
    
    private bool RequestMemory(Space space)
    {
        if (!stall) addressLatch = Mmu.Translate(Point(signal.First).Get(), space);
        
        bool granted = Biu.Request(addressLatch, priority);
        stall = !granted;
        return granted;
    }
    
    private void MemoryRead(Space space)
    {
        if(!RequestMemory(space)) return;
        switch (signal.Width)
        {
            case Width.WORD: Point(Pointer.MDR).Set(Biu.ReadWord(addressLatch)); break;
            case Width.BYTE: Point(Pointer.MDR).Set(Biu.ReadByte(addressLatch)); break;
        }
    }
    
    private void MemoryWrite(Space space)
    {
        if(!RequestMemory(space)) return;
        switch (signal.Width)
        {
            case Width.WORD: Biu.WriteWord(addressLatch, Point(Pointer.MDR).Get()); break;
            case Width.BYTE: Biu.WriteByte(addressLatch, (byte)Point(Pointer.MDR).Get()); break;
        }
    }
}