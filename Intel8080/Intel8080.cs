namespace Intel8080;
using Computing;
using Signaling;
using Bounds;
using Kernel;

public class Intel8080(IBus Bus) : Cpu(Bus, 
    new Datapath(18, new Alu(), new Fru(), new Logger()), 
    new Control(new Decoder(), new Interrupt())), ICpu;