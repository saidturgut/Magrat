namespace Devices;
using External;
using Bounds;

public class Ioc
{
    public readonly Terminal Terminal = new (); // TERMINAL
    public readonly Timer Timer = new (); // TIMER
    public readonly Disk Disk = new (); // DISK
    
    public const uint start = 0xFF00;
    public const uint end = 0xFF0F;
    
    public byte Read(uint address, IBus bus)
    {
        byte index = (byte)(address & 0xF);
        byte output;
        switch (index)
        {
            case 0x0: output = Terminal.ReadStatus(); bus.AddReadLog("TERMINAL","STATUS",output); return output;
            case 0x1: output = Terminal.ReadData(); bus.AddReadLog("TERMINAL","DATA",output); return output;
            case 0x3: output = Timer.ReadStatus(); bus.AddReadLog("TIMER","STATUS",output); return output;
            case 0xF: output = Disk.ReadStatus(); bus.AddReadLog("DISK","STATUS",output); return output;
        }
        throw new Exception($"INVALID IO ADDRESS TO READ \"{Log.Hex(address)}\"");
    }

    public void Write(uint address, byte data, IBus bus)
    {
        byte index = (byte)(address & 0xF);
        switch (index)
        {
            case 0x2: Terminal.WriteData(data, bus); bus.AddWriteLog("TERMINAL", "DATA", data); return;
            case 0x4: Timer.WriteStatus(data); bus.AddWriteLog("TIMER", "STATUS", data); return;
            case 0x5: Timer.WriteCounterLow(data); bus.AddWriteLog("TIMER", "COUNTER L", data); return;
            case 0x6: Timer.WriteCounterHigh(data); bus.AddWriteLog("TIMER", "COUNTER H", data); return;
            case 0x7: Timer.WriteReloadLow(data); bus.AddWriteLog("TIMER", "RELOAD", data); return;
            case 0x8: Timer.WriteReloadHigh(data); bus.AddWriteLog("TIMER", "RELOAD", data); return;
            case 0x9: Timer.WriteEnable(data); bus.AddWriteLog("TIMER", "ENABLE", data); return;
            case 0xA: Disk.WriteBlockLow(data); bus.AddWriteLog("DISK", "BLOCK L", data); return;
            case 0xB: Disk.WriteBlockHigh(data); bus.AddWriteLog("DISK", "BLOCK H", data); return;
            case 0xC: Disk.WriteDmaLow(data); bus.AddWriteLog("DISK", "DMA L", data); return;
            case 0xD: Disk.WriteDmaHigh(data); bus.AddWriteLog("DISK", "DMA H", data); return;
            case 0xE: Disk.WriteCommand(data, bus); bus.AddWriteLog("DISK", "COMMAND", data); return;
        }
        throw new Exception($"INVALID IO ADDRESS TO WRITE  ADDR:\"{Log.Hex(address)}\"  DATA:\"{Log.Hex(data)}\"");
    }
}