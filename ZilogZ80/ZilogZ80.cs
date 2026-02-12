namespace ZilogZ80;
using Executing;
using Signaling;
using Bounds;

public class ZilogZ80(IBus Bus) : ICpu
{
    private readonly Datapath Datapath = new();
    private readonly Control Control = new();

    private bool halt;
    
    public string Init()
    {
        Datapath.Init();
        Control.Init();
        return "z80";
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