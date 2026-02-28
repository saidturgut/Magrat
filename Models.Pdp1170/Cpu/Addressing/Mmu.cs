namespace Models.Pdp1170.Cpu.Addressing;

public class Mmu
{
    public MmuOutput Translate(ushort virtualAddr, Space space)
    {
        return new MmuOutput()
        {
            Address = virtualAddr,
            Trap = Trap.NONE,
        };
    }
}

public struct MmuOutput
{
    public uint Address;
    public Trap Trap;
}

public enum Space
{
    Instruction, Data
}