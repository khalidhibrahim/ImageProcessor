// <copyright file="BinaryThresholdProcessor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore.Processors
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// An <see cref="IImageProcessor{T,TC,TP}"/> to perform binary threshold filtering against an 
    /// <see cref="Image"/>. The image will be converted to grayscale before thresholding occurs.
    /// </summary>
    /// <typeparam name="T">The pixel format.</typeparam>
    /// <typeparam name="TP">The packed format. <example>long, float.</example></typeparam>
    public class BinaryThresholdProcessor<T, TC, TP> : ImageProcessor<T, TC, TP>
        where T : IPixelAccessor<TC, TP>
        where TC : IPackedVector<TP>
        where TP : struct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryThresholdProcessor{T,TC,TP}"/> class.
        /// </summary>
        /// <param name="threshold">The threshold to split the image. Must be between 0 and 1.</param>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="threshold"/> is less than 0 or is greater than 1.
        /// </exception>
        public BinaryThresholdProcessor(float threshold)
        {
            // TODO: Check limit.
            Guard.MustBeBetweenOrEqualTo(threshold, 0, 1, nameof(threshold));
            this.Value = threshold;

            TC upper = default(TC);
            upper.PackFromVector4(Color.White.ToVector4());
            this.UpperColor = upper;

            TC lower = default(TC);
            lower.PackFromVector4(Color.Black.ToVector4());
            this.LowerColor = lower;
        }

        /// <summary>
        /// Gets the threshold value.
        /// </summary>
        public float Value { get; }

        /// <summary>
        /// Gets or sets the color to use for pixels that are above the threshold.
        /// </summary>
        public TC UpperColor { get; set; }

        /// <summary>
        /// Gets or sets the color to use for pixels that fall below the threshold.
        /// </summary>
        public TC LowerColor { get; set; }

        /// <inheritdoc/>
        protected override void OnApply(ImageBase<T, TC, TP> target, ImageBase<T, TC, TP> source, Rectangle targetRectangle, Rectangle sourceRectangle)
        {
            new GrayscaleBt709Processor<T, TC, TP>().Apply(source, source, sourceRectangle);
        }

        /// <inheritdoc/>
        protected override void Apply(ImageBase<T, TC, TP> target, ImageBase<T, TC, TP> source, Rectangle targetRectangle, Rectangle sourceRectangle, int startY, int endY)
        {
            float threshold = this.Value;
            TC upper = this.UpperColor;
            TC lower = this.LowerColor;
            int startX = sourceRectangle.X;
            int endX = sourceRectangle.Right;

            // Align start/end positions.
            int minX = Math.Max(0, startX);
            int maxX = Math.Min(source.Width, endX);
            int minY = Math.Max(0, startY);
            int maxY = Math.Min(source.Height, endY);

            // Reset offset if necessary.
            if (minX > 0)
            {
                startX = 0;
            }

            if (minY > 0)
            {
                startY = 0;
            }

            using (T sourcePixels = source.Lock())
            using (T targetPixels = target.Lock())
            {
                Parallel.For(
                    minY,
                    maxY,
                    this.ParallelOptions,
                    y =>
                        {
                            int offsetY = y - startY;
                            for (int x = minX; x < maxX; x++)
                            {
                                int offsetX = x - startX;
                                TC color = sourcePixels[offsetX, offsetY];

                                // Any channel will do since it's Grayscale.
                                targetPixels[offsetX, offsetY] = color.ToVector4().X >= threshold ? upper : lower;
                            }

                            this.OnRowProcessed();
                        });
            }
        }
    }
}
