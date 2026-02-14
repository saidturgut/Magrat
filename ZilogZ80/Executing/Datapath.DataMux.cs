namespace ZilogZ80.Executing;
using Computing;
using Signaling;
using Bounds;

public partial class Datapath
{
    private void RegisterWrite()
        => Point(signal.Second).Set(Point(signal.First).Get());

    private void MemoryRead(IBus bus)
        => Point(Pointer.MDR).Set(bus.Read(Merge(Point(signal.First).Get(), Point(signal.Second).Get()), bus));

    private void MemoryWrite(IBus bus)
        => bus.Write(Merge(Point(signal.First).Get(), Point(signal.Second).Get()), Point(Pointer.MDR).Get(), bus);
    
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

    private void ConditionCompute()
        => stall = signal.Condition is not Condition.NONE
                   && !Fru.Check(signal.Condition, Fru.Flags(Point(Pointer.TMP).Get()));
    
    private ushort Merge(byte low, byte high)
        => (ushort)(low + (high << 8));
    
    public void Clear()
    {
        debugName = "NULL";
        Point(Pointer.MDR).Set(0);
        Point(Pointer.TMP).Set(0);
        Point(Pointer.W).Set(0);
        Point(Pointer.Z).Set(0);
        flagLatch = 0;
        stall = false;
        logs = [];
    }
}