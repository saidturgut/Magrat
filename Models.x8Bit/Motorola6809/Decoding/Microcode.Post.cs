namespace Models.x8Bit.Motorola6809.Decoding;
using Models.x8Bit.Engine;

public partial class Microcode
{
    private static Signal[] TRANSFER_BYTE(Pointer source, Pointer destination) =>
    [
        REG_COMMIT(source, destination),
    ];
    
    private static Signal[] TRANSFER_WORD(Pointer[] sources, Pointer[] destinations) =>
    [
        ..TRANSFER_BYTE(sources[0], destinations[0]),
        ..TRANSFER_BYTE(sources[1], destinations[1]),
    ];

    private static Signal[] EXCHANGE_BYTE(Pointer first, Pointer second) =>
    [
        REG_COMMIT(first, Pointer.TMP),
        REG_COMMIT(second, first),
        REG_COMMIT(Pointer.TMP, second),
    ];
    
    private static Signal[] EXCHANGE_WORD(Pointer[] first, Pointer[] second) =>
    [
        ..EXCHANGE_BYTE(first[0], second[0]),
        ..EXCHANGE_BYTE(first[1], second[1]),
    ];
}