namespace Models.Pdp1170.Kernel.Addressing;

public class Mmu
{
    public MmuOutput Translate(ushort virtualAddr, Space space)
    {
        return new MmuOutput
        {
            Address = virtualAddr,
            Vector = Vector.NONE,
        };
    }
}

public struct MmuOutput
{
    public uint Address;
    public Vector Vector;
}

public enum Space
{
    Instruction, Data
}