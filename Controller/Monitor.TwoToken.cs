namespace Controller;
using Models.x8Bit.Devices;

public partial class Monitor
{
    private void Set(string name)
    {
        Cpu = x8Bit.CpuTable(name, this);
        Cpu!.Init();
        Bus = Cpu.Bus();
        //cpuName = Cpu is not null ? "@" + Cpu!.Init() : "";
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