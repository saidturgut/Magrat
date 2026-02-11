namespace Bounds;

public interface IHost
{
    public void WriteOutput(char character);
    
    public void WriteLog(string[] lines);

    public byte ReadInput();

    public void Clear();

    public void Print(string input);
}