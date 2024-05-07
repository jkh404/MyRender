using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MyRender
{
    [StructLayout(LayoutKind.Explicit, Size = 16)]
    public struct Vec4
    {
        [FieldOffset(0)] public int X;
        [FieldOffset(1)] public int Y;
        [FieldOffset(2)] public int Z;
        [FieldOffset(3)] public int W;

        public Vec4(Vec3 vec3,int w)
        {
            ref byte r0 = ref Unsafe.As<Vec4, byte>(ref this);
            ref byte t0 = ref Unsafe.As<Vec3, byte>(ref vec3);
            Unsafe.CopyBlock(ref r0, ref t0, (uint)Unsafe.SizeOf<Vec3>());
            W = w;
        }
    }
}
