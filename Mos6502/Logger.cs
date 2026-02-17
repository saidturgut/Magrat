namespace Mos6502;
using Kernel.Devices;
using External;
using Decoding;
using Kernel;

public class Logger : ILogger
{
    public List<string> Debug(Register[] registers, string debugName)
    {        
        ushort flags = registers[(byte)Pointer.FR].Get();
        return [
            $"-> {debugName}",

            $"IR: {Log.Hex(registers[(byte)Pointer.IR].Get())}",
            $"PC: {Log.Hex(Merge(registers[(byte)Pointer.PCL].Get(), registers[(byte)Pointer.PCH].Get()))}",
            $"SP: {Log.Hex(registers[(byte)Pointer.SPL].Get())}",
            $"WZ: {Log.Hex(Merge(registers[(byte)Pointer.W].Get(), registers[(byte)Pointer.Z].Get()))}",
            
            $"A: {Log.Hex(registers[(byte)Pointer.A].Get())}",
            $"X: {Log.Hex(registers[(byte)Pointer.IX].Get())}",
            $"Y: {Log.Hex(registers[(byte)Pointer.IY].Get())}",
            $"TMP: {Log.Hex(registers[(byte)Pointer.TMP].Get())}",
            
            "CZIDBVN",
            $"{(flags >> 0) & 1}{(flags >> 1) & 1}{(flags >> 2) & 1}{(flags >> 3) & 1}{(flags >> 4) & 1}{(flags >> 6) & 1}{(flags >> 7) & 1}"

        ];
    }

    private static ushort Merge(byte low, byte high)
        => (ushort)(low + (high << 8));
}