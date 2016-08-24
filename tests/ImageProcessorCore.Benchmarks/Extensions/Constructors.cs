using System;
using BenchmarkDotNet.Attributes;

namespace ImageProcessorCore.Benchmarks.Extensions
{
    //public class Constructors
    //{
    //    private readonly ImageBase<PixelAccessor, ImageProcessorCore.Color, uint> image = new ImageProcessorCore.Image(200, 200);

    //    [Benchmark(Baseline = true, Description = "Activator")]
    //    public int ActivatorCreate()
    //    {
    //        using (ColorPixelAccessor accessor = (ColorPixelAccessor)Activator.CreateInstance(typeof(ColorPixelAccessor), image))
    //        {
    //            return accessor.Width;
    //        }
    //    }

    //    [Benchmark(Description = "GetInstance")]
    //    public int GetInstanceCreate()
    //    {
    //        using (ColorPixelAccessor accessor = (ColorPixelAccessor)typeof(ColorPixelAccessor).GetInstance(image))
    //        {
    //            return accessor.Width;
    //        }
    //    }
    //}
}
