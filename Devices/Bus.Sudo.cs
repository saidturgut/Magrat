namespace Devices;
using Bounds;

public partial class Bus : ISudo
{
    public void Insert(byte[] image)
        => Ioc.Dsk.Insert(image);
}