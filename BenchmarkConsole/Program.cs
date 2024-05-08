


using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using MyRender;

//FrameBufferTest test= new FrameBufferTest();
//test.Init();
//test.ClearTest();
//test.ClearRedTest();
//test.ResizeToMinTest();
//test.ResizeToMaxTest();
//BenchmarkDotNet.Running.BenchmarkRunner.Run<FrameBufferTest>();
//[MemoryDiagnoser]
//public class FrameBufferTest
//{
//    public FrameBuffer[] arr;

//    public FrameBufferTest()
//    {
//        arr=new FrameBuffer[10];
//        for (int i = 0; i < arr.Length; i++)
//        {
//            arr[i]=new FrameBuffer(1920, 1080);
//        }
//    }

//    //[GlobalSetup]
//    //public void Init()
//    //{
//    //    arr=new FrameBuffer[10];
//    //    for (int i = 0; i < arr.Length; i++)
//    //    {
//    //        arr[i]=new FrameBuffer(1920,1080);
//    //    }
//    //}
//    [Benchmark]
//    public void ClearTest()
//    {
//        foreach (var frameBuffer in arr)
//        {
//            frameBuffer.Clear();
//        }
//    }
//    [Benchmark]
//    public void ClearRedTest()
//    {
//        foreach (var frameBuffer in arr)
//        {
//            frameBuffer.Clear(new Color(255,0,0));
//        }
//    }
//    [Benchmark]
//    public void ResizeToMinTest()
//    {
//        foreach (var frameBuffer in arr)
//        {
//            frameBuffer.Resize(1920/2, 1080/2);
//        }
//    }
//    [Benchmark]
//    public void ResizeToMaxTest()
//    {
//        foreach (var frameBuffer in arr)
//        {
//            frameBuffer.Resize(1920*2, 1080*2);
//        }
//    }
//}