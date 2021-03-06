﻿// <copyright file="GlowProcessor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore.Processors
{
    using System;
    using System.Numerics;
    using System.Threading.Tasks;

    /// <summary>
    /// An <see cref="IImageProcessor{TColor, TPacked}"/> that applies a radial glow effect an <see cref="Image{TColor, TPacked}"/>.
    /// </summary>
    /// <typeparam name="TColor">The pixel format.</typeparam>
    /// <typeparam name="TPacked">The packed format. <example>uint, long, float.</example></typeparam>
    public class GlowProcessor<TColor, TPacked> : ImageProcessor<TColor, TPacked>
            where TColor : IPackedVector<TPacked>
            where TPacked : struct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlowProcessor{T,TP}"/> class.
        /// </summary>
        public GlowProcessor()
        {
            TColor color = default(TColor);
            color.PackFromVector4(Color.Black.ToVector4());
            this.GlowColor = color;
        }

        /// <summary>
        /// Gets or sets the glow color to apply.
        /// </summary>
        public TColor GlowColor { get; set; }

        /// <summary>
        /// Gets or sets the the radius.
        /// </summary>
        public float Radius { get; set; }

        /// <inheritdoc/>
        protected override void Apply(ImageBase<TColor, TPacked> target, ImageBase<TColor, TPacked> source, Rectangle targetRectangle, Rectangle sourceRectangle, int startY, int endY)
        {
            int startX = sourceRectangle.X;
            int endX = sourceRectangle.Right;
            TColor glowColor = this.GlowColor;
            Vector2 centre = Rectangle.Center(sourceRectangle).ToVector2();
            float maxDistance = this.Radius > 0 ? Math.Min(this.Radius, sourceRectangle.Width * .5F) : sourceRectangle.Width * .5F;
            Ellipse ellipse = new Ellipse(new Point(centre), maxDistance, maxDistance);

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

            using (PixelAccessor<TColor, TPacked> sourcePixels = source.Lock())
            using (PixelAccessor<TColor, TPacked> targetPixels = target.Lock())
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
                                if (ellipse.Contains(offsetX, offsetY))
                                {
                                    // TODO: Premultiply?
                                    float distance = Vector2.Distance(centre, new Vector2(offsetX, offsetY));
                                    Vector4 sourceColor = sourcePixels[offsetX, offsetY].ToVector4();
                                    TColor packed = default(TColor);
                                    packed.PackFromVector4(Vector4.Lerp(glowColor.ToVector4(), sourceColor, distance / maxDistance));
                                    targetPixels[offsetX, offsetY] = packed;
                                }
                            }

                            this.OnRowProcessed();
                        });
            }
        }
    }
}

