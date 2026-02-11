namespace Devices;
using Bounds;

public class Tty
{
    private readonly Queue<byte> inputBuffer = new();

    public void KeyInput(byte data)
        => inputBuffer.Enqueue(data);
    
    public byte ReadStatus()
        => (byte)(inputBuffer.Count != 0 ? 0xFF : 0x00);

    public byte ReadData()
        => Normalize(inputBuffer.Dequeue());

    public void WriteData(byte data, IBus bus)
        => bus.Print((char)data);

    private byte Normalize(byte data)
    {
        if (data == 0xA) data = 0xD;
        return data;
    }
}