namespace Models.Pdp1170.Kernel.Executing;
using Microcodes;
using Signaling;
using Computing;
using Devices;

public partial class Datapath
{
    private Signal signal;
    private StatusWord statusWord;

    private Trapper Trapper = null!;

    private string debugName = "";

    private bool stall;
    private bool skip;
    
    public void Init(Unibus unibus, Membus membus, Trapper trapper)
    {
        for (byte i = 0; i < CoreRegisters.Length; i++)
            CoreRegisters[i] =  new CommitRegister();
        for (byte i = 0; i < TemporaryLatches.Length; i++)
            TemporaryLatches[i] =  new CommitRegister();
        for (byte i = 0; i < GeneralRegisters.Length; i++)
            GeneralRegisters[i] =  new CommitRegister();
        for (byte i = 0; i < StackPointers.Length; i++)
            StackPointers[i] =  new CommitRegister();
        Biu.Init(unibus, membus);
        Mmu.Init();
        Trapper = trapper;
        
        StackPointers[0].Init(0xCCCC);
        //PointCore(Pointer.PC).Init(0x1000);
    }
    
    public void Restore()
    {
        signal = new Signal();
        statusWord = new StatusWord(0);
        stall = false;
        skip = false;
    }
    
    public void Receive(Signal input)
        => signal = input;

    private void Protocol()
    {
        statusWord = new StatusWord(PointCore(Pointer.PSW).Get());
        PointCore(Pointer.NIL).Set(0);
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
        => new (PointCore(Pointer.IR).Get(), stall, skip);
    
    public void Commit(Trap trap)
    {
        Clear();
        if(statusWord.Trace) Trapper.RequestTraceTrap();
        debugName = signal.State is not State.TRAP 
            ? Microcode.LoggerNames[PointCore(Pointer.IR).Get()] : (trap.Abort ? "ABORT " : "POST ") + "TRAP ROUTINE";
        foreach (var reg in CoreRegisters) reg.Commit(trap.Abort);
        foreach (var reg in TemporaryLatches) reg.Commit(true);
        foreach (var reg in GeneralRegisters) reg.Commit(trap.Abort);
        foreach (var reg in StackPointers) reg.Commit(trap.Abort);
        PointLatch(Pointer.VEC).Set(trap.Address);
    }

    private void Clear()
    {
        stall = false;
        skip = false;
    }
}