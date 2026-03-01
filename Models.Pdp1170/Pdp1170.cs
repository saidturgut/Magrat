namespace Models.Pdp1170;
using Contracts;
using Bus;
using Cpu;

public class Pdp1170 : IMachine
{
    private readonly Unibus Unibus = new();
    private readonly Kb11c Kb11c = new();

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
            Tick();
            
            if (sleep != 0) Thread.Sleep(sleep);
        }
    }
    
    public void Tick()
    {
        Kb11c.Tick();
        
        Unibus.Arbitrate();
    }

    public void Load(uint address, byte data) { Unibus.Load(address, data); }

    public void Insert(byte[] image) { }

    public void Dump() { Unibus.Dump(); }
}