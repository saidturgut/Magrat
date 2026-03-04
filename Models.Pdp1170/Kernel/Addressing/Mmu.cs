namespace Models.Pdp1170.Kernel.Addressing;
using Executing;

public class Mmu
{
    private readonly DeviceRegister[] DescriptorRegisters = new DeviceRegister[3 * 2 * 8];
    private readonly DeviceRegister[] AddressRegisters = new DeviceRegister[3 * 2 * 8];
    private readonly DeviceRegister[] ControlRegisters = new DeviceRegister[4];

    public void Init()
    {
        for (byte index = 0; index < DescriptorRegisters.Length; index++)
        {
            DescriptorRegisters[index] = new DeviceRegister();
            AddressRegisters[index] = new DeviceRegister();
        }
        for (byte index = 0; index < ControlRegisters.Length; index++)
            ControlRegisters[index] = new DeviceRegister();
    }

    public MmuOutput Translate(ushort virtualAddr, Mode mode, Space space, bool write)
    {
        if (PointMmr(0).GetBit(0))
            return new MmuOutput { Address = virtualAddr };

        MmuOutput output = new MmuOutput();

        byte page = (byte)((virtualAddr >> 13) & 7);
        ushort offset = (ushort)(virtualAddr & 0x1FFF);
        byte block = (byte)(offset >> 6);

        var par = PointPar(mode, space, page);
        var pdr = PointPdr(mode, space, page);

        MmuError error;

        if ((pdr.GetWord() & 7) == 0)
            error = MmuError.NR;

        return output;
    }

    private DeviceRegister DecodeAddress(uint address)
    {
        uint group = address - 0x7FF000u;
        return group switch
        {
            0x54E => PointMmr(3),
            0xF7E => PointMmr(2),
            0xF7C => PointMmr(1),
            0xF7A => PointMmr(0),
            >= 0xF80 and <= 0xFBE => DecodePage(group, Mode.USER, 0xF80),
            >= 0x440 and <= 0x47E => DecodePage(group, Mode.KERNEL, 0x440),
            >= 0x400 and <= 0x43E => DecodePage(group, Mode.SUPERVISOR, 0x400),
        };
    }
    
    private DeviceRegister DecodePage(uint group, Mode mode, ushort offset)
    {
        byte index = (byte)(group - offset);
        return index switch
        {
            >= 0xB0 and <= 0xBE => IndexPar(mode, Space.Data, (byte)((0xBE - index) >> 1)),
            >= 0xA0 and <= 0xAE => IndexPar(mode, Space.Instruction, (byte)((0xAE - index) >> 1)),
            >= 0x90 and <= 0x9E => IndexPdr(mode, Space.Data, (byte)((0x9E - index) >> 1)),
            >= 0x80 and <= 0x8E => IndexPdr(mode, Space.Instruction, (byte)((0x8E - index) >> 1)),
        };
    }
    
    private Space CheckSpace(Mode mode, Space space)
        => PointMmr(3).GetBit(mode switch
        {
            Mode.KERNEL => 2,
            Mode.SUPERVISOR => 3,
            Mode.USER => 4,
        }) ? space : Space.Data;
    
    private DeviceRegister IndexPdr(Mode mode, Space space, byte page)
        => DescriptorRegisters[((byte)mode << 4) | ((byte)space << 3) | page];
    private DeviceRegister IndexPar(Mode mode, Space space, byte page)
        => AddressRegisters[((byte)mode << 4) | ((byte)space << 3) | page];
    private DeviceRegister PointPdr(Mode mode, Space space, byte page)
        => DescriptorRegisters[((byte)mode << 4) | ((byte)CheckSpace(mode, space) << 3) | page];
    private DeviceRegister PointPar(Mode mode, Space space, byte page)
        => AddressRegisters[((byte)mode << 4) | ((byte)CheckSpace(mode, space) << 3) | page];
    private DeviceRegister PointMmr(byte index)
        => ControlRegisters[index];
}

public struct MmuOutput
{
    public uint Address;
    public Vector Vector;
}

[Flags]
public enum MmuError : ushort
{
    NONE = 0,
    TR = 1 << 12, RO = 1 << 13,
    PL = 1 << 14, NR = 1 << 15,
}
