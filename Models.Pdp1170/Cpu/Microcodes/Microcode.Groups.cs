namespace Models.Pdp1170.Cpu.Microcodes;

public partial class Microcode
{
    private static Signal[] TWO_OPERAND(Descriptor input) =>
    [
        ..ADDRESS[input.Modes[0]](input.Regs[0], Pointer.SRC, CheckWidth(input.Width)),
        ..ADDRESS[input.Modes[1]](input.Regs[1], Pointer.DST, CheckWidth(input.Width)),
        ..EXECUTE(input),
        ..WRITEBACK(input),
    ];

    private static Signal[] ONE_OPERAND(Descriptor input) =>
    [
        ..ADDRESS[input.Modes[0]](input.Regs[0], Pointer.DST, CheckWidth(input.Width)),
        ..EXECUTE(input),
        ..WRITEBACK(input),
    ];
    
    private static bool CheckWidth(Width width)
        => width is Width.BYTE;
}