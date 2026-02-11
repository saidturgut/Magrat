namespace Mos6502;
using Bounds;
using Signaling;
using Executing;

public class Mos6502(IBus Bus) : ICpu
{
    private readonly Datapath Datapath = new();
    private readonly Control Control = new();

    private bool halt;
    
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
        halt = Control.halt;
        Datapath.Debug();
        Bus.Poll();
        Bus.Debug(Datapath.logs);
        Datapath.Clear();
        Control.Clear();
    }

    public bool Halt() 
        => halt;
}