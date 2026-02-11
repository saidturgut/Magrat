namespace Engine;

public partial class Engine
{
    private void Help()
        => Host.Print("HELP PLACEHOLDER");
    private void Exit() 
        => exit = true;
    private void Clear()
        => Host.Clear();
    
    private void Run()
    {
        while (!Cpu!.Halt())
        {
            Cpu.Tick();

            if (sleep != 0)
            {
                Thread.Sleep(sleep);
            }
        }
    }
}