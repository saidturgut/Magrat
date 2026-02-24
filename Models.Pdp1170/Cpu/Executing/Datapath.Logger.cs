namespace Models.Pdp1170.Cpu.Executing;

public partial class Datapath
{
    public List<string> Debug()
    {
        ushort flags = Point(Pointer.PSW).Get();
        return [
            $"-> {debugName}",

            $"IR: {Tools.Octal(Point(Pointer.IR).Get())}",
            $"PC: {Tools.Octal(Point(Pointer.PC).Get())}",
            $"SP: {Tools.Octal(Point(Pointer.SP).Get())}",
            
            $"R0: {Tools.Octal(Point(Pointer.R0).Get())}   R1: {Tools.Octal(Point(Pointer.R1).Get())}",
            $"R2: {Tools.Octal(Point(Pointer.R2).Get())}   R3: {Tools.Octal(Point(Pointer.R3).Get())}",
            $"R4: {Tools.Octal(Point(Pointer.R4).Get())}   R5: {Tools.Octal(Point(Pointer.R5).Get())}",
            //$"SRC: {Octal(Point(Pointer.SRC).Get())}   DST: {Octal(Point(Pointer.DST).Get())}",
            
            "CVZN",
            $"{(flags >> 0) & 1}{(flags >> 1) & 1}{(flags >> 2) & 1}{(flags >> 3) & 1}",
            "---------------------"        
        ];
    }
}