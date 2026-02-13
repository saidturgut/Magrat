namespace ZilogZ80.Signaling;

public partial class Decoder
{
    private byte pageIndex;
    private bool dispSwap;
    
    private bool ED()
    {
        pageIndex = 1;
        dispSwap = false;
        return true;
    }
    
    private bool CB()
    {
        switch (pageIndex)
        {
            case 0:
                pageIndex += 2;
                dispSwap = false;
                return true;
            case 3 or 4:
                pageIndex += 2;
                dispSwap = true;
                return true;
            default:
                return false;
        }
    }
    private bool DD()
    {
        if (pageIndex is not (0 or 3 or 4)) return false;
        pageIndex = 3;
        dispSwap = false;
        return true;
    }
    private bool FD()
    {
        if (pageIndex is not (0 or 3 or 4)) return false;
        pageIndex = 4;
        dispSwap = false;
        return true;
    }
}