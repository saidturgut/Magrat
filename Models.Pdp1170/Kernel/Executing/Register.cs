namespace Models.Pdp1170.Kernel.Executing;

public class CommitRegister
{
    private ushort commit;
    private ushort value;

    public void Init(ushort input)
    {
        Set(input);
        Commit(false);
    }
    
    public void Set(ushort input)
        => value = input;

    public ushort Get()
        => value;

    public void Commit(bool abort)
    {
        if(!abort) commit = value;
        else value = commit;
    }
}

public class DeviceRegister
{
    private ushort value;
    
    public byte GetByte(uint address)
        => address % 2 != 0 ? (byte)(value >> 8) : (byte)(value & 0xFF);
    public void SetByte(uint address, byte data)
        => value = (ushort)((value & 0x00FF) | ((address % 2 != 0) ? data << 8 : data));

    public ushort GetWord()
        => value;
    public void SetWord(ushort data)
        => value = data;

    public bool GetBit(byte bit)
        => (value & (1 << bit)) != 0;
    public void SetBit(byte bit)
        => value |= (ushort)(1 << bit);
    public void ClearBit(byte bit)
        => value &= (ushort)~(1 << bit);
}