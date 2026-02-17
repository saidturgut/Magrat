namespace Kernel.Devices;

public interface IDecoder
{
    public Signal[] Fetch();
    
    public Signal[] Interrupt();
    
    public Signal[] Decode(byte opcode);
    
    public void Clear();
}