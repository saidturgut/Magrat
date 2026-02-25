namespace Models.Pdp1170.Cpu.Translating;

public class Mmu
{
    public uint Translate(ushort virtualAddr, Space space)
    {
        return virtualAddr;
    }
}

public enum Space
{
    Instruction, Data
}