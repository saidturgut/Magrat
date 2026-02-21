namespace Models.x8Bit.Mos6502.Signaling;
using Engine.Units;

public class Interrupt : IInterrupt
{
    private bool irq;
    
    public void Receive(bool signal)
        => irq = signal;

    public void Execute(byte state)
    {
    }

    public bool Check()
    {
        return irq;
    }
}