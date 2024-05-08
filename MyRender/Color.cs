using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MyRender
{

    [StructLayout(LayoutKind.Explicit,Size =4)]
    public readonly struct Color
    {
        public static readonly Color White = new Color(255, 255, 255);
        public static readonly Color Black = new Color(0, 0, 0);
        public static readonly Color Red = new Color(255, 0, 0);
        public static readonly Color Green = new Color(0, 255, 0);
        public static readonly Color Blue = new Color(0, 0, 255);
        public static readonly Color Yellow = new Color(255, 255, 0);
        public static readonly Color Cyan = new Color(0, 255, 255);
        public static readonly Color Magenta = new Color(255, 0, 255);
        public static readonly Color Grey = new Color(128, 128, 128);
        [FieldOffset(0)] public readonly int Value;
        [FieldOffset(0)] public readonly byte B;
        [FieldOffset(1)] public readonly byte G;
        [FieldOffset(2)] public readonly byte R;
        [FieldOffset(3)] public readonly byte A;
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

        
        //public Vec4 AsVec4=>new Vec4 (B,G,R,A);
    }
    internal static class ColorHelper
    {
        internal static Vector4 AsVector4(this Color  c)
        {
            return new Vector4(c.B, c.G, c.R, c.A);
        }
        internal static Color AsColor(this Vector4 v)
        {
            var B=Convert.ToByte(v.X);
            var G=Convert.ToByte(v.Y);
            var R=Convert.ToByte(v.Z);
            var A=Convert.ToByte(v.W);
            return new Color(A,R,G,B);
        }
    }
}
