namespace Models.x8Bit.Intel8080.Signaling;
using Engine.Units;
using Decoding;

public class Interrupt : IInterrupt
{
    private bool ei;
    private bool enb;
    private bool irq;
    
    public void Receive(bool signal)
        => irq = signal;

    public void Execute(byte state)
    {
        switch ((State)state)
        {
            case State.INT_E: ei = true; break;
            case State.INT_D: enb = false; break;
        }
    }

    public bool Check()
    {
        if (ei) 
            enb = true;
        
        return enb && irq;
    }
}