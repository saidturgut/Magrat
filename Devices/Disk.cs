namespace Devices;
using Bounds;

public class Disk
{
    private byte[] diskImage = [];
    
    private const byte blockSize = 128;

    private ushort blockNumber;
    private ushort dmaAddress;
    private byte status;

    public void Insert(byte[] image)
        => diskImage = image;

    public byte ReadStatus()
        => status;

    public void WriteBlockLow(byte data)
        => blockNumber = (ushort)((blockNumber & 0xFF00) | data);
    public void WriteBlockHigh(byte data)
        => blockNumber = (ushort)((blockNumber & 0xFF) | (data << 8));
    public void WriteDmaLow(byte data)
        => dmaAddress = (ushort)((dmaAddress & 0xFF00) | data);
    public void WriteDmaHigh(byte data)
        => dmaAddress = (ushort)((dmaAddress & 0xFF) | (data << 8));

    public void WriteCommand(byte data, IBus bus)
    {
        switch (data)
        {
            case 0: // NOP
            case 1: ReadBlockDisk(bus); break;
            case 2: WriteBlockDisk(bus); break;
        }
    }
    private void ReadBlockDisk(IBus bus)
    {
        uint offset = CalculateOffset();
        if(CheckFault(offset)) return;
        var dma = dmaAddress;
        
        for (int i = 0; i < blockSize; i++)
        {
            bus.Write(dma++, diskImage[offset + i], bus);
        }
    }
    private void WriteBlockDisk(IBus bus)
    {
        uint offset = CalculateOffset();
        if(CheckFault(offset)) return;
        var dma = dmaAddress;

        for (int i = 0; i < blockSize; i++)
        {
            diskImage[offset + i] = bus.Read(dma++, bus);
        }
    }

    private uint CalculateOffset()
        => (uint)(blockNumber * blockSize);

    private bool CheckFault(uint offset)
    {
        bool fault = diskImage.Length == 0 || offset + blockSize > diskImage.Length;
        status = (byte)(fault ? 0xFF : 0x00);
        return fault;
    } 
}