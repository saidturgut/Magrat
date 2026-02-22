namespace Models.Pdp1170.Cpu.Executing;
using Computing;
using Bus;

public partial class Datapath
{
    private readonly Register[] CoreRegisters =  new Register[8];
    private readonly Register[] GeneralRegisters =  new Register[12];
    private readonly Register[] StackRegisters =  new Register[4];
    
    private Signal signal;
    private StatusWord statusWord;

    private bool stall;
    
    private string debugName = "";

    public void Init()
    {
        for (byte i = 0; i < CoreRegisters.Length; i++)
            CoreRegisters[i] =  new Register();
        for (byte i = 0; i < GeneralRegisters.Length; i++)
            GeneralRegisters[i] =  new Register();
        for (byte i = 0; i < StackRegisters.Length; i++)
            StackRegisters[i] =  new Register();
    }
    
    public void Receive(Signal input)
        => signal = input;

    private void Protocol()
    {
        statusWord = new StatusWord(CoreRegisters[(byte)Pointer.PSW].Get());
        if (signal.Name != "") debugName = signal.Name;
        Point(Pointer.NIL).Set(0);
    }
    
    public void Execute(Unibus unibus)
    {
        Protocol();
        switch (signal.Cycle)
        {
            case Cycle.REG_WRITE: RegisterWrite(); break;
            case Cycle.MEM_READ: MemoryRead(unibus); break;
            case Cycle.MEM_WRITE: MemoryWrite(unibus); break;
            case Cycle.ALU_COMPUTE: AluCompute(); break;
        }
    }
    
    private Register Point(Pointer pointer) => pointer switch
    {
        Pointer.R0 or Pointer.R1 or Pointer.R2 or Pointer.R3 or Pointer.R4 or Pointer.R5 
            => GeneralRegisters[(byte)pointer - CoreRegisters.Length + (statusWord.RegisterSet * 6)],
        Pointer.SP => StackRegisters[(byte)statusWord.CurrentMode],
        _ => CoreRegisters[(byte)pointer]
    };

    public ControlSignal Emit()
        => new (Point(Pointer.IR).Get(), stall);
}