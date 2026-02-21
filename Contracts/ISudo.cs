namespace Contracts;

public interface ISudo
{
    public void Load(uint address, byte data);

    public void Insert(byte[] image);

    public void Dump(uint start, uint end);
}
