namespace Console;
using External;
using Engine;
using Bounds;

public class Host : IHost
{
    private Engine Engine = null!;
    
    public void Enter()
    {
        Engine = new Engine(this);
        Engine.Init();
        
        Boot();
        
        while (!Engine.exit) Tick();
    }

    private void Boot()
    {
        Assembler.Run("z80");
        Execute("set z80");
        Execute("load 0 z80.bin");
        Execute("sleep 40");
        Execute("run");
    }
    /*private void Boot()
    {
        Assembler.Run("6502");
        Execute("set 6502");
        Execute("load 0 6502.bin");
        Execute("sleep 40");
        Execute("run");
    }*/


    private void Tick()
    {
        System.Console.Write($"magrat{Engine.cpuName}>");
        
        var input = System.Console.ReadLine();
        
        Execute(input);
    }

    private void Execute(string input)
    {
        string[] tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
        switch (tokens.Length)
        {
            case 1: if (Engine.OneToken!.TryGetValue(tokens[0], out var one)) one();
                else Fault(); break;
            case 2: if (Engine.TwoToken!.TryGetValue(tokens[0], out var two)) two(tokens[1]);
                else Fault(); break;
            case 3: if (Engine.ThreeToken!.TryGetValue(tokens[0], out var three)) three(tokens[1], tokens[2]);
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