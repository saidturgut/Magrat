namespace Models.Pdp1170.Cpu.Executing;
using Signaling;
using Computing;
using Bus;

public partial class Datapath
{
    private Signal signal;
    private StatusWord statusWord;

    private Trapper Trapper;

    private bool stall;
    private bool skip;
    
    private string debugName = "";

    public void Init(Unibus unibus, Trapper trapper)
    {
        for (byte i = 0; i < CoreRegisters.Length; i++)
            CoreRegisters[i] =  new Register();
        for (byte i = 0; i < TemporaryLatches.Length; i++)
            TemporaryLatches[i] =  new Register();
        for (byte i = 0; i < GeneralRegisters.Length; i++)
            GeneralRegisters[i] =  new Register();
        for (byte i = 0; i < StackPointers.Length; i++)
            StackPointers[i] =  new Register();
        Biu.Init(unibus);
        Trapper = trapper;
    }
    
    public void Restore()
    {
        signal = new Signal();
        statusWord = new StatusWord(0);
        stall = false;
        skip = false;
        debugName = "";
    }
    
    public void Receive(Signal input)
        => signal = input;

    private void Protocol()
    {
        statusWord = new StatusWord(CoreRegisters[(byte)Pointer.PSW].Get());
        if (signal.Name != "") debugName = signal.Name;
        Point(Pointer.NIL).Set(0);
    }
    
    public void Execute()
    {
        Protocol();
        switch (signal.Cycle)
        {
            case Cycle.REG_MOVE: RegisterMove(); break;
            case Cycle.MEM_FETCH: MemoryFetch(); break;
            case Cycle.MEM_READ: MemoryRead(); break;
            case Cycle.MEM_WRITE: MemoryWrite(); break;
            case Cycle.ALU_COMPUTE: AluCompute(); break;
            case Cycle.COND_COMPUTE: CondCompute(); break;
        }
    }
    
    public ControlSignal Emit()
        => new (Point(Pointer.IR).Get(), stall, skip);

    public void Commit(TrapInfo info)
    {
        foreach (Register reg in CoreRegisters)
            reg.Commit(info.Abort);
        foreach (Register reg in TemporaryLatches)
            reg.Commit(true);
        foreach (Register reg in GeneralRegisters)
            reg.Commit(info.Abort);
        foreach (Register reg in StackPointers)
            reg.Commit(info.Abort);
        
        Point(Pointer.VEC).Set(info.Vector);
    }
}