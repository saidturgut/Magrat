namespace Devices;

public class Timer
{
    private byte enable;
    private ushort counter;
    private ushort reload;
    private byte interrupt = 0xFF;

    public void Advance()
    {
        if (enable == 0) return;
        if (counter == 0) return;

        counter--;

        if (counter != 0) return;
        
        interrupt = 0x00;

        if (reload != 0) counter = reload;
    }

    public bool Emit()
        => interrupt == 0;

    public byte ReadStatus()
        => interrupt;
    public void WriteStatus(byte data)
        => interrupt = data != 0 ? (byte)0x00 : (byte)0xFF;

    public void WriteCounterLow(byte data)
        => counter = (ushort)((counter & 0xFF00) | data);
    public void WriteCounterHigh(byte data)
        => counter = (ushort)((counter & 0x00FF) | (data << 8));
    public void WriteReloadLow(byte data)
        => reload = (ushort)((reload & 0xFF00) | data);
    public void WriteReloadHigh(byte data)
        => reload = (ushort)((reload & 0x00FF) | (data << 8));
    
    public void WriteEnable(byte data)
        => enable = data;
}