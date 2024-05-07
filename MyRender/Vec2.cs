using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace MyRender
{
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct Vec2
    {
        [FieldOffset(0)] public int X;
        [FieldOffset(1)] public int Y;
    }
}
