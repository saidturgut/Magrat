namespace Models.x8Bit.Mos6502;
using Engine.Units;
using Computing;
using Signaling;
using Contracts;
using Engine;

public class Mos6502(IBus bus) : Cpu(bus,
    new Datapath(14, new Computing.Alu(), new Fru(), new Logger()),
    new Control(new Decoder(), new Interrupt())), ICpu
{
    public ISudo Bus() => (ISudo)bus;
}