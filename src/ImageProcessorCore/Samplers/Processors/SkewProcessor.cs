﻿// <copyright file="SkewProcessor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore.Processors
{
    using System.Numerics;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides methods that allow the skewing of images.
    /// </summary>
    /// <typeparam name="T">The pixel accessor.</typeparam>
    /// <typeparam name="TC">The pixel format.</typeparam>
    /// <typeparam name="TP">The packed format. <example>uint, long, float.</example></typeparam>
    public class SkewProcessor<T, TC, TP> : Matrix3x2Processor<T, TC, TP>
        where T : IPixelAccessor<TC, TP>
        where TC : IPackedVector<TP>
        where TP : struct
    {
        /// <summary>
        /// The tranform matrix to apply.
        /// </summary>
        private Matrix3x2 processMatrix;

        /// <summary>
        /// Gets or sets the angle of rotation along the x-axis in degrees.
        /// </summary>
        public float AngleX { get; set; }

        /// <summary>
        /// Gets or sets the angle of rotation along the y-axis in degrees.
        /// </summary>
        public float AngleY { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to expand the canvas to fit the skewed image.
        /// </summary>
        public bool Expand { get; set; } = true;

        /// <inheritdoc/>
        protected override void OnApply(ImageBase<T, TC, TP> target, ImageBase<T, TC, TP> source, Rectangle targetRectangle, Rectangle sourceRectangle)
        {
            this.processMatrix = Point.CreateSkew(new Point(0, 0), -this.AngleX, -this.AngleY);
            if (this.Expand)
            {
                CreateNewTarget(target, sourceRectangle, this.processMatrix);
            }
        }

        /// <inheritdoc/>
        protected override void Apply(ImageBase<T, TC, TP> target, ImageBase<T, TC, TP> source, Rectangle targetRectangle, Rectangle sourceRectangle, int startY, int endY)
        {
            int height = target.Height;
            int width = target.Width;
            Matrix3x2 matrix = GetCenteredMatrix(target, source, this.processMatrix);

            using (T sourcePixels = source.Lock())
            using (T targetPixels = target.Lock())
            {
                Parallel.For(
                    0,
                    height,
                    this.ParallelOptions,
                    y =>
                        {
                            for (int x = 0; x < width; x++)
                            {
                                Point transformedPoint = Point.Skew(new Point(x, y), matrix);
                                if (source.Bounds.Contains(transformedPoint.X, transformedPoint.Y))
                                {
                                    targetPixels[x, y] = sourcePixels[transformedPoint.X, transformedPoint.Y];
                                }
                            }

                            OnRowProcessed();
                        });
            }
        }
    }
}