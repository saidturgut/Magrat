namespace Models.x8Bit;
using Contracts;
using Devices;

public class Machine
{
    public ICpu? CpuTable(string name, IMonitor monitor) => name switch
    {
        "none" => null,
        "mos6502" or "m6502" or "6502" => new Mos6502.Mos6502(new Bus(monitor)),
        "intel8080" or "i8080" or "8080" => new Intel8080.Intel8080(new Bus(monitor)),
        "zilogz80" or "zilog80" or "z80" => new ZilogZ80.ZilogZ80(new Bus(monitor)),
        _ => throw new Exception($"UNKNOWN CPU: \"{name}\""),
    };
}