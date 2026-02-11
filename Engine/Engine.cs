namespace Engine;
using Bounds;
using Devices;

public partial class Engine(IHost Host)
{
    private readonly Tables Tables = new();

    private ICpu? Cpu;
    private ISudo? Bus;
    
    public bool exit;
    
    public void Set(string name)
    {
        Bus = new Bus(this);
        Cpu = Tables.CpuTable(name, Bus);
        Cpu!.Init();
    }
    
    public void Mem(uint address, byte data)
    {
        Bus!.Write(address, data, Bus);
    }

    public void Load(uint address, byte[] image)
    {
        for (int i = 0; i < image.Length; i++)
        {
            Bus!.Write((uint)(address + i), image[i], Bus);
        }
    }

    public void Disk(byte[] image)
    {
        Bus!.Insert(image);
    }

    public void Reg(string register, ushort value)
    {
    }
    
    public void Run()
    {
        while (!Cpu!.Halt())
        {
            Cpu.Tick();
        }
    }

    public void Step()
    {
        Cpu!.Tick();
    }
}

public enum Cpus
{
    Mos6502, Intel8080, Motorola6809, ZilogZ80, Intel8086
}