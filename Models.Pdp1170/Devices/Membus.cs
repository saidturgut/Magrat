namespace Models.Pdp1170.Devices;

public class Membus() : Bus(3)
{
    private readonly Ram Ram = new();
    
    public static bool Validate(uint address)
        => address <= 0x10000;

    public static bool CheckIoPage(uint address)
        => address is < 0x7FE000 or > 0x7FFFFF;

    public void Load(uint address, byte data)
        => Ram.Write(address, data);
    
    public ushort Read(uint address)
    {
        return (ushort)(Ram.Read(address) | (Ram.Read(address + 1) << 8));
    }

    public void Write(uint address, ushort value)
    {
        Console.WriteLine($"MEMORY[{Tools.Octal(address)}]: {Tools.Octal((byte)(value & 0xFF))}");
        Console.WriteLine($"MEMORY[{Tools.Octal(address + 1)}]: {Tools.Octal((byte)(value >> 8))}");
        Ram.Write(address, (byte)(value & 0xFF));
        Ram.Write(address + 1, (byte)(value >> 8));
    }
    
    public void Dump() => Ram.Dump();
}