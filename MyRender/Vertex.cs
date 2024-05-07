
using System.Numerics;
using System.Runtime.Intrinsics;

namespace MyRender
{
    public sealed class Vertex
    {
        public readonly Vec4 Position;
        public readonly Color Color;
        public readonly Vec2 Texcoord;
        public readonly Vec3 Normal;

        public Vertex(Vec3 position, Color color, Vec2 texcoord, Vec3 normal)
        {
            Position=new Vec4(position,1);
            Color=color;
            Texcoord=texcoord;
            Normal=normal;
        }
    }
}
