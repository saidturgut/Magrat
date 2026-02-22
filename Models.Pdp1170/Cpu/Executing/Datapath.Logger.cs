namespace Models.Pdp1170.Cpu.Executing;

public partial class Datapath
{
    public List<string> Debug()
    {
        ushort flags = Point(Pointer.PSW).Get();
        return [
            $"-> {debugName}",

            $"IR: {Octal(Point(Pointer.IR).Get())}",
            $"PC: {Octal(Point(Pointer.PC).Get())}",
            $"SP: {Octal(Point(Pointer.SP).Get())}",
            
            $"R0: {Octal(Point(Pointer.R0).Get())}   R1: {Octal(Point(Pointer.R1).Get())}",
            $"R2: {Octal(Point(Pointer.R2).Get())}   R3: {Octal(Point(Pointer.R3).Get())}",
            $"R4: {Octal(Point(Pointer.R4).Get())}   R5: {Octal(Point(Pointer.R5).Get())}",
            //$"SRC: {Octal(Point(Pointer.SRC).Get())}   DST: {Octal(Point(Pointer.DST).Get())}",
            
            "CVZN",
            $"{(flags >> 0) & 1}{(flags >> 1) & 1}{(flags >> 2) & 1}{(flags >> 3) & 1}",
            "---------------------"        
        ];
    }
    
    private static string Octal(uint input)         
        => $"0{Convert.ToString(input, 8).ToUpper()}";
}