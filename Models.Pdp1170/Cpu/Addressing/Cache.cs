namespace Models.Pdp1170.Cpu.Addressing;
using Bus;

public partial class Cache
{
    private readonly ushort[] Tags = new ushort[0x200];
    private readonly ushort[] DataLow = new ushort[0x200];
    private readonly ushort[] DataHigh = new ushort[0x200];

    private ushort index;
    private byte offset;
    private ushort tag;
    
    private ushort entry;
    private bool valid;
    private bool dirty;
    private ushort oldTag;
    private uint oldBase;

    private uint blockBase;

    private bool highWord;
    private bool highByte;
    
    private const byte validBit = 0b01;
    private const byte dirtyBit = 0b10;
    private const byte tagShift = 5;

    public void Extract(uint address)
    {
        index = (ushort)((address >> 2) & 0x1FF);
        offset = (byte)(address & 0b11);
        tag = (ushort)(address >> 11);
        
        entry = Tags[index];
        valid = (entry & validBit) != 0;
        dirty = (entry & dirtyBit) != 0;
        oldTag = (ushort)(entry >> tagShift);
        oldBase = (uint)((oldTag << 11) | (index << 2));
        
        blockBase = address & 0xFFFFFFFCu;

        highWord = (offset & 0b10) != 0;
        highByte = (offset & 0b01) == 0;
    }
    
    public bool Hit()
        => valid && oldTag == tag;
    
    public void Miss(Unibus unibus)
    {
        if (valid && dirty)
        {
            unibus.WriteWord(oldBase, DataLow[index]);
            unibus.WriteWord(oldBase + 2, DataHigh[index]);
        }
        
        DataLow[index]  = unibus.ReadWord(blockBase);
        DataHigh[index] = unibus.ReadWord(blockBase + 2);
        
        Tags[index] = (ushort)((tag << tagShift) | validBit);
    }
}
