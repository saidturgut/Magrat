namespace Bounds;

public interface IEngine
{
    public void Print(char character);
    
    public byte Poll();

    public void Debug(string[] logs);
}