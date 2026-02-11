namespace Devices;
using External;
using Bounds;

public class Ioc
{
    public readonly Tty Tty = new (); // TERMINAL
    public readonly Clk Clk = new (); // TIMER
    public readonly Dsk Dsk = new (); // DISK
    
    public const uint start = 0xDFF0;
    public const uint end = 0xDFFF;
    
    public byte Read(uint address, IBus bus)
    {
        byte index = (byte)(address & 0xF);
        byte output;
        switch (index)
        {
            case 0x0: output = Tty.ReadStatus(); bus.AddReadLog("TTY","STATUS",output); return output;
            case 0x1: output = Tty.ReadData(); bus.AddReadLog("TTY","DATA",output); return output;
            
            case 0xF: output = Dsk.ReadStatus(); bus.AddReadLog("DISK","STATUS",output); return output;
        }
        throw new Exception($"INVALID IO ADDRESS TO READ \"{Log.Hex(address)}\"");
    }

    public void Write(uint address, byte data, IBus bus)
    {
        byte index = (byte)(address & 0xF);
        switch (index)
        {
            case 0x2: Tty.WriteData(data, bus); bus.AddWriteLog("TTY", "DATA", data); return;
            
            case 0xA: Dsk.WriteBlockLow(data); bus.AddWriteLog("DISK", "BLOCK L", data); return;
            case 0xB: Dsk.WriteBlockHigh(data); bus.AddWriteLog("DISK", "BLOCK H", data); return;
            case 0xC: Dsk.WriteDmaLow(data); bus.AddWriteLog("DISK", "DMA L", data); return;
            case 0xD: Dsk.WriteDmaHigh(data); bus.AddWriteLog("DISK", "DMA H", data); return;
            case 0xE: Dsk.WriteCommand(data, bus); bus.AddWriteLog("DISK", "COMMAND", data); return;
        }
        throw new Exception($"INVALID IO ADDRESS TO WRITE \"{Log.Hex(address)}\"");
    }
}