namespace Models.Pdp1170.Kernel.Signaling;

public partial class Trapper
{
    private readonly Vector[] TrapRequests = new Vector[6];

    public bool abort;
    private bool suppress;
    
    public void Execute(Signal signal)
    {
        switch (signal.State)
        {
            case State.REQUEST: RequestPostTrap(signal.Vector); break;
            case State.SUPPRESS: suppress = true; break;
        }
    }
    
    public void RequestAbortTrap(Vector vector)
    {
        TrapRequests[TrapsTable[(byte)vector].Priority] = vector;
        Console.WriteLine($"ABORT TRAP REQUESTED: {vector}");
        abort = true;
    }
    public void RequestPostTrap(Vector vector)
    {
        TrapRequests[TrapsTable[(byte)vector].Priority] = vector;
        Console.WriteLine($"POST TRAP REQUESTED: {vector}");
    }
    public void RequestTraceTrap()
    {
        if (suppress)
        {
            suppress = false;
            return;
        }
        TrapRequests[TrapsTable[(byte)Vector.TRACE].Priority] = Vector.TRACE;
        Console.WriteLine($"POST TRAP REQUESTED: TRACE");
    }
    public void RequestInterrupt(ushort vector)
    {
        TrapsTable[(byte)Vector.INTERRUPT].Address = vector;
        TrapRequests[TrapsTable[(byte)Vector.INTERRUPT].Priority] = Vector.INTERRUPT;
        Console.WriteLine($"INTERRUPT REQUESTED: {Tools.Octal(vector)}");
    }

    public Trap Arbitrate()
    {
        for (byte level = 0; level < TrapRequests.Length; level++)
        {
            if (TrapRequests[level] is not Vector.NONE)
            {
                var arbitrated = TrapsTable[(byte)TrapRequests[level]];
                
                arbitrated.Abort = abort;
                abort = false;
                suppress = true;
                
                for (byte index = 0; index < 5; index++)
                    TrapRequests[index] = Vector.NONE;
                
                return arbitrated;
            }
        }
        abort = false;
        return TrapsTable[0];
    }
}
