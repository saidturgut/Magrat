namespace Engine;
using Devices;

public partial class Engine
{
    private void Set(string name)
    {
        Bus = new Bus(this);
        Cpu = Tables.CpuTable(name, Bus);
        cpuName = Cpu is not null ? "@" + Cpu!.Init() : "";
    }
    
    private void Disk(string name)
    {
        Bus!.Insert(File.ReadAllBytes(name));
    }

    private void Sleep(string dat)
    {
        sleep = Convert.ToByte(dat);
    }
    
    private void Step(string dat)
    {
        step = Convert.ToByte(dat);
        while (step > 0)
        {
            Cpu!.Tick();
        }
    }
}