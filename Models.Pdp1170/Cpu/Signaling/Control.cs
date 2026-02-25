namespace Models.Pdp1170.Cpu.Signaling;

public class Control
{
    private readonly Decoder Decoder = new();
    private readonly Trapper Trapper = new();

    private Signal[] decoded = [];
    
    private byte timeState;

    public bool commit;

    private bool wait;
    public bool halt;

    public void Init()
    {
        decoded = Decoder.Fetch;
    }

    public void Restore()
    {
        decoded = Decoder.Fetch;
        timeState = 0;
        commit = false;
        wait = false;
        halt = false;
    }
    
    public Signal Emit()
    {
        return decoded[timeState];
    }

    public void Advance(ControlSignal signal)
    {
        if(signal.Stall) return;
        
        if (timeState != decoded.Length - 1 && !signal.Abort && !wait && !halt)
            timeState++;
        else
            Commit(signal);
    }

    private void Commit(ControlSignal signal)
    {
        switch (decoded[timeState].State)
        {
            case State.FETCH: Fetch(); break;
            case State.DECODE: decoded = Decoder.Decode(signal.Opcode); break;
            case State.WAIT: wait = true; decoded = Decoder.Nop; break;
            case State.HALT: halt = true; decoded = Decoder.Nop; break;
            default: Trapper.Execute(decoded[timeState].State); Fetch(); break;
        }
        timeState = 0;
    }

    private void Fetch()
    {
        decoded = Decoder.Fetch; 
        commit = true;
    }

    public void Clear()
    {
        commit = false;
    }
}