namespace Console;
using Engine;
using Bounds;

public class Host : IHost
{
    private Engine Engine = null!;
    
    public void Enter()
    {
        Engine = new Engine(this);
        Engine.Init();
        
        while (!Engine.exit) Tick();
    }

    private void Tick()
    {
        System.Console.Write($"magrat{Engine.cpuName}>");
        
        var input = System.Console.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
        switch (input?.Length)
        {
            case 1: if (Engine.OneToken!.TryGetValue(input[0], out var one)) one();
                else Fault(); break;
            case 2: if (Engine.TwoToken!.TryGetValue(input[0], out var two)) two(input[1]);
                else Fault(); break;
            case 3: if (Engine.ThreeToken!.TryGetValue(input[0], out var three)) three(input[1], input[2]);
                else Fault(); break;
            default: Fault(); break;
        }
    }
    
    public void Print(string input)
        => System.Console.WriteLine(input);
    
    private void Fault()
        => Print("FAULT");
    
    public void Clear() 
        => System.Console.Clear();
    
    public void WriteOutput(char character)
        => System.Console.Write(character);

    public byte ReadInput()
        => (byte)(System.Console.KeyAvailable
            ? System.Console.ReadKey(intercept: true).KeyChar : 0);
    
    public void WriteLog(string[] lines)
    {
        foreach (string line in lines)
            System.Console.WriteLine(line);
    }
}