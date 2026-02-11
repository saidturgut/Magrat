namespace Bounds;

public interface ICpu
{
    public string Init();
    
    public void Tick();
    
    public bool Halt();
}
