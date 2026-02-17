namespace Intel8080.Signaling;
using Kernel.Devices;

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
        return false;
    }
}