namespace Controller;

public partial class Monitor
{
    private void Set(string name)
    {
        Machine = Machines[name];
        Machine.Init();
    }
    
    private void Disk(string name)
    {
        Machine!.Insert(File.ReadAllBytes(name));
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
            Machine!.Clock();
        }
    }
}