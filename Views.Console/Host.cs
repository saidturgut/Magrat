namespace Views.Console;
using Controller;
using Contracts;

public class Host : IHost
{
    private Monitor Monitor = null!;

    public void Enter()
    {
        Monitor = new Monitor(this);
        Monitor.Init();

        Boot();

        while (!Monitor.exit) Tick();
    }

    private void Boot()
    {
        //Assembler.Run("zz80");
        Execute("set 6502");
        Execute("load 0 6502.bin");
        /*Execute("load 128 boot.bin");
        Execute("load 48128 bios.bin");
        Execute("disk cpm.DSK");*/
        //Execute("debug");
        //Execute("sleep 25");
        //Execute("dump 0 1000");
        Execute("run");
    }

    private void Tick()
    {
        System.Console.Write($"magrat{Monitor.cpuName}>");

        var input = System.Console.ReadLine();

        Execute(input);
    }

    private void Execute(string input)
    {
        string[] tokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        switch (tokens.Length)
        {
            case 1:
                if (Monitor.OneToken!.TryGetValue(tokens[0], out var one)) one();
                else Fault();
                break;
            case 2:
                if (Monitor.TwoToken!.TryGetValue(tokens[0], out var two)) two(tokens[1]);
                else Fault();
                break;
            case 3:
                if (Monitor.ThreeToken!.TryGetValue(tokens[0], out var three)) three(tokens[1], tokens[2]);
                else Fault();
                break;
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
            ? System.Console.ReadKey(intercept: true).KeyChar
            : 0);

    public void WriteLog(string[] lines)
    {
        foreach (string line in lines)
            System.Console.WriteLine(line);
    }
}