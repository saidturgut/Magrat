namespace Models.Pdp1170.Cpu.Signaling;

public class Trapper
{
    public void Execute(State state)
    {
        
    }

    public void Request()
    {
        
    }
}

public struct Trap
{
    public string Name;
    public ushort Vector;
    public bool Abort;
}