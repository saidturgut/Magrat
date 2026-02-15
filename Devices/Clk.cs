namespace Devices;

public class Tmr
{
    private byte enable;
    private ushort counter;
    private ushort reload;
    private byte interrupt;

    public void Advance()
    {
        if (enable == 0) return;
        if (counter == 0) return;

        counter--;

        if (counter != 0) return;
        
        interrupt = 0xFF;

        if (reload != 0) counter = reload;
    }

    public bool Sample()
        => interrupt != 0;

    public byte ReadStatus()
        => interrupt;
    public void WriteStatus(byte data)
        => interrupt = data != 0 ? (byte)0xFF : (byte)0x00;

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