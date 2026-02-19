namespace Kernel.Computing;
using System.Numerics;
using Devices;

public class Alu
{
    protected static bool Carry(int source, byte bit)
        => (byte)((source >> bit) & 1) != 0;
    protected static bool HalfCarryAdd(byte A, byte B, byte C)
        => (A & 0xF) + (B & 0xF) + C > 0xF;
    protected static bool HalfCarrySub(byte A, byte B, byte C)
        => (A & 0xF) < ((B & 0xF) + C);
    protected static bool SignedOverflowAdd(byte A, byte B, byte result)
        => (~(A ^ B) & (A ^ result) & 0x80) != 0;
    protected static bool SignedOverflowSub(byte A, byte B, byte result)
        => (~(A ^ (byte)~B) & (A ^ result) & 0x80) != 0;
    protected static bool EvenParity(byte result)
        => BitOperations.PopCount(result) % 2 == 0;
}