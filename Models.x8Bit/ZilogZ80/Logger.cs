namespace Models.x8Bit.ZilogZ80;
using Engine.Units;
using Engine.Utility;
using Decoding;
using Engine;

public class Logger : ILogger
{
    public List<string> Debug(Register[] registers, string debugName)
    {        
        ushort flags = registers[(byte)Pointer.FR].Get();
        return [
            $"-> {debugName}",

            $"IR: {Log.Hex(registers[(byte)Pointer.IR].Get())}",
            $"PC: {Log.Hex(Merge(registers[(byte)Pointer.PCL].Get(), registers[(byte)Pointer.PCH].Get()))}",
            $"SP: {Log.Hex(Merge(registers[(byte)Pointer.SPL].Get(), registers[(byte)Pointer.SPH].Get()))}",
            $"HL: {Log.Hex(Merge(registers[(byte)Pointer.L].Get(), registers[(byte)Pointer.H].Get()))}",
            $"IX: {Log.Hex(Merge(registers[(byte)Pointer.IXL].Get(), registers[(byte)Pointer.IXH].Get()))}",
            $"IY: {Log.Hex(Merge(registers[(byte)Pointer.IYL].Get(), registers[(byte)Pointer.IYH].Get()))}",
            
            $"A: {Log.Hex(registers[(byte)Pointer.A].Get())}",
            $"B: {Log.Hex(registers[(byte)Pointer.B].Get())}   C: {Log.Hex(registers[(byte)Pointer.C].Get())}",
            $"D: {Log.Hex(registers[(byte)Pointer.D].Get())}   E: {Log.Hex(registers[(byte)Pointer.E].Get())}",
            
            $"I: {Log.Hex(registers[(byte)Pointer.I].Get())}   R: {Log.Hex(registers[(byte)Pointer.R].Get())}",
            
            "CNVHZS",
            $"{(flags >> 0) & 1}{(flags >> 1) & 1}{(flags >> 2) & 1}{(flags >> 4) & 1}{(flags >> 6) & 1}{(flags >> 7) & 1}"
        ];
    }

    private static ushort Merge(byte low, byte high)
        => (ushort)(low + (high << 8));

}