// <copyright file="CropProcessor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore.Processors
{
    using System.Threading.Tasks;

    /// <summary>
    /// Provides methods to allow the cropping of an image.
    /// </summary>
    /// <typeparam name="T">The pixel accessor.</typeparam>
    /// <typeparam name="TC">The pixel format.</typeparam>
    /// <typeparam name="TP">The packed format. <example>uint, long, float.</example></typeparam>
    public class CropProcessor<T, TC, TP> : ImageSampler<T, TC, TP>
        where T : IPixelAccessor<TC, TP>
        where TC : IPackedVector<TP>
        where TP : struct
    {
        /// <inheritdoc/>
        protected override void Apply(ImageBase<T, TC, TP> target, ImageBase<T, TC, TP> source, Rectangle targetRectangle, Rectangle sourceRectangle, int startY, int endY)
        {
            int startX = targetRectangle.X;
            int endX = targetRectangle.Right;
            int sourceX = sourceRectangle.X;
            int sourceY = sourceRectangle.Y;

            using (T sourcePixels = source.Lock())
            using (T targetPixels = target.Lock())
            {
                Parallel.For(
                    startY,
                    endY,
                    this.ParallelOptions,
                    y =>
                        {
                            for (int x = startX; x < endX; x++)
                            {
                                targetPixels[x, y] = sourcePixels[x + sourceX, y + sourceY];
                            }

                            this.OnRowProcessed();
                        });
            }
        }
    }
}
