namespace Models.Pdp1170.Devices;

public class Bus(byte highestLevel)
{
    private readonly bool[] Requests = new bool[highestLevel + 1];

    private byte currentMaster;

    public void Init()
    {
    }

    public void Restore()
    {
        for (byte i = 0; i < Requests.Length; i++)
            Requests[i] = false;
        Release();
    }

    public void Tick()
    {
        Arbitrate();
    }

    public bool Request(byte level)
    {
        if (currentMaster == level)
            return true;

        Requests[level] = true;

        return false;
    }

    private void Arbitrate()
    {
        if (currentMaster != 0) return;

        for (byte level = highestLevel; level > 0; level--)
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