using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MyRender
{
    [StructLayout(LayoutKind.Sequential)]
    public  struct Face
    {
        public  V2F V0;
        public  V2F V1;
        public  V2F V2;

        public Span<V2F> AsSpan()
        {
            return MemoryMarshal.CreateSpan(ref Unsafe.As<Face, V2F>(ref Unsafe.AsRef(ref this)), 3);
        }

    }
}
