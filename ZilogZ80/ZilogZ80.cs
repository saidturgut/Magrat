namespace ZilogZ80;
using Computing;
using Signaling;
using Bounds;
using Kernel;

public class ZilogZ80(IBus Bus) : Cpu(Bus, 
    new Datapath(32, new Alu(), new Fru(), new Logger()), 
    new Control(new Decoder(), new Interrupt())), ICpu;