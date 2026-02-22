namespace Models.Pdp1170.Cpu.Signaling;
using Microcodes;

public class Decoder
{
    public readonly Signal[] Fetch = Microcode.FETCH;
    
    public Signal[] Decode(ushort opcode)
        => [];
}