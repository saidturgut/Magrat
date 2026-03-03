namespace Models.Pdp1170.Kernel.Addressing;

public partial class Cache
{
    public ushort Read(Width width)
    {
        ushort word = !highWord ? DataLow[index] : DataHigh[index];
        if (width == Width.BYTE) word = !highByte ? (byte)(word & 0x00FF) : (byte)((word >> 8) & 0x00FF);
        Console.WriteLine($"CACHE[{Tools.Octal(index)}]: READ {Tools.Octal(word)}");
        return word;
    }

    public void Write(ushort data, Width width)
    {
        if (width == Width.WORD)
        {
            if (!highWord) DataLow[index] = data;
            else DataHigh[index] = data;
        }
        else
        {
            ushort word = !highWord ? DataLow[index] : DataHigh[index];

            if (!highByte) data = (ushort)((word & 0xFF00) | (data & 0x00FF));
            else data = (ushort)((word & 0x00FF) | ((data & 0x00FF) << 8));
        }
        
        if (!highWord) DataLow[index] = data;
        else DataHigh[index] = data;
        
        Console.WriteLine($"CACHE[{Tools.Octal(index)}]: WRITE {Tools.Octal(data)}");

        Tags[index] |= dirtyBit;
    }
}