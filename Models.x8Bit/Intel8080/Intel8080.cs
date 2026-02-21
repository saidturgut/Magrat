namespace Models.x8Bit.Intel8080;
using Engine.Units;
using Computing;
using Signaling;
using Contracts;
using Engine;

public class Intel8080(IBus bus) : Cpu(bus, 
    new Datapath(18, new Computing.Alu(), new Fru(), new Logger()), 
    new Control(new Decoder(), new Interrupt())), ICpu
{
    public ISudo Bus() => (ISudo)bus;
}