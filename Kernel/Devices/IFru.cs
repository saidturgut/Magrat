namespace Kernel.Devices;

public interface IFru
{
    public bool Check(byte condition, Flags tmp);
}

public struct Flags(byte source)
{
    public bool Bit0 = (source & (1 << 0)) != 0;
    public bool Bit1 = (source & (1 << 1)) != 0;
    public bool Bit2 = (source & (1 << 2)) != 0;
    public bool Bit3 = (source & (1 << 3)) != 0;
    public bool Bit4 = (source & (1 << 4)) != 0;
    public bool Bit5 = (source & (1 << 5)) != 0;
    public bool Bit6 = (source & (1 << 6)) != 0;
    public bool Bit7 = (source & (1 << 7)) != 0;
}