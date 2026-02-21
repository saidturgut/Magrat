namespace Models.x8Bit.Engine;

public class Register
{
    private byte value;

    public void Set(byte input)
        => value = input;
    
    public byte Get()
        => value;
}