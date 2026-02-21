namespace Models.x8Bit.Engine.Units;

public interface IDecoder
{
    public Signal[] Fetch();
    
    public Signal[] Interrupt();
    
    public Signal[] Decode(byte opcode);
    
    public bool Execute(byte state);
    
    public void Clear();
}