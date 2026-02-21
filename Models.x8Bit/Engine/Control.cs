namespace Models.x8Bit.Engine;
using Units;

public class Control(IDecoder Decoder, IInterrupt Interrupt)
{
    private Signal[] decoded = [];

    private byte timeState;
    
    public bool commit;
    public bool halt;
    
    public void Init()
    {
        decoded = Decoder.Fetch();
    }

    public Signal Emit()
        => decoded[timeState];

    public void Advance(ControlSignal signal)
    {
        if (timeState != decoded.Length - 1 && !signal.Abort && !halt)
            timeState++;
        else
            Commit(signal);
    }

    private void Commit(ControlSignal signal)
    {
        if (Interrupt.Check())
        {
            decoded = Decoder.Interrupt();
            commit = true;
            timeState = 0;
            halt = false;
            return;
        }
        switch (decoded[timeState].State)
        {
            case 0: decoded = Decoder.Fetch(); Decoder.Clear(); commit = true; break;
            case 1: decoded = Decoder.Decode(signal.Opcode); break;
            case 2: halt = true; decoded = [new Signal()]; break;
            default:
            {
                if(!Decoder.Execute(decoded[timeState].State))
                    Interrupt.Execute(decoded[timeState].State);
                decoded = Decoder.Fetch(); 
                commit = true; 
                break;
            }
        }
        timeState = 0;
    }

    public void Receive(bool irq)
        => Interrupt.Receive(irq);

    public void Clear()
    {
        commit = false;
    }
}

public struct ControlSignal(byte opcode, bool abort)
{
    public readonly byte Opcode = opcode;
    public readonly bool Abort = abort;
}