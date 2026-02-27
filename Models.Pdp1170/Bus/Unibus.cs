namespace Models.Pdp1170.Bus;

public partial class Unibus
{
    private readonly Ram Ram = new();
    
    private readonly bool[] Requests = new bool[6];

    private byte currentMaster;
    
    public void Init() { }
    
    public void Restore() { }
    
    public bool Request(byte level)
    {
        if (currentMaster == level)
            return true;
        
        Requests[level] = true;
        
        return false;
    }

    public void Arbitrate()
    {
        if(currentMaster != 0) return;
        
        for (byte level = 5; level > 0; level--)
        {
            if (Requests[level])
            {
                currentMaster = level;
                break;
            }
        }
    }

    public void Release()
    {
        Requests[currentMaster] = false;
        currentMaster = 0;
    }
}
