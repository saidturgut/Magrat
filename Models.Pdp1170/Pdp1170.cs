namespace Models.Pdp1170;
using Contracts;
using Devices;
using Kernel;

public class Pdp1170 : IMachine
{
    private readonly Kb11c Kb11c = new();
    private readonly Membus Membus = new();
    private readonly Unibus Unibus = new();

    public void Init()
    {
        Unibus.Init();
        Kb11c.Init(Unibus);
    }
    
    public void Power(byte sleep)
    {
        Unibus.Restore();
        Kb11c.Restore();
        
        while (!Kb11c.Halt())
        {
            Clock();
            
            if (sleep != 0) Thread.Sleep(sleep);
        }
    }
    
    public void Clock()
    {
        Kb11c.Tick();
        Unibus.Tick();
        Membus.Tick();
    }

    public void Load(uint address, byte data) { Membus.Load(address, data); }

    public void Insert(byte[] image) { }

    public void Dump() { Membus.Dump(); }
}