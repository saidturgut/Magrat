namespace Models.x8Bit.Engine.Units;

public interface IInterrupt
{
    public void Receive(bool signal);
    
    public bool Check();

    public void Execute(byte state);
}