namespace Models.Pdp1170.Bus;
using Cpu;

public class Unibus
{
    private readonly Ram Ram = new();
    
    public void Init() { }
    public void Restore() { }

    public void Load(uint address, byte data)
        => Ram.Write(address, data);

    public bool Request(byte priority)
    {
        return true;
    }
    
    public byte ReadByte(uint address)
    {
        return Ram.Read(address);
    }
    public ushort ReadWord(uint address)
    {
        return (ushort)(Ram.Read(address) | (Ram.Read(address + 1) << 8));
    }

    public void WriteByte(uint address, byte value)
    {
        Console.WriteLine($"MEMORY[{Tools.Octal(address)}]: {Tools.Octal(value)}");
        Ram.Write(address, value);
    }
    public void WriteWord(uint address, ushort value)
    {
        Console.WriteLine($"MEMORY[{Tools.Octal(address)}]: {Tools.Octal((byte)(value & 0xFF))}");
        Console.WriteLine($"MEMORY[{Tools.Octal(address + 1)}]: {Tools.Octal((byte)(value >> 8))}");
        Ram.Write(address, (byte)(value & 0xFF));
        Ram.Write(address + 1, (byte)(value >> 8));
    }

    public void Dump() => Ram.Dump();
}