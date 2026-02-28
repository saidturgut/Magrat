namespace Models.Pdp1170.Cpu.Signaling;

public class Control
{
    private readonly Decoder Decoder = new();
    public readonly Trapper Trapper = new();

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

        bool permit = !signal.Skip && !Trapper.abort && !wait && !halt;
        
        if (timeState != decoded.Length - 1 && permit)
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
            default: Trapper.Execute(decoded[timeState]); Fetch(); break;
        }
        timeState = 0;
    }

    public TrapInfo Resolve()
    {
        TrapInfo info = Trapper.Arbitrate();
        if (info.Trap is not Trap.NONE)
        {
            decoded = Decoder.Trap;
            return info;
        }
        return new TrapInfo();
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