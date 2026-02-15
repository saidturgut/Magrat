namespace ZilogZ80.Signaling;
using Executing;

public class Control
{
    private readonly Decoder Decoder = new();
    private readonly Interrupt Interrupt = new();
    private Signal[] decoded = [];

    private byte timeState;
    
    public bool commit;
    public bool halt;
    
    public void Init()
    {
        decoded = Decoder.Fetch;
    }

    public Signal Emit()
        => decoded[timeState];

    public void Advance(ControlSignal signal)
    {
        if (timeState != decoded.Length - 1 && !signal.Abort)
            timeState++;
        else
            Commit(signal);
    }

    private void Commit(ControlSignal signal)
    {
        if (Interrupt.Check())
        {
            decoded = Decoder.Int1;
            commit = true;
            timeState = 0;
            return;
        }
        switch (decoded[timeState].State)
        {
            case State.FETCH: decoded = Decoder.Fetch; commit = true; break;
            case State.DECODE: decoded = Decoder.Decode(signal.Opcode); break;
            case State.HALT: halt = true; break;
            default: Interrupt.Execute(decoded[timeState].State); break;
        }
        timeState = 0;
    }

    public void Receive(bool irq)
        => Interrupt.irq = irq;

    public void Clear()
    {
        Decoder.Clear();
        commit = false;
    }
}

public enum State
{
    FETCH, DECODE, HALT, 
    INT_E, INT_D, INT_0, INT_1, INT_2, RET_N,
}