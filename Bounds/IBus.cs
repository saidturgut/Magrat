namespace Bounds;

public interface IBus
{
    public byte Read(uint address, IBus bus);

    public void Write(uint address, byte data, IBus bus);
    
    public void Print(char character);
    
    public void Poll();

    public void Debug(List<string> input);

    public void AddReadLog(string device, string address, byte data);
    public void AddWriteLog(string device, string address, byte data);
}

public interface ISudo : IBus
{
    public void Insert(byte[] image);
}
