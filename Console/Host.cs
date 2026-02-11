namespace Console;
using Engine;
using Bounds;

public class Host : IHost
{
    private Engine Engine = null!;
    
    public void Enter()
    {
        Engine = new Engine(this);
        
        while (!Engine.exit) Tick();
    }

    private void Tick()
    {
        // PRINT THE PROMPT
        // WAIT FOR COMMAND
        // PARSE THE TYPED INPUT
        // CALL COMMAND METHOD
        // PRINT THE RESULTS
    }
    
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