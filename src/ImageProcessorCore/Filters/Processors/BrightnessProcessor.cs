// <copyright file="BrightnessProcessor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore.Processors
{
    using System;
    using System.Numerics;
    using System.Threading.Tasks;

    /// <summary>
    /// An <see cref="IImageProcessor{T,TC,TP}"/> to change the brightness of an <see cref="Image{T, TC, TP}"/>.
    /// </summary>
    /// <typeparam name="T">The pixel accessor.</typeparam>
    /// <typeparam name="TC">The pixel format.</typeparam>
    /// <typeparam name="TP">The packed format. <example>uint, long, float.</example></typeparam>
    public class BrightnessProcessor<T, TC, TP> : ImageProcessor<T, TC, TP>
        where T : IPixelAccessor<TC, TP>
        where TC : IPackedVector<TP>
        where TP : struct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BrightnessProcessor{T,TC,TP}"/> class.
        /// </summary>
        /// <param name="brightness">The new brightness of the image. Must be between -100 and 100.</param>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="brightness"/> is less than -100 or is greater than 100.
        /// </exception>
        public BrightnessProcessor(int brightness)
        {
            Guard.MustBeBetweenOrEqualTo(brightness, -100, 100, nameof(brightness));
            this.Value = brightness;
        }

        /// <summary>
        /// Gets the brightness value.
        /// </summary>
        public int Value { get; }

        /// <inheritdoc/>
        protected override void Apply(ImageBase<T, TC, TP> target, ImageBase<T, TC, TP> source, Rectangle targetRectangle, Rectangle sourceRectangle, int startY, int endY)
        {
            float brightness = this.Value / 100F;
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

                                // TODO: Check this with other formats.
                                Vector4 vector = sourcePixels[offsetX, offsetY].ToVector4().Expand();
                                Vector3 transformed = new Vector3(vector.X, vector.Y, vector.Z) + new Vector3(brightness);
                                vector = new Vector4(transformed, vector.W);

                                TC packed = default(TC);
                                packed.PackFromVector4(vector.Compress());

                                targetPixels[offsetX, offsetY] = packed;
                            }

                            this.OnRowProcessed();
                        });
            }
        }
    }
}
