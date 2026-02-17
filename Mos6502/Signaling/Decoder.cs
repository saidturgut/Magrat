namespace Mos6502.Signaling;
using Kernel.Devices;
using Decoding;
using Kernel;


public class Decoder : IDecoder
{
    private readonly Signal[][] Table = Microcode.OpcodeRom(false);

    public Signal[] Fetch() => Microcode.FETCH;
    public Signal[] Interrupt() => [];

    public Signal[] Decode(byte opcode) => Table[opcode] != Array.Empty<Signal>()
        ? Table[opcode]
        : throw new Exception($"ILLEGAL OPCODE \"{opcode}\"");

    public void Clear(){}
}
