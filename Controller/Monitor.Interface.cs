namespace Controller;
using Contracts;

public partial class Monitor : IMonitor
{
    public void Print(char character)
        => Host.WriteOutput(character);

    public byte Poll()
        => Host.ReadInput();

    public void Debug(string[] logs)
    {
        if (debug)
        {
            Host.WriteLog(logs);
        }
        step--;
    }
}