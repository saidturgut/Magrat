namespace Mos6502.Executing;
using Signaling;
using External;
using Bounds;

public partial class Datapath
{
    private string debugName = "NULL";
    
    public void Debug()
    {
        logs.Add($"-> {debugName}");
        logs.Add($"IR: {Log.Hex(DebugGet(Pointer.IR))}");
        logs.Add($"PC: {Log.Hex(Merge(DebugGet(Pointer.PCL), DebugGet(Pointer.PCH)))}");
        logs.Add($"SP: {Log.Hex(DebugGet(Pointer.SPL))}");
        //logs.Add($"WZ: {Convert.Hex(Merge(DebugGet(Pointer.WL), DebugGet(Pointer.ZL)))}");
        
        logs.Add($"A: {Log.Hex(DebugGet(Pointer.A))}");
        logs.Add($"X: {Log.Hex(DebugGet(Pointer.IX))}");
        logs.Add($"Y: {Log.Hex(DebugGet(Pointer.IY))}");
        //logs.Add($"TMP: {Convert.Hex(DebugGet(Pointer.TMP))}");
        
        ushort flags = DebugGet(Pointer.F);
        logs.Add("CZIDBVN");
        logs.Add($"{(flags >> 0) & 1}{(flags >> 1) & 1}{(flags >> 2) & 1}{(flags >> 3) & 1}{(flags >> 4) & 1}{(flags >> 6) & 1}{(flags >> 7) & 1}");
    }
    
    private byte DebugGet(Pointer pointer)
        => Point(pointer).Get();
}