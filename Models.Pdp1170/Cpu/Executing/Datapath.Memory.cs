namespace Models.Pdp1170.Cpu.Executing;
using Addressing;

public partial class Datapath
{
    private readonly Mmu Mmu = new();
    private readonly Biu Biu = new();
    
    private uint addressLatch;
    
    private bool MemoryReady(Space space)
    {
        if (!stall)
        {
            addressLatch = Mmu.Translate(Point(signal.First).Get(), space);
            Biu.Restore(addressLatch);
        }
        
        bool granted = Biu.Ready();
        stall = !granted;
        return granted;
    }
    
    private void MemoryFetch()
    {
        if(!MemoryReady(Space.Instruction)) return;
        Point(Pointer.MDR).Set(Biu.Read(addressLatch, signal.Width));
    }
    
    private void MemoryRead()
    {
        if(!MemoryReady(Space.Data)) return;
        Point(Pointer.MDR).Set(Biu.Read(addressLatch, signal.Width));
    }
    
    private void MemoryWrite()
    {
        if(!MemoryReady(Space.Data)) return;
        Biu.Write(addressLatch, Point(Pointer.MDR).Get(), signal.Width);
    }
}