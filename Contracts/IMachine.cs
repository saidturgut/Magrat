namespace Contracts;

public interface IMachine
{
    public void Init();

    public void Power(byte sleep);

    public void Clock();
    
    public void Load(uint address, byte data);

    public void Insert(byte[] image);

    public void Dump();
}
