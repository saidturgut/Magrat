namespace Models.Pdp1170.Cpu.Executing;
using Addressing;

public partial class Datapath
{
    private readonly Mmu Mmu = new();
    private readonly Biu Biu = new();
    
    private uint addressLatch;
    
    private MemoryOperation ResolveAddress(Space space)
    {
        if (!stall)
        {
            // CHECK IF IT IS ODD ADDRESS
            ushort virtualAddress = Point(signal.First).Get();
            if (signal.Width is Width.WORD && virtualAddress % 2 != 0)
            {
                Trapper.RequestAbortTrap(Trap.BUS_ABORT);
                return MemoryOperation.FAULT;
            }
            
            // CHECK IF VIRTUAL ADDRESS IS VALID
            MmuOutput translated = Mmu.Translate(virtualAddress, space);
            switch (translated.Trap)
            {
                case Trap.MMU_ABORT: Trapper.RequestAbortTrap(Trap.MMU_ABORT); return MemoryOperation.FAULT;
                case Trap.PDR_ERROR: Trapper.RequestPostTrap(Trap.PDR_ERROR); break;
            }
            addressLatch = translated.Address;

            // CHECK IF PHYSICAL ADDRESS IS VALID
            if (!Biu.Validate(addressLatch))
            {
                Trapper.RequestAbortTrap(Trap.BUS_ABORT);
                return MemoryOperation.FAULT;
            }
            
            // CHECK IF PHYSICAL ADDRESS ON CPU/MMU IO PAGE
            if (Biu.CheckIoPage(addressLatch))
            {
                stall = false;
                return MemoryOperation.REGISTER;
            }
        }
        
        bool granted = Biu.Ready();
        stall = !granted;
        return granted ? MemoryOperation.UNIBUS : MemoryOperation.FAULT;
    }
    
    private void MemoryFetch()
    {
        switch (ResolveAddress(Space.Instruction))
        {
            case MemoryOperation.UNIBUS: Point(Pointer.MDR).Set(Biu.Read(addressLatch, signal.Width)); break;
            case MemoryOperation.REGISTER: Point(Pointer.MDR).Set(ReadRegister()); break;
        }
    }
    
    private void MemoryRead()
    {
        switch (ResolveAddress(Space.Data))
        {
            case MemoryOperation.UNIBUS: Point(Pointer.MDR).Set(Biu.Read(addressLatch, signal.Width)); break;
            case MemoryOperation.REGISTER: Point(Pointer.MDR).Set(ReadRegister()); break;
        }
    }
    
    private void MemoryWrite()
    {
        switch (ResolveAddress(Space.Data))
        {
            case MemoryOperation.UNIBUS: Biu.Write(addressLatch, Point(Pointer.MDR).Get(), signal.Width); break;
            case MemoryOperation.REGISTER: WriteRegister(Point(Pointer.MDR).Get()); break;
        }
    }
}

public enum MemoryOperation
{
    FAULT, UNIBUS, REGISTER
}