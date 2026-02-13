namespace Mos6502;
using Bounds;
using Signaling;
using Executing;

public class Mos6502(IBus Bus) : ICpu
{
    private readonly Datapath Datapath = new();
    private readonly Control Control = new();
    
    public string Init()
    {
        Datapath.Init();
        Control.Init();
        return "6502";
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
        Bus.Poll();
        Bus.Debug(Datapath.logs);
        Datapath.Clear();
        Control.Clear();
    }

    public bool Halt() 
        => Control.halt;
}