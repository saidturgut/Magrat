namespace Engine;
using Mos6502;
using ZilogZ80;
using Bounds;

public class Tables
{
    public ICpu? CpuTable(string name, IBus bus) => name switch
    {
        "none" => null,
        "mos6502" or "m6502" or "6502" => new Mos6502(bus),
        "zilogz80" or "zilog80" or "z80" => new ZilogZ80(bus),
        _ => throw new Exception($"UNKNOWN CPU: \"{name}\""),
    };
}