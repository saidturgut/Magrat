namespace Models.Pdp1170.Cpu.Signaling;
using Microcodes;

public class Decoder
{
    private readonly Signal[][] Opcodes = Microcode.Generate();
    
    public readonly Signal[] Fetch = Microcode.FETCH;
    
    public Signal[] Decode(ushort opcode)
        => Opcodes[opcode] is not [] ? Opcodes[opcode] : throw new Exception($"ILLEGAL OPCODE -> \"0{Convert.ToString(opcode, 8)}\"");
}