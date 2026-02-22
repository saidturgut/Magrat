namespace Models.Pdp1170.Cpu.Microcodes;

public partial class Microcode
{
    private static Signal[] MOV() =>
    [
        ..AddressingModes[encodedModes[0]](0, Pointer.SRC),
        ..AddressingModes[encodedModes[1]](1, Pointer.DST),
        ..MemoryWriteback() 
            ? (Signal[])[REG_WRITE(Pointer.DST, Pointer.MDR), MEM_WRITE(Pointer.SRC, encodedWidth)] 
            : [REG_WRITE(Pointer.SRC, Pointer.DST, encodedWidth)],
    ];
    
    private static Signal[] ALU() =>
    [
        ..AddressingModes[encodedModes[0]](0, Pointer.SRC),
        ..AddressingModes[encodedModes[1]](1, Pointer.DST),
        ..MemoryWriteback() 
            ? (Signal[])[REG_WRITE(Pointer.DST, Pointer.MDR), MEM_WRITE(Pointer.SRC, encodedWidth)] 
            : [REG_WRITE(Pointer.SRC, Pointer.DST, encodedWidth)],
    ];
}