namespace Models.Pdp1170;
using Bus;
using Cpu;

public class Pdp1170
{
    private readonly Unibus Unibus = new();
    private readonly Kb11c Kb11c = new();

    public void Init()
    {
        Unibus.Init();
        Kb11c.Init();
    }
    
    public void Clock(byte sleep)
    {
        while (!Kb11c.Halt())
        {
            Tick();
            
            if (sleep != 0) Thread.Sleep(sleep);
        }
    }
    
    private void Tick()
    {
        Kb11c.Tick(Unibus);
    }
}