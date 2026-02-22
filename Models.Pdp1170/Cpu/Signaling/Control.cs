namespace Models.Pdp1170.Cpu.Signaling;

public class Control
{
    private readonly Decoder Decoder = new();

    private Signal[] decoded = [];
    
    private byte timeState;

    public bool commit;

    public bool halt;
    
    public void Init() {}

    public Signal Emit()
        => decoded[timeState];

    public void Advance(ControlSignal signal)
    {
        if(signal.Stall) return;
        
        if (timeState != decoded.Length - 1 && !halt)
            timeState++;
        else
            Commit(signal);
    }

    private void Commit(ControlSignal signal)
    {
        switch (decoded[timeState].State)
        {
            case State.FETCH: decoded = Decoder.Fetch; commit = true; break;
            case State.DECODE: decoded = Decoder.Decode(signal.Opcode); break;
            case State.HALT: halt = true; decoded = [new Signal()]; break;
        }
        timeState = 0;
    }
}