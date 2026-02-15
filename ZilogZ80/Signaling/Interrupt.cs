namespace ZilogZ80.Signaling;

public class Interrupt
{
    private bool iff1;
    private bool iff2;
    private byte mode; // 0, 1, 2

    private bool ei;
    public bool irq;

    public void Execute(State state)
    {
        switch (state)
        {
            case State.INT_E: ei = true; break;
            case State.INT_D: iff1 = false; iff2 = false; break;
            case State.INT_0: mode = 0; break;
            case State.INT_1: mode = 1; break;
            case State.INT_2: mode = 2; break;
            case State.RET_N: iff1 = iff2; break;
        }
    }

    public bool Check()
    {
        if (irq && iff1)
        {
            iff1 = false;
            return true;
        }
        
        if (ei)
        {
            iff1 = true; 
            iff2 = true;
            ei = false;
        }
        return false;
    }
}