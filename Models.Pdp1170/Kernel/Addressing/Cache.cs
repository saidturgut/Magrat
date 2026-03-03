namespace Models.Pdp1170.Kernel.Addressing;
using Devices;

public partial class Cache
{
    private readonly ushort[] Tags = new ushort[0x200];
    private readonly ushort[] DataLow = new ushort[0x200];
    private readonly ushort[] DataHigh = new ushort[0x200];

    private byte offset;
    private ushort index, tag, entry, oldTag;
    private bool valid, dirty, highWord, highByte;
    private uint newBase;
    
    private const byte validBit = 0b01;
    private const byte dirtyBit = 0b10;
    private const byte tagShift = 5;

    public void Extract(uint address)
    {
        index = (ushort)((address >> 2) & 0x1FF);
        offset = (byte)(address & 0b11);
        tag = (ushort)(address >> 11);
        newBase = address & 0xFFFFFFFCu;

        entry = Tags[index];
        valid = (entry & validBit) != 0;
        dirty = (entry & dirtyBit) != 0;
        oldTag = (ushort)(entry >> tagShift);
        
        highWord = (offset & 0b10) != 0;
        highByte = (offset & 0b01) != 0;
    }

    public bool Hit()
    {
        bool output = valid && oldTag == tag;
        //if(output) Console.WriteLine($"CACHE[{Tools.Octal(index)}]: HIT");
        return output;
    }
    
    public void Miss(Membus membus)
    {
        //Console.WriteLine($"CACHE[{Tools.Octal(index)}]: MISS");
        
        uint oldBase = (uint)((oldTag << 11) | (index << 2));

        if (valid && dirty)
        {
            membus.Write(oldBase, DataLow[index]);
            membus.Write(oldBase + 2, DataHigh[index]);
        }
        
        DataLow[index]  = membus.Read(newBase);
        DataHigh[index] = membus.Read(newBase + 2);
        
        Tags[index] = (ushort)((tag << tagShift) | validBit);
        
        membus.Release();
    }
}
