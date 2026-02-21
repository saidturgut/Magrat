namespace Models.x8Bit.Engine;
using Units;

public partial class Datapath
{
    private void RegisterWrite()
        => Point(signal.Second).Set(Point(signal.First).Get());

    private void MemoryRead(IBus bus)
        => PointK(PointerK.MDR).Set(bus.Read(Merge(Point(signal.First).Get(), Point(signal.Second).Get()), bus));

    private void MemoryWrite(IBus bus)
        => bus.Write(Merge(Point(signal.First).Get(), Point(signal.Second).Get()), PointK(PointerK.MDR).Get(), bus);
    
    private void Increment()
    {
        Point(signal.First).Set((byte)(Point(signal.First).Get() + 1));
        if (Point(signal.First).Get() == 0x00)
            Point(signal.Second).Set((byte)(Point(signal.Second).Get() + 1));
    }

    private void Decrement()
    {
        Point(signal.First).Set((byte)(Point(signal.First).Get() - 1));
        if (Point(signal.First).Get() == 0xFF)
            Point(signal.Second).Set((byte)(Point(signal.Second).Get() - 1));
    }

    private void CondCompute()
        => abort = signal.Condition is not 0
                   && !Fru.Check(signal.Condition, new Flags(PointK(PointerK.TMP).Get()));
    
    private ushort Merge(byte low, byte high)
        => (ushort)(low + (high << 8));
    
    public void Clear()
    {
        debugName = "NULL";
        PointK(PointerK.MDR).Set(0);
        PointK(PointerK.TMP).Set(0);
        PointK(PointerK.W).Set(0);
        PointK(PointerK.Z).Set(0);
        flagLatch = 0;
        abort = false;
        logs = [];
    }
}