namespace Models.x8Bit.Engine.Units;

public interface IBus
{
    public byte Read(uint address, IBus bus);

    public void Write(uint address, byte data, IBus bus);
    
    public void Print(char character);
    
    public bool Poll();

    public void Debug(List<string> input);

    public void AddReadLog(string device, string address, byte data);
    public void AddWriteLog(string device, string address, byte data);
}
