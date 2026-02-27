namespace Models.Pdp1170.Cpu.Executing;
using Addressing;

public partial class Datapath
{
    private readonly Mmu Mmu = new();
    private readonly Biu Biu = new();
    
    private uint addressLatch;
    
    private bool MemoryReady(Space space, bool read)
    {
        if (!stall) addressLatch = Mmu.Translate(Point(signal.First).Get(), space);
        
        bool granted = Biu.Ready(addressLatch);
        stall = !granted;
        return granted;
    }
    
    private void MemoryFetch()
    {
        if(!MemoryReady(Space.Instruction, true)) return;
        Point(Pointer.MDR).Set(Biu.Read(addressLatch, signal.Width));
    }
    
    private void MemoryRead()
    {
        if(!MemoryReady(Space.Data, true)) return;
        Point(Pointer.MDR).Set(Biu.Read(addressLatch, signal.Width));
    }
    
    private void MemoryWrite()
    {
        if(!MemoryReady(Space.Data, false)) return;
        Biu.Write(addressLatch, Point(Pointer.MDR).Get(), signal.Width);
    }
}