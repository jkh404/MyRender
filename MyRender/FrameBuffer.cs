using System.Buffers;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MyRender
{
    public sealed class FrameBuffer : IDisposable
    {
        private readonly static ArrayPool<byte> Pool= ArrayPool<byte>.Create();
        public  int Width, Height, Size;
        private int _bufferSize;
        private byte[] _buffer;
        private Color[]? _colorBuffer;
        private bool _isReturn=false;
        public Span<byte> Buffer => _buffer.AsSpan(0, _bufferSize);
        public Span<Color> Colors => _colorBuffer.AsSpan(0, Size);
        public FrameBuffer(int width, int height)
        {
            Width = width;
            Height = height;
            Size = width * height;
            _bufferSize = Size*4;
            _buffer=Pool.Rent(_bufferSize);
            _isReturn=true;
            _InitColor();
        }
        public FrameBuffer(int width, int height, byte[] data)
        {
            Width = width;
            Height = height;
            Size = width * height;
            _bufferSize = Size*4;
            if (data.Length<_bufferSize) throw new ArgumentException("data length is not enough");
            _buffer = data;
            _InitColor();
        }
        private void _InitColor()
        {
            _colorBuffer =Unsafe.As<byte[], Color[]>(ref _buffer);
        }
        public void Clear()
        {
            Array.Clear(_buffer,0, _bufferSize);
        }
        public void Clear(Color color)
        {
            _colorBuffer.AsSpan(0, Size).Fill(color);
        }
        public void Resize(int width, int height)
        {
            if(_isReturn) Pool.Return(_buffer);
            Width = width;
            Height = height;
            Size = width * height;
            _bufferSize = Size*4;
            _buffer=Pool.Rent(_bufferSize);
            _isReturn=true;
            _InitColor();
        }
        public void Dispose()
        {
            if(_isReturn) Pool.Return(_buffer);
        }
        public byte[] ToByteArray()
        {
            return _buffer.AsSpan(0,_bufferSize).ToArray();
        }

        public Color[] ToColorArray()
        {
            return _colorBuffer.AsSpan(0, Size).ToArray();
        }

        public void Write(int x,int y,Color color)
        {
            if(_colorBuffer!=null) _colorBuffer[y*Width+x]=color;
            else
            {
                _buffer[y*Width*4+x*4]=color.B;
                _buffer[y*Width*4+x*4+1]=color.G;
                _buffer[y*Width*4+x*4+2]=color.R;
                _buffer[y*Width*4+x*4+3]=color.A;
            }
        }
        public Color this[int x,int y]
        {
            set
            {
                Write(x,y,value);
            }
            get
            {
                if(_colorBuffer!=null) return _colorBuffer[y*Width+x];
                else
                {
                    return new Color(_buffer[y*Width*4+x*4+3], _buffer[y*Width*4+x*4+2], _buffer[y*Width*4+x*4+1], _buffer[y*Width*4+x*4]);
                }
            }
        }
    }
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
    }
}
