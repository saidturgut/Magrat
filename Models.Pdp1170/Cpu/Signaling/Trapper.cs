namespace Models.Pdp1170.Cpu.Signaling;

public partial class Trapper
{
    private readonly Trap[] TrapRequests = new Trap[7];

    public bool abort;
    
    public void Execute(State state)
    {
        
    }

    public void RequestAbortTrap(Trap trap)
    {
        TrapRequests[TrapsTable[(byte)trap].Priority] = trap;
        Console.WriteLine($"ABORT TRAP REQUESTED: {trap}");
        abort = true;
    }
    public void RequestPostTrap(Trap trap)
    {
        TrapRequests[TrapsTable[(byte)trap].Priority] = trap;
        Console.WriteLine($"POST TRAP REQUESTED: {trap}");
    }
    public void RequestInterrupt(ushort vector)
    {
        TrapsTable[(byte)Trap.INTERRUPT].Vector = vector;
        TrapRequests[TrapsTable[(byte)Trap.INTERRUPT].Priority] = Trap.INTERRUPT;
        Console.WriteLine($"INTERRUPT REQUESTED: 0x{vector}");
    }

    public TrapInfo Arbitrate()
    {
        for (byte level = 0; level < TrapRequests.Length; level++)
        {
            if (TrapRequests[level] is not Trap.NONE)
            {
                TrapInfo arbitrated = TrapsTable[(byte)TrapRequests[level]];
                
                arbitrated.Abort = abort;
                abort = false;
                
                for (byte index = 0; index < 7; index++)
                    TrapRequests[index] = Trap.NONE;
                
                return arbitrated;
            }
        }
        abort = false;
        return TrapsTable[0];
    }
}
