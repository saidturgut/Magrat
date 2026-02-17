namespace Devices;
using Bounds;

public class Terminal
{
    private readonly Queue<byte> inputBuffer = new();

    public void KeyInput(byte data)
    {
        if(data != 0) inputBuffer.Enqueue(data);
    }

    public byte ReadStatus()
    {        
        //Environment.Exit(9);
        return (byte)(inputBuffer.Count != 0 ? 0x00 : 0xFF);
    }

    public byte ReadData()
    {
        //Environment.Exit(8);
        return Normalize(inputBuffer.Dequeue());
    }

    public void WriteData(byte data, IBus bus)
        => bus.Print((char)data);

    private byte Normalize(byte data)
    {
        if (data == 0xA) data = 0xD;
        return data;
    }
}