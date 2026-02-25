namespace Models.Pdp1170.Cpu;
using Executing;
using Signaling;
using Bus;

public class Kb11c
{
    private readonly Datapath Datapath = new();
    private readonly Control Control = new();
    
    public void Init()
    {
        Datapath.Init();
        Control.Init();
    }

    public void Restore()
    {
        Datapath.Restore();
        Control.Restore();
    }
    
    public void Tick(Unibus unibus)
    {
        Datapath.Receive(Control.Emit());
        
        Datapath.Execute(unibus);

        Control.Advance(Datapath.Emit());

        if (Control.commit) Commit();
    }

    private void Commit()
    {
        foreach (var log in Datapath.Debug())
        {
            Console.WriteLine(log);
        }
        Control.Clear();
    }

    public bool Halt() 
        => Control.halt;
}