namespace Kernel.Devices;

public interface IFru
{
    public Flags Flags(byte source);
    public bool Check(byte condition, Flags tmp);
}

public struct Flags
{
    public bool Bit0;
    public bool Bit1;
    public bool Bit2;
    public bool Bit3;
    public bool Bit4;
    public bool Bit5;
    public bool Bit6;
    public bool Bit7;
}