namespace ZilogZ80.Executing.Computing;

public partial class Alu
{
    private static AluOutput NONE(AluInput input) => new()
        { Result = input.A, };

    private static AluOutput RFR(AluInput input) => new();
}