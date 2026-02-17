namespace Kernel;
using Devices;
using Bounds;

public partial class Datapath(byte registerCount, IAlu Alu, IFru Fru, ILogger Logger)
{
    private readonly Register[] Registers = new Register[registerCount];
    
    private Signal signal = new();

    private bool abort;
    
    private string debugName = "";
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
        PointK(PointerK.NIL).Set(0);
    }

    private Register PointK(PointerK pointer)
        => Registers[(byte)pointer];
    
    private Register Point(byte pointer)
        => Registers[pointer];

    public ControlSignal Emit()
        => new(PointK(PointerK.IR).Get(), abort);

    public void Debug()
        => logs = Logger.Debug(Registers, debugName);
}


