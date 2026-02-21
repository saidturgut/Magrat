namespace Controller;

public partial class Monitor
{
    private void Help()
        => Host.Print("HELP PLACEHOLDER");
    private void Exit() 
        => exit = true;
    private void Clear()
        => Host.Clear();

    private void Debug()
        => debug = !debug;
    
    private void Run()
    {
        while (!Cpu!.Halt() || !debug)
        {
            Cpu.Tick();

            if (sleep != 0)
            {
                Thread.Sleep(sleep);
            }
        }
        Reset();
    }
}