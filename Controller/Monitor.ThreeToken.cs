namespace Controller;

public partial class Monitor
{
    private void Mem(string addr, string dat)
    {
        uint address = Convert.ToUInt32(addr);
        byte data = Convert.ToByte(dat);
        Bus!.Load(address, data);
    }

    private void Load(string addr, string name)
    {
        uint address = Convert.ToUInt32(addr);
        byte[] image = File.ReadAllBytes(name);
        for (int i = 0; i < image.Length; i++)
        {
            Bus!.Load((uint)(address + i), image[i]);
        }
    }
    
    private void Reg(string reg, string dat) { }
    
        
    private void Dump(string start, string end)
    {
        Bus!.Dump(Convert.ToUInt32(start), Convert.ToUInt32(end));
    }
}