namespace Models.x8Bit.Engine.Units;

public interface ILogger
{
    public List<string> Debug(Register[] registers, string debugName);
}