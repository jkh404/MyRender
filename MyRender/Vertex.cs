
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace MyRender
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct Vertex
    {
        public readonly  Vector4 Position;
        public readonly  Color Color;
        public readonly  Vector2 Texcoord;
        public readonly  Vector3 Normal;

        public Vertex(Vector3 position,Color color,Vector2 texcoord,Vector3 normal)
        {
            Position =  new Vector4(position, 1F);
            Color =  color;
            Texcoord =  texcoord;
            Normal =  normal;
        }
        public Vertex(Vector3 position, Color color)
        {
            Position =  new Vector4(position, 1F);
            Color =  color;
            Texcoord =  new Vector2(0,0);
            Normal =  new Vector3(0,0,1);
        }
    }
}
