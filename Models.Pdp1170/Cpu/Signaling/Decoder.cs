namespace Models.Pdp1170.Cpu.Signaling;
using Microcodes;

public class Decoder
{
    private readonly Signal[][] Opcodes = Microcode.Generate();
    
    public readonly Signal[] Fetch = Microcode.FETCH;
    
    public Signal[] Decode(ushort opcode)
        => Opcodes[opcode];
}