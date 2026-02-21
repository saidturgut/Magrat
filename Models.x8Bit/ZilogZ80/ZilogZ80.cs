namespace Models.x8Bit.ZilogZ80;
using Engine.Units;
using Computing;
using Signaling;
using Contracts;
using Engine;

public class ZilogZ80(IBus bus) : Cpu(bus, 
    new Datapath(32, new Computing.Alu(), new Fru(), new Logger()), 
    new Control(new Decoder(), new Interrupt())), ICpu
{
    public ISudo Bus() => (ISudo)bus;
}