namespace Models.Pdp1170.Cpu.Signaling;
using Microcodes;

public class Decoder
{
    private readonly Signal[][] Opcodes = Microcode.GenerateTable();
    
    public readonly Signal[] Nop = Microcode.NOP;
    public readonly Signal[] Fetch = Microcode.FETCH;
    public readonly Signal[] Trap = Microcode.TRAP;
    
    public Signal[] Decode(ushort opcode)
        => Opcodes[opcode] is not null ? Opcodes[opcode] : throw new Exception($"ILLEGAL OPCODE -> \"{Tools.Octal(opcode)}\"");
}