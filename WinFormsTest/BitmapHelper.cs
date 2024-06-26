﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MyRender;

namespace Program
{
    internal static class BitmapHelper
    {
        public unsafe static void CopyTo(this FrameRender  frameRender, Bitmap bitmap)
        {
            var Width = Math.Min(frameRender.Width, bitmap.Width);
            var Height = Math.Min(frameRender.Height, bitmap.Height);
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            var pixSize = data.Stride/ bitmap.Width;
            byte* _ptr = (byte*)data.Scan0.ToPointer();
            if (frameRender.Width==bitmap.Width)
            {
                var size = Width * Height * pixSize;
                frameRender.Buffer.Slice(0, size).CopyTo(new Span<byte>(_ptr, size));
            }
            else if (frameRender.Width>bitmap.Width)
            {
                for (int y = 0; y < Height; y++)
                {
                    var offset = y * data.Stride;
                    var Stride = pixSize*Width;
                    frameRender.Buffer.Slice(y*frameRender.Width*4, Stride).CopyTo(new Span<byte>(_ptr+offset, Stride));
                }
            }
            else 
            {
                for (int y = 0; y < Height; y++)
                {
                    var offset = y * data.Stride;
                    var Stride = pixSize*Width;
                    frameRender.Buffer.Slice(y*Stride, Stride).CopyTo(new Span<byte>(_ptr+offset, Stride));
                }
            }
            bitmap.UnlockBits(data);

        }
    }
}
