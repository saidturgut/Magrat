namespace Contracts;

public interface ICpu
{
    public void Init();
    
    public void Tick();
    
    public bool Halt();

    public ISudo Bus();
}
