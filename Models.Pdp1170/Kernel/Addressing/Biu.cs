namespace Models.Pdp1170.Kernel.Addressing;
using Devices;

public class Biu
{
    private Unibus Unibus = null!;
    private Membus Membus = null!;
    
    private readonly Cache Cache = new ();

    private bool cacheable;
    
    private const byte memLevel = 3;
    private const byte uniLevel = 5;
    
    public void Init(Unibus unibus, Membus membus)
    {
        Unibus = unibus;
        Membus = membus;
    }

    public bool Validate(uint address)
    {
        if (!Membus.Validate(address)) return false;
        cacheable = Membus.CheckIoPage(address);
        if(cacheable) Cache.Extract(address);
        return true;
    }

    public static bool CheckIoPage(uint address)
        => address is ((>= 0x7FFF80 and <= 0x7FFFFE) or (>= 0x7FF480 and <= 0x7FF4FE));
    
    public bool Ready()
    {
        if (!cacheable) 
            return Unibus.Request(uniLevel);
        
        if (Cache.Hit())
            return true;

        if (!Membus.Request(memLevel)) 
            return false;

        Cache.Miss(Membus);
        
        return true;
    }

    public ushort Read(uint address, Width width)
    {
        if (cacheable) return Cache.Read(width);
        ushort data = width is Width.WORD ? Unibus.ReadWord(address) :  Unibus.ReadByte(address);
        Unibus.Release();
        return data;
    }
    
    public void Write(uint address, ushort value, Width width)
    {
        if (cacheable) Cache.Write(value, width);
        else
        {
            switch (width)
            {
                case Width.WORD: Unibus.WriteWord(address, value); break;
                case Width.BYTE: Unibus.WriteByte(address, (byte)value); break;
            }
            Unibus.Release();
        }
    }
}