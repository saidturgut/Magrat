namespace Models.Pdp1170.Cpu.Addressing;

public partial class Cache
{
    public ushort Read(Width width)
    {
        ushort data = !highWord ? DataLow[index] : DataHigh[index];
        
        return width == Width.WORD ? data : 
            highByte ? (byte)(data & 0x00FF) : (byte)((data >> 8) & 0x00FF);
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

            if (highByte) data = (ushort)((word & 0xFF00) | (data & 0x00FF));
            else data = (ushort)((word & 0x00FF) | ((data & 0x00FF) << 8));
        }
        
        if (!highWord) DataLow[index] = data;
        else DataHigh[index] = data;
        
        Tags[index] |= dirtyBit;
    }
}