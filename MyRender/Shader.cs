using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;

namespace MyRender
{
    public sealed class Shader
    {

        Matrix4x4 _modelMatrix;
        Matrix4x4 _viewMatrix;
        Matrix4x4 _projectionMatrix;
        Matrix4x4 _mvpMatrix;
        public Matrix4x4 ModelMatrix 
        { 
            internal get => _modelMatrix; 
            set => _modelMatrix = value; 
        }
        public Matrix4x4 ViewMatrix
        {
            internal get => _viewMatrix;
            set => _viewMatrix = value;
        }
        public Matrix4x4 ProjectionMatrix
        {
            internal get => _projectionMatrix;
            set => _projectionMatrix = value;
        }
        public Matrix4x4 MvpMatrix
        {
            internal get => _mvpMatrix;
            set => _mvpMatrix = value;
        }

        public Shader()
        {
            _modelMatrix = Matrix4x4.Identity;
            _viewMatrix = Matrix4x4.Identity;
            _projectionMatrix = Matrix4x4.Identity;
            _mvpMatrix = Matrix4x4.Identity;
        }
        public  V2F VertexShader([In]ref Vertex vertex)
        {
            var worldPos = Vector4.Transform(vertex.Position, _modelMatrix);
            var windowPos = Vector4.Transform(worldPos, _viewMatrix);
            windowPos = Vector4.Transform(windowPos, _projectionMatrix);
            var v2f= new V2F(worldPos, windowPos, vertex.Color.AsVector4(), vertex.Texcoord, vertex.Normal);
            return v2f;
        }
        public Color FragmentShader([In]ref V2F v2f)
        {
            return v2f.Color.AsColor();
        }
    }
}
