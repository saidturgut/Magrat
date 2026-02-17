namespace Devices;
using External;
using Bounds;

public partial class Bus(IEngine Engine) : IBus
{
    private readonly Ram Ram = new();
    private readonly Ioc Ioc = new();

    private List<string> logs = [];
    
    public byte Read(uint address, IBus bus)
    {
        address &= 0xFFFFF;
        
        if (address is >= Ioc.start and <= Ioc.end)
        {
            return Ioc.Read(address, bus);
        }

        var output = Ram.Read(address);
        AddReadLog("RAM", Log.Hex(address), output);
        return output;
    }
    
    public void Write(uint address, byte data, IBus bus)
    {
        address &= 0xFFFFF;

        if (address is >= Ioc.start and <= Ioc.end)
        {
            Ioc.Write(address, data, bus); 
            return;
        }
        
        AddWriteLog("RAM", Log.Hex(address), data);
        Ram.Write(address, data); 
    }

    public void Load(uint address, byte data)
    {
        address &= 0xFFFFF;
        Ram.Write(address, data);
    }
    
    public void Print(char character)
        => Engine.Print(character);

    public bool Poll()
    {
        Ioc.Terminal.KeyInput(Engine.Poll());
        Ioc.Timer.Advance();
        return Ioc.Timer.Sample();
    }
    
    public void Debug(List<string> input)
    {
        input.Add("MEMORY:");
        input.AddRange(logs);
        input.Add("---------------------");
        Engine.Debug(input.ToArray());
        logs = [];
    }
    
    public void AddReadLog(string device, string address, byte data)
        => logs.Add($"{device}[{address}]: READ \"{Log.Hex(data)}\"");
    public void AddWriteLog(string device, string address, byte data)
        => logs.Add($"{device}[{address}]: WRITE \"{Log.Hex(data)}\"");
}

public partial class Bus : ISudo
{
    public void Insert(byte[] image)
        => Ioc.Disk.Insert(image);

    public void Dump(uint start, uint end)
        => Ram.Dump(start, end);
}