namespace Models.x8Bit.Engine.Units;

public interface IAlu
{
    public AluOutput Compute(AluInput input, byte operation);
}

public struct AluInput
{
    public byte A;
    public byte B;
    public byte C;
    public byte FL;
    public Flags FR;
}

public struct AluOutput
{
    public byte Result;
    public byte Flags;
    public bool Custom;
}