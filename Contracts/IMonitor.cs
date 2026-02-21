namespace Contracts;

public interface IMonitor
{
    public void Print(char character);
    
    public byte Poll();

    public void Debug(string[] logs);
}