// <copyright file="ConvolutionFilter.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore.Processors
{
    using System.Numerics;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines a filter that uses a 2 dimensional matrix to perform convolution against an image.
    /// </summary>
    /// <typeparam name="T">The pixel accessor.</typeparam>
    /// <typeparam name="TC">The pixel format.</typeparam>
    /// <typeparam name="TP">The packed format. <example>uint, long, float.</example></typeparam>
    public abstract class ConvolutionFilter<T, TC, TP> : ImageProcessor<T, TC, TP>
        where T : IPixelAccessor<TC, TP>
        where TC : IPackedVector<TP>
        where TP : struct
    {
        /// <summary>
        /// Gets the 2d gradient operator.
        /// </summary>
        public abstract float[,] KernelXY { get; }

        /// <inheritdoc/>
        protected override void Apply(ImageBase<T, TC, TP> target, ImageBase<T, TC, TP> source, Rectangle targetRectangle, Rectangle sourceRectangle, int startY, int endY)
        {
            float[,] kernelX = this.KernelXY;
            int kernelLength = kernelX.GetLength(0);
            int radius = kernelLength >> 1;

            int sourceY = sourceRectangle.Y;
            int sourceBottom = sourceRectangle.Bottom;
            int startX = sourceRectangle.X;
            int endX = sourceRectangle.Right;
            int maxY = sourceBottom - 1;
            int maxX = endX - 1;

            using (T sourcePixels = source.Lock())
            using (T targetPixels = target.Lock())
            {
                Parallel.For(
                startY,
                endY,
                this.ParallelOptions,
                y =>
                {
                    if (y >= sourceY && y < sourceBottom)
                    {
                        for (int x = startX; x < endX; x++)
                        {
                            float rX = 0;
                            float gX = 0;
                            float bX = 0;

                            // Apply each matrix multiplier to the color components for each pixel.
                            for (int fy = 0; fy < kernelLength; fy++)
                            {
                                int fyr = fy - radius;
                                int offsetY = y + fyr;

                                offsetY = offsetY.Clamp(0, maxY);

                                for (int fx = 0; fx < kernelLength; fx++)
                                {
                                    int fxr = fx - radius;
                                    int offsetX = x + fxr;

                                    offsetX = offsetX.Clamp(0, maxX);

                                    Vector4 currentColor = sourcePixels[offsetX, offsetY].ToVector4();
                                    float r = currentColor.X;
                                    float g = currentColor.Y;
                                    float b = currentColor.Z;

                                    rX += kernelX[fy, fx] * r;
                                    gX += kernelX[fy, fx] * g;
                                    bX += kernelX[fy, fx] * b;
                                }
                            }

                            float red = rX;
                            float green = gX;
                            float blue = bX;

                            Vector4 targetColor = targetPixels[x, y].ToVector4();
                            TC packed = default(TC);
                            packed.PackFromVector4(new Vector4(red, green, blue, targetColor.Z));
                            targetPixels[x, y] = packed;

                        }
                        this.OnRowProcessed();
                    }
                });
            }
        }
    }
}
