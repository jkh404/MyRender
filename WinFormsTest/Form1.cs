using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Numerics;
using System.Runtime.CompilerServices;
using MyRender;
using Program;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinFormsTest
{
    public partial class Form1 : Form
    {
        private readonly BufferedGraphicsContext bufferedGraphicsContext;
        private  BufferedGraphics bufferedGraphics;
        private System.Threading.Timer timer;
        Font textFont = new Font("宋体", 10);

        private readonly int width = 1920;
        private readonly int height = 1080;
        private object renderLockObj = new object();
        public Form1()
        {
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(width, height);
            this.Text=$"{this.ClientSize.Width}×{this.ClientSize.Height}";
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer|ControlStyles.ResizeRedraw|ControlStyles.AllPaintingInWmPaint, true);
            bufferedGraphicsContext = new BufferedGraphicsContext();
            var graphics = this.CreateGraphics();
            //graphics.SmoothingMode=SmoothingMode.HighSpeed;
            //graphics.PixelOffsetMode=PixelOffsetMode.HighSpeed;
            bufferedGraphics =bufferedGraphicsContext.Allocate(graphics, this.ClientRectangle);
            this.Load+=Form1_Load;
            this.FormClosing+=Form1_FormClosing;
        }
        private void Form1_Load(object? sender, EventArgs e)
        {
            Thread thread = new Thread(Start);
            thread.IsBackground=true;
            thread.Start();
        }
        private void Start(object? obj)
        {
            Stopwatch stopwatch = new Stopwatch();
            double drawTime = 0;
            double tickTime = 0;
            double Fps = 0;
            timer = new System.Threading.Timer((_) =>
            {
                var Milliseconds = drawTime/10000.0;
                if (Milliseconds==0) Milliseconds=1;
                Fps =(Fps+(1000/Milliseconds))/2;
            }, null, 0, 1000);
            FrameRender frameRender=new FrameRender(width, height);
            frameRender.Init();
            frameRender.Clear(MyRender.Color.White);
            Bitmap bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var gh = bufferedGraphics.Graphics;
            gh.Clear(System.Drawing.Color.White);
            //using CachedBitmap cachedBitmap = new CachedBitmap(bitmap, gh);
            //var v1 = new Vector2(100, 100);
            //var v2 = new Vector2(200, 200);
            //var v3 = new Vector2(0, 200);            
            var model=frameRender.LoadFile("african_head.obj");
            var v1 = new Vertex(new Vector3(-0.5F, -0.5F, 0),MyRender.Color.Red);
            var v2 = new Vertex(new Vector3(0.5F, -0.5F, 0), MyRender.Color.Blue);
            var v3 = new Vertex(new Vector3(0, 0.5F, 0), MyRender.Color.Green) ;
            
            Random random = Random.Shared;
            lock (renderLockObj)
            {
                try
                {
                    while (Visible && bufferedGraphics!=null)
                    {
                        stopwatch.Restart();
                        gh.Clear(System.Drawing.Color.White);


                        //frameRender.Clear(MyRender.Color.White);
                        //frameRender.DrawTriangle(v1,v2,v3,MyRender.Color.Red);
                        //frameRender.FillTriangle(v1, v2, v3);
                        frameRender.ShowModel(model);
                        frameRender.CopyTo(bitmap);

                        gh.DrawImage(bitmap, 0, 0);
                        TextRenderer.DrawText(gh, $"FPS:{Fps:N2}", textFont, Point.Empty, System.Drawing.Color.Black);

                        //this.Invoke(() =>
                        //{
                        //    this.Text=$"{this.ClientSize.Width}×{this.ClientSize.Height},FPS:{Fps:N2}";

                        //});
                        if (tickTime/10000.0>17)
                        {
                            tickTime=0;

                            bufferedGraphics?.Render();

                        }
                        //bufferedGraphics?.Render();
                        stopwatch.Stop();
                        drawTime =stopwatch.ElapsedTicks;
                        tickTime+=drawTime;


                    }
                    Debug.WriteLine("结束");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            
        }
        //private void Start(object? obj)
        //{
        //    Stopwatch stopwatch = new Stopwatch();
        //    double drawTime = 0;
        //    double tickTime = 0;
        //    double Fps = 0;
        //    timer = new System.Threading.Timer((_) => {
        //        var Milliseconds = drawTime/10000.0;
        //        if (Milliseconds==0) Milliseconds=1;
        //        Fps =(Fps+(1000/Milliseconds))/2;
        //    }, null, 0, 1000);

        //    int i = 0;
        //    byte[] color = [0, 0, 0, 255];
        //    ref byte b = ref color[i%3];
        //    ref byte g = ref color[(i+1)%3];
        //    ref byte r = ref color[(i+2)%3];
        //    ref int argb = ref Unsafe.As<byte, int>(ref color[0]);
        //    while (Visible)
        //    {


        //        var gh = bufferedGraphics.Graphics;
        //        stopwatch.Restart();
        //        gh.Clear(Color.FromArgb(argb));
        //        TextRenderer.DrawText(gh, $"FPS:{Fps:N2}", textFont, Point.Empty, Color.Black);
        //        if (tickTime/10000.0>17)
        //        {
        //            tickTime=0;
        //            bufferedGraphics.Render();

        //            r++;
        //            if (r==255)
        //            {
        //                i++;
        //                r= ref color[i%3];
        //                g = ref color[(i+1)%3];
        //                b = ref color[(i+2)%3];
        //            }
        //            if (r==255 && g==255 && b==255)
        //            {
        //                i++;
        //                r=0;
        //                g=0;
        //                b=0;
        //            }
        //        }
        //        stopwatch.Stop();
        //        drawTime =stopwatch.ElapsedTicks;
        //        tickTime+=drawTime;


        //    }
        //}
        protected override void OnBackColorChanged(EventArgs e)
        {

        }
        protected override void OnBackgroundImageChanged(EventArgs e)
        {

        }
        protected override void OnBackgroundImageLayoutChanged(EventArgs e)
        {
        }
        protected override void OnParentBackColorChanged(EventArgs e)
        {

        }
        protected override void OnParentBackgroundImageChanged(EventArgs e)
        {

        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {

        }
        protected override void OnPaint(PaintEventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Visible=false;
            bufferedGraphics?.Dispose();
            bufferedGraphics=null;
            bufferedGraphicsContext?.Dispose();
            e.Cancel=false;
        }
    }
}
