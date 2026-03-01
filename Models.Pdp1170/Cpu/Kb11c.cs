namespace Models.Pdp1170.Cpu;
using Executing;
using Signaling;
using Bus;

public class Kb11c
{
    private readonly Datapath Datapath = new();
    private readonly Control Control = new();
    
    public void Init(Unibus unibus)
    {
        Datapath.Init(unibus, Control.Trapper);
        Control.Init();
    }

    public void Restore()
    {
        Datapath.Restore();
        Control.Restore();
    }
    
    public void Tick()
    {
        Datapath.Receive(Control.Emit());
        Datapath.Execute();
        Control.Advance(Datapath.Emit());

        if (Control.commit) Commit();
    }

    private void Commit()
    {
        Datapath.Commit(Control.Resolve());
        foreach (var log in Datapath.Debug())
            Console.WriteLine(log);
        Control.Clear();
    }

    public bool Halt() 
        => Control.halt;
}