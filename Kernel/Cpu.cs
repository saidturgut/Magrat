namespace Kernel;
using Bounds;

public class Cpu(IBus Bus, Datapath Datapath, Control Control)
{
    public void Init()
    {
        Datapath.Init();
        Control.Init();
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

    public bool Halt() 
        => Control.halt;
}