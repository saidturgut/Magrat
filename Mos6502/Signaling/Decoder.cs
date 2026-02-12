namespace Mos6502.Signaling;
using Microcodes;

public class Decoder
{
    private readonly Signal[][] Table = Microcode.OpcodeRom(false);

    public readonly Signal[] Fetch = Microcode.FETCH;

    public Signal[] Decode(byte opcode) => Table[opcode] != Array.Empty<Signal>()
        ? Table[opcode]
        : throw new Exception($"ILLEGAL OPCODE \"{opcode}\"");
}
