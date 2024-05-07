using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MyRender
{

    [StructLayout(LayoutKind.Explicit,Size =4)]
    public record struct Color
    {

        [FieldOffset(0)] public int Value;
        [FieldOffset(0)] public byte B;
        [FieldOffset(1)] public byte G;
        [FieldOffset(2)] public byte R;
        [FieldOffset(3)] public byte A;
        public Color(byte a,  byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
        public Color( byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
            A = byte.MaxValue;
        }
        public Color(int val)
        {
            Value = val;
        }
        public Vec4 AsVec4=>new Vec4 { X=B,Y=G,Z=R,W=A };
    }
}
