namespace Engine;

public partial class Engine
{
    private void Mem(string addr, string dat)
    {
        uint address = Convert.ToUInt32(addr);
        byte data = Convert.ToByte(dat);
        Bus!.Write(address, data, Bus);
    }

    private void Load(string addr, string name)
    {
        uint address = Convert.ToUInt32(addr);
        byte[] image = File.ReadAllBytes(name);
        for (int i = 0; i < image.Length; i++)
        {
            Bus!.Write((uint)(address + i), image[i], Bus);
        }
    }
    
    private void Reg(string reg, string dat) { }
}