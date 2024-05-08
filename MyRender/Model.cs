using System.Numerics;
using System.Runtime.InteropServices;

namespace MyRender
{
    public sealed class Model
    {
        private  Face[] _faceArr;
        public ReadOnlySpan<Face> Faces => _faceArr;

        internal Model(Face[]  faces)
        {
            _faceArr=faces;
        }
        
    }
}
