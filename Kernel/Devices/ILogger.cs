namespace Kernel.Devices;

public interface ILogger
{
    public List<string> Debug(Register[] registers, string debugName);
}