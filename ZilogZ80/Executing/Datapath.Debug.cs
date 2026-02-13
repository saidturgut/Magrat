namespace ZilogZ80.Executing;
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
        logs.Add($"SP: {Log.Hex(Merge(DebugGet(Pointer.SPL), DebugGet(Pointer.SPH)))}");
        logs.Add($"HL: {Log.Hex(Merge(DebugGet(Pointer.L), DebugGet(Pointer.H)))}");
        logs.Add($"IX: {Log.Hex(Merge(DebugGet(Pointer.IXL), DebugGet(Pointer.IXH)))}");
        logs.Add($"IY: {Log.Hex(Merge(DebugGet(Pointer.IYL), DebugGet(Pointer.IYH)))}");
        
        logs.Add($"A: {Log.Hex(DebugGet(Pointer.A))}");
        logs.Add($"B: {Log.Hex(DebugGet(Pointer.B))}   C: {Log.Hex(DebugGet(Pointer.C))}");
        logs.Add($"D: {Log.Hex(DebugGet(Pointer.D))}   E: {Log.Hex(DebugGet(Pointer.E))}");
        
        logs.Add($"V: {Log.Hex(DebugGet(Pointer.VR))}   R: {Log.Hex(DebugGet(Pointer.RR))}");
        
        ushort flags = DebugGet(Pointer.F);
        logs.Add("CNVHZS");
        logs.Add($"{(flags >> 0) & 1}{(flags >> 1) & 1}{(flags >> 2) & 1}{(flags >> 4) & 1}{(flags >> 6) & 1}{(flags >> 7) & 1}");
    }
    
    private byte DebugGet(Pointer pointer)
        => Point(pointer).Get();
}