namespace Models.x8Bit.Engine;
using Contracts;
using Units;

public class Cpu(IBus Bus, Datapath Datapath, Control Control) : IMachine
{
    public void Init()
    {
        Datapath.Init();
        Control.Init();
    }

    public void Power(byte sleep)
    {
        while (!Control.halt)
        {
            Tick();
            
            if (sleep != 0) Thread.Sleep(sleep);
        }
    }
    
    public void Tick()
    {
        Datapath.Receive(Control.Emit());
        
        Datapath.Execute(Bus);

        Control.Advance(Datapath.Emit());

        if (Control.commit) Commit();
    }

    private void Commit()
    {
        Datapath.Debug();
        Control.Receive(Bus.Poll());
        Bus.Debug(Datapath.logs);
        Datapath.Clear();
        Control.Clear();
    }
    
    public void Load(uint address, byte data)
    {
    }

    public void Insert(byte[] image)
    {
    }
    
    public void Dump()
    {
    }
}