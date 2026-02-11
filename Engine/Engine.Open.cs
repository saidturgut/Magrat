namespace Engine;
using Bounds;

public partial class Engine : IEngine
{
    public void Print(char character)
        => Host.WriteOutput(character);

    public byte Poll()
        => Host.ReadInput();

    public void Debug(string[] logs)
        => Host.WriteLog(logs);
}