using System.Runtime.InteropServices;

namespace MyRender
{
    [StructLayout(LayoutKind.Explicit, Size = 12)]
    public struct Vec3
    {
        [FieldOffset(0)] public int X;
        [FieldOffset(1)] public int Y;
        [FieldOffset(2)] public int Z;
    }
}
