using System.Numerics;
using System.Runtime.InteropServices;

namespace MyRender
{
    [StructLayout(LayoutKind.Sequential)]
    public  struct V2F
    {
        public  Vector4 WorldPos;
        public  Vector4 WindowPos;
        public  Vector4 Color;
        public  Vector2 Texcoord;
        public  Vector3 Normal;
        public V2F( Vector4 worldPos,  Vector4 windowPos,  Vector4 color,  Vector2 texcoord,  Vector3 normal)
        {
            WorldPos = worldPos;
            WindowPos = windowPos;
            Color = color;
            Texcoord = texcoord;
            Normal = normal;
        }
        public static V2F Lerp(in V2F a, in V2F b, float factor)
        {
            Vector4 worldPos = Vector4.Lerp(a.WorldPos, b.WorldPos, factor);
            Vector4 windowPos = Vector4.Lerp(a.WindowPos, b.WindowPos, factor);
            Vector4 color = Vector4.Lerp(a.Color, b.Color, factor);
            Vector2 texcoord = Vector2.Lerp(a.Texcoord, b.Texcoord, factor);
            Vector3 normal = Vector3.Lerp(a.Normal, b.Normal, factor);
            return new V2F(
                worldPos,
                windowPos,
                color,
                texcoord,
                normal
                );
        }
    }
}
