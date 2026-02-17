namespace Mos6502;
using Computing;
using Signaling;
using Bounds;
using Kernel;

public class Mos6502(IBus Bus) : Cpu(Bus,
    new Datapath(14, new Alu(), new Fru(), new Logger()),
    new Control(new Decoder(), new Interrupt())), ICpu;