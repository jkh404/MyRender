using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MyRender
{
    public sealed class FrameRender
    {
        FrameBuffer? _frameBuffer;
        Shader? _shader;
        Matrix4x4 _viewPortMatrix;
        int _width;
        int _height;
        public int Width => _width;
        public int Height => _height;
        public Span<byte> Buffer => _frameBuffer!.Buffer;
        public FrameRender(int width, int height)
        {
            _width = width;
            _height = height;

        }
        public void Init()
        {
            _frameBuffer = new FrameBuffer(_width, _height);
            _shader = new Shader();
            
            _viewPortMatrix = GetViewPortMatrix(0, 0, _width, _height);
            _shader.ViewMatrix=_viewPortMatrix;
        }
        public Matrix4x4 GetViewPortMatrix(int ox, int oy, int width, int height)
        {
            return Matrix4x4.CreateScale(width / 2, -height / 2, 1) * Matrix4x4.CreateTranslation(ox + width / 2, oy + height / 2, 0);
        }
        public void Resize(int w,int h)
        {
            _width=w;
            _height=h;
            _frameBuffer!.Resize(w,h);
            _viewPortMatrix = GetViewPortMatrix(0, 0, _width, _height);
            _shader.ViewMatrix=_viewPortMatrix;
        }
        public void DrawLine(float x1, float y1, float x2, float y2, Color color, int lineWidth = 1)
        {
            DrawLine(
                Convert.ToInt32(x1),
                Convert.ToInt32(y1),
                Convert.ToInt32(x2),
                Convert.ToInt32(y2), color
                );
        }
        private void DrawLine(int x0, int y0, int x1, int y1, Color color)
        {
            bool steep = false;
            if (Math.Abs(x0-x1)<Math.Abs(y0-y1))
            {
                (x0, y0)=(y0, x0);
                (x1, y1)=(y1, x1);
                steep = true;
            }
            if (x0>x1)
            {
                (x0, x1)=(x1, x0);
                (y0, y1)=(y1, y0);
            }
            int dx = x1-x0;
            int dy = y1-y0;
            int derror2 = Math.Abs(dy)*2;
            int error2 = 0;
            int y = y0;
            for (int x = x0; x<=x1; x++)
            {
                if (steep)
                {
                    _frameBuffer!.Write(y, x, ref color);
                }
                else
                {
                    _frameBuffer!.Write(x, y, ref color);
                }
                error2 += derror2;
                if (error2 > dx)
                {
                    y += (y1>y0 ? 1 : -1);
                    error2 -= dx*2;
                }
            }
        }
        public void DrawLine(int x1, int y1, int x2, int y2, Color color, int lineWidth)
        {
            if (x1<0 || x1>Width) return;
            if (x2<0 || x2>Width) return;
            if (y1<0 || y1>Height) return;
            if (y2<0 || y2>Height) return;
            if (lineWidth<1) return;
            int dx = x2 - x1;  // 计算 x 方向上的增量
            int dy = y2 - y1;  // 计算 y 方向上的增量
            int ux = (dx > 0 ? 1 : -1);  // 确定 x 方向上的步进
            int uy = (dy > 0 ? 1 : -1);  // 确定 y 方向上的步进
            int x = x1, y = y1, eps;
            dx = Math.Abs(dx);  // 计算 dx 的绝对值
            dy = Math.Abs(dy);  // 计算 dy 的绝对值
            if (dx > dy)  // 判断直线在 x 方向上增长得更快还是在 y 方向上增长得更快
            {
                eps = dx / 2;
                for (int i = 0; i < dx; i++)
                {
                    _frameBuffer!.Write(x, y, ref color);  // 绘制像素点
                    if (lineWidth>1) for (int j = 0; j < lineWidth; j++) _frameBuffer!.Write(x, y+j, ref color);  // 绘制像素点
                    x += ux;  // 更新 x 的值
                    eps += dy;
                    if (eps > dx)
                    {
                        eps -= dx;
                        y += uy;  // 更新 y 的值
                    }
                }
            }
            else
            {
                eps = dy / 2;
                for (int i = 0; i < dy; i++)
                {
                    _frameBuffer!.Write(x, y, ref color);  // 绘制像素点
                    if (lineWidth>1) for (int j = 0; j < lineWidth; j++) _frameBuffer!.Write(x+j, y, ref color);  // 绘制像素点
                    y += uy;  // 更新 y 的值
                    eps += dx;
                    if (eps > dy)
                    {
                        eps -= dy;
                        x += ux;  // 更新 x 的值
                    }
                }
            }
        }
        public void DrawLine(Vector2 start, Vector2 end, Color color, int lineWidth = 1)
        {
            var x1 = Convert.ToInt32(start.X);
            var x2 = Convert.ToInt32(end.X);
            var y1 = Convert.ToInt32(start.Y);
            var y2 = Convert.ToInt32(end.Y);
            DrawLine(x1, y1, x2, y2, color, lineWidth);
        }
        public void DrawTriangle(Vector2 v0, Vector2 v1, Vector2 v2, Color color, int lineWidth = 1)
        {
            DrawLine(v0, v1, color, lineWidth);
            DrawLine(v0, v2, color, lineWidth);
            DrawLine(v1, v2, color, lineWidth);
        }
        public void DrawTriangle(V2F v0, V2F v1, V2F v2, Color color, int lineWidth = 1)
        {
            DrawLine(v0.WindowPos.X, v0.WindowPos.Y, v1.WindowPos.X, v1.WindowPos.Y, color, lineWidth);
            DrawLine(v1.WindowPos.X, v1.WindowPos.Y, v2.WindowPos.X, v2.WindowPos.Y, color, lineWidth);
            DrawLine(v2.WindowPos.X, v2.WindowPos.Y, v0.WindowPos.X, v0.WindowPos.Y, color, lineWidth);
        }
        public void Clear(Color color)
        {
            _frameBuffer!.Clear(color);
        }
        public void FillTriangle(Vertex v1,Vertex v2,Vertex v3)
        {
            V2F o1 = _shader!.VertexShader(ref v1);
            V2F o2 = _shader!.VertexShader(ref v2);
            V2F o3 = _shader!.VertexShader(ref v3);
            //o1.WindowPos=Vector4.Transform(o1.WindowPos, _viewPortMatrix);
            //o2.WindowPos=Vector4.Transform(o2.WindowPos, _viewPortMatrix);
            //o3.WindowPos=Vector4.Transform(o3.WindowPos, _viewPortMatrix);
            ScanLineTriangle(o1, o2, o3);
        }

        
        public void ScanLineTriangle(V2F v1, V2F v2, V2F v3)
        {

            if (v1.WindowPos.Y > v2.WindowPos.Y)
            {
                (v1,v2)=(v2,v1);
            }
            if (v2.WindowPos.Y > v3.WindowPos.Y)
            {
                (v2, v3)=(v3, v2);
            }
            if (v1.WindowPos.Y > v2.WindowPos.Y)
            {
                (v1, v2)=(v2, v1);
            }
            //v1 在最下面  v3在最上面
            //中间跟上面的相等，是底三角形
            if (v2.WindowPos.Y== v3.WindowPos.Y)
            {
                DownTriangle(v2, v3, v1);
            }//顶三角形
            else if (v2.WindowPos.Y==v1.WindowPos.Y)
            {
                UpTriangle(v2, v1, v3);
            }
            else
            {
                //插值求出中间点对面的那个点，划分为两个新的三角形
                float weight = (v3.WindowPos.Y - v2.WindowPos.Y) / (v3.WindowPos.Y - v1.WindowPos.Y);
                V2F newEdge = V2F.Lerp(v3, v1, weight);
                UpTriangle(v2, newEdge, v3);
                DownTriangle(v2, newEdge, v1);
            }

        }

        private void UpTriangle(V2F v1, V2F v2, V2F v3)
        {
            V2F left, right, top;
            left = v1.WindowPos.X > v2.WindowPos.X ? v2 : v1;
            right = v1.WindowPos.X > v2.WindowPos.X ? v1 : v2;
            top = v3;
            left.WindowPos.X = Convert.ToInt32(left.WindowPos.X);
            float dy = top.WindowPos.Y - left.WindowPos.Y;
            float nowY = top.WindowPos.Y;
            //从上往下插值
            for (int i = (int)dy; i >= 0; i--)
            {
                float weight = 0;
                if (dy != 0)
                {
                    weight = i / dy;
                }
                V2F newLeft = V2F.Lerp(left, top, weight);
                V2F newRight = V2F.Lerp(right, top, weight);
                newLeft.WindowPos.X = Convert.ToInt32(newLeft.WindowPos.X);
                newRight.WindowPos.X = Convert.ToInt32(newRight.WindowPos.X+0.5);
                newLeft.WindowPos.Y = newRight.WindowPos.Y = nowY;
                ScanLine(newLeft, newRight);
                nowY--;
            }
        }

        private void DownTriangle(V2F v1, V2F v2, V2F v3)
        {
            V2F left, right, bottom;
            left = v1.WindowPos.X> v2.WindowPos.X ? v2 : v1;
            right = v1.WindowPos.X > v2.WindowPos.X ? v1 : v2;
            bottom = v3;
            int dy = Convert.ToInt32(left.WindowPos.Y - bottom.WindowPos.Y);
            float nowY = left.WindowPos.Y;
            //从上往下插值
            for (float i = 0; i < dy; i++)
            {
                float weight = 0;
                if (dy != 0) weight = i / dy;
                V2F newLeft = V2F.Lerp(left, bottom, weight);
                V2F newRight = V2F.Lerp(right, bottom, weight);
                newLeft.WindowPos.X = Convert.ToInt32(newLeft.WindowPos.X);
                newRight.WindowPos.X = Convert.ToInt32(newRight.WindowPos.X+0.5);
                newLeft.WindowPos.Y = newRight.WindowPos.Y = nowY;
                ScanLine(newLeft, newRight);
                nowY--;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ScanLine(V2F left, V2F right)
        {
            float length = right.WindowPos.X - left.WindowPos.X;
            for (int i = 0; i < length; i++)
            {
                //V2F v = V2F.Lerp(left, right, i / length);
                //v.WindowPos.X = left.WindowPos.X + i;
                //v.WindowPos.Y = left.WindowPos.Y;
                //Color color = _shader!.FragmentShader(ref v);
                //_frameBuffer!.Write((int)v.WindowPos.X, (int)v.WindowPos.Y, ref color);
                var factor = i / length;
                Vector4 windowPos = Vector4.Lerp(left.WindowPos, right.WindowPos, factor);
                Color color = Vector4.Lerp(left.Color, right.Color, factor).AsColor();
                _frameBuffer!.Write(Convert.ToInt32(windowPos.X), Convert.ToInt32(windowPos.Y), ref color);
            }

        }
        public void ShowModel(Model model)
        {

            var parallelReuslt = Parallel.For(0, model.Faces.Length, i =>
            {
                var face = model.Faces[i];
                //DrawTriangle(face.V0, face.V1, face.V2, Color.Red, 1);
                ScanLineTriangle(face.V0, face.V1, face.V2);
            });

            //foreach (var face in model.Faces)
            //{
            //    var o1 = face.V0;
            //    var o2 = face.V1;
            //    var o3 = face.V2;
            //    //_viewPortMatrix=GetViewPortMatrix(0, 0, 1000, 1000);

            //    //o1.WindowPos=Vector4.Transform(o1.WindowPos, _viewPortMatrix);
            //    //o2.WindowPos=Vector4.Transform(o2.WindowPos, _viewPortMatrix);
            //    //o3.WindowPos=Vector4.Transform(o3.WindowPos, _viewPortMatrix);
            //    //DrawTriangle(o1, o2, o3, Color.Red, 1);
            //    ScanLineTriangle(o1, o2, o3);
            //}
        }

        public Model LoadFile(string path)
        {

            if (File.Exists(path))
            {
                List<Vertex> vList = new List<Vertex>();
                List<Face> fList = new List<Face>();
                using var reader = File.OpenText(path);
                ReadOnlySpan<char> text = null;
                Span<Range> ranges0 = stackalloc Range[3];//空格分割
                Span<Range> ranges1 = stackalloc Range[3];//斜杠分割
                do
                {
                    text=reader.ReadLine();
                    if (text==null) break;
                    if (text.Length<=0) continue;
                    var frist = text[0];
                    if (frist=='#' || frist==' '|| frist=='\0') continue;
                    var startChar = text[0..2];
                    int count = 0;
                    var valText = text[2..];
                    switch (startChar)
                    {
                        case "v ":

                            Vector3 pos = new Vector3();
                            count=valText.Split(ranges0, ' ');
                            for (int i = 0; i < count; i++)
                            {
                                float.TryParse(valText[ranges0[i]], out var result);
                                pos[i]=result;
                            }
                            Vertex v = new Vertex(pos, Color.Blue);
                            vList.Add(v);
                            continue;
                        case "f ":
                            Face face = new Face();
                            count=valText.Split(ranges0, ' ');
                            V2F v2F = new V2F();
                            for (int i = 0; i < count; i++)
                            {
                                valText[ranges0[i]].Split(ranges1, '/');
                                if (int.TryParse(valText[ranges0[i]][ranges1[0]], out var vIndex))
                                {
                                    var v0 = vList[vIndex-1];
                                    face.AsSpan()[i]=_shader!.VertexShader(ref v0);
                                }
                            }
                            var color = new Color((byte)Random.Shared.Next(0, 255), (byte)Random.Shared.Next(0, 255), (byte)Random.Shared.Next(0, 255)).AsVector4();
                            face.V0.Color=color;
                            face.V1.Color=color;
                            face.V2.Color=color;
                            fList.Add(face);
                            continue;
                        default:
                            continue;
                    }

                } while (true);
                Model model=new Model(fList.ToArray());
                return model;
            }
            else
            {
                throw new FileNotFoundException();
            }
            return null;
        }

        //public void FillTriangle(Vertex v1, Vertex v2, Vertex v3)
        //{
        //    V2F o1 = _shader!.VertexShader(ref v1);
        //    V2F o2 = _shader!.VertexShader(ref v2);
        //    V2F o3 = _shader!.VertexShader(ref v3);
        //    o1.WindowPos=Vector4.Transform(o1.WindowPos, _viewPortMatrix);
        //    o2.WindowPos=Vector4.Transform(o2.WindowPos, _viewPortMatrix);
        //    o3.WindowPos=Vector4.Transform(o3.WindowPos, _viewPortMatrix);
        //    ScanLineTriangle(o1, o2, o3);
        //}
        //public void ScanLineTriangle(V2F v1, V2F v2, V2F v3)
        //{
        //    float minX = MathF.Min(v1.WindowPos.X, MathF.Min(v2.WindowPos.X, v3.WindowPos.X));
        //    float maxX = MathF.Max(v1.WindowPos.X, MathF.Max(v2.WindowPos.X, v3.WindowPos.X));
        //    float minY = MathF.Min(v1.WindowPos.Y, MathF.Min(v2.WindowPos.Y, v3.WindowPos.Y));
        //    float maxY = MathF.Max(v1.WindowPos.Y, MathF.Max(v2.WindowPos.Y, v3.WindowPos.Y));

        //    Vector2 boundingBoxMin = new Vector2(minX, minY);
        //    Vector2 boundingBoxMax = new Vector2(maxX, maxY);

        //    for (float y = boundingBoxMin.Y; y < boundingBoxMax.Y; y++)
        //    {
        //        for (float x = boundingBoxMin.X; x < boundingBoxMax.X; x++)
        //        {
        //            Vector2 p = new Vector2(x, y);
        //            Vector4 coord = Barycentric(ref v1.WindowPos, ref v2.WindowPos, ref v3.WindowPos, ref p);

        //            if (coord.X < 0 || coord.Y < 0 || coord.Z < 0)
        //                continue;

        //            int pixelX = (int)x;
        //            int pixelY = (int)y;

        //            float weight1 = x / (maxX - minX);
        //            float weight2 = y / (maxY - minY);

        //            //Vector4 newColor = Vector4.Lerp(v1.Color, v2.Color, weight1);
        //            //newColor = Vector4.Lerp(newColor, v3.Color, weight2);
        //            Color color = v1.Color.AsColor();

        //            _frameBuffer!.Write(pixelX, pixelY, ref color);
        //        }
        //    }
        //}
        //private static Vector4 Barycentric([In]ref Vector4 A, [In] ref Vector4 B, [In] ref Vector4 C, [In] ref Vector2 P)
        //{
        //    var x0 = new Vector3(B.X,C.X,A.X);
        //    var y0 = new Vector3(B.Y,C.Y,A.Y);
        //    var x1 = new Vector3(A.X,A.X,P.X);
        //    var y1 = new Vector3(A.Y,A.Y,P.Y);
        //    Vector3 Coord = Cross(x0-x1, y0-y1);
        //    if (Math.Abs(Coord.Z)<1)
        //    {
        //        return new Vector4(-1F, 1F, 1F,1);
        //    }
        //    else
        //    {
        //        return new Vector4(1F - (Coord.X + Coord.Y) / Coord.Z, Coord.X / Coord.Z, Coord.Y / Coord.Z,1);
        //    }
        //}
        //private static Vector3 Cross(Vector3 A, Vector3 B)
        //{
        //    var a0 = new Vector3(A.Y, A.Z, A.X);
        //    var b0 = new Vector3(B.Z, B.X, B.Y);
        //    var a1 = new Vector3(A.Z, A.X, A.Y);
        //    var b1 = new Vector3(B.Y, B.Z, B.X);
        //    return (a0* b0) - (a1 * b1);
        //}

    }
}
