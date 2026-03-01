namespace Models.Pdp1170.Cpu.Executing;
using Microcodes;

public partial class Datapath
{
    public List<string> Debug()
    {
        ushort flags = PointCore(Pointer.PSW).Get();
        return [
            $"-> {debugName}",

            $"IR: {Tools.Octal(PointCore(Pointer.IR).Get())}",
            $"PC: {Tools.Octal(PointCore(Pointer.PC).Get())}",
            $"SP: {Tools.Octal(Point(Pointer.SP).Get())}",
            
            $"R0: {Tools.Octal(Point(Pointer.R0).Get())}   R1: {Tools.Octal(Point(Pointer.R1).Get())}",
            $"R2: {Tools.Octal(Point(Pointer.R2).Get())}   R3: {Tools.Octal(Point(Pointer.R3).Get())}",
            $"R4: {Tools.Octal(Point(Pointer.R4).Get())}   R5: {Tools.Octal(Point(Pointer.R5).Get())}",
            
            "CVZNT",
            $"{(flags >> 0) & 1}{(flags >> 1) & 1}{(flags >> 2) & 1}{(flags >> 3) & 1}{(flags >> 4) & 1}",
            "---------------------"        
        ];
    }
}