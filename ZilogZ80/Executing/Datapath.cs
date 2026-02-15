namespace ZilogZ80.Executing;
using Signaling;
using Bounds;

public partial class Datapath
{
    private readonly Register[] Registers = new Register[32];
    
    private Signal signal = new();

    private bool abort;

    public List<string> logs = [];
    
    public void Init()
    {
        for (byte i = 0; i < Registers.Length; i++)
            Registers[i] =  new Register();
    }
    
    public void Receive(Signal input)
        => signal = input;

    public void Execute(IBus bus)
    {
        switch (signal.Cycle)
        {
            case Cycle.REG_COMMIT: RegisterWrite(); break;
            case Cycle.MEM_READ: MemoryRead(bus); break;
            case Cycle.MEM_WRITE: MemoryWrite(bus); break;
            case Cycle.ALU_COMPUTE: AluCompute(); break;
            case Cycle.COND_COMPUTE: CondCompute(); break;
            case Cycle.PAIR_INC: Increment(); break;
            case Cycle.PAIR_DEC: Decrement(); break;
        }
        Protocol();
    }

    private void Protocol()
    {
        if (signal.Name != "") debugName = signal.Name;
        Point(Pointer.NIL).Set(0);
    }

    private Register Point(Pointer pointer)
        => Registers[(byte)pointer];

    public ControlSignal Emit()
        => new(Point(Pointer.IR).Get(), abort);
}

public struct ControlSignal(byte opcode, bool abort)
{
    public readonly byte Opcode = opcode;
    public readonly bool Abort = abort;
}