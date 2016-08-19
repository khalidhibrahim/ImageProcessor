// <copyright file="GlowProcessor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore.Processors
{
    using System;
    using System.Numerics;
    using System.Threading.Tasks;

    /// <summary>
    /// An <see cref="IImageProcessor{T,TC,TP}"/> that applies a radial glow effect an <see cref="Image{T, TC, TP}"/>.
    /// </summary>
    /// <typeparam name="T">The pixel accessor.</typeparam>
    /// <typeparam name="TC">The pixel format.</typeparam>
    /// <typeparam name="TP">The packed format. <example>uint, long, float.</example></typeparam>
    public class GlowProcessor<T, TC, TP> : ImageProcessor<T, TC, TP>
            where T : IPixelAccessor<TC, TP>
            where TC : IPackedVector<TP>
            where TP : struct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlowProcessor{T,TP}"/> class.
        /// </summary>
        public GlowProcessor()
        {
            TC color = default(TC);
            color.PackFromVector4(Color.Black.ToVector4());
            this.GlowColor = color;
        }

        /// <summary>
        /// Gets or sets the glow color to apply.
        /// </summary>
        public TC GlowColor { get; set; }

        /// <summary>
        /// Gets or sets the the radius.
        /// </summary>
        public float Radius { get; set; }

        /// <inheritdoc/>
        protected override void Apply(ImageBase<T, TC, TP> target, ImageBase<T, TC, TP> source, Rectangle targetRectangle, Rectangle sourceRectangle, int startY, int endY)
        {
            int startX = sourceRectangle.X;
            int endX = sourceRectangle.Right;
            TC glowColor = this.GlowColor;
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
                                if (ellipse.Contains(offsetX, offsetY))
                                {
                                    // TODO: Premultiply?
                                    float distance = Vector2.Distance(centre, new Vector2(offsetX, offsetY));
                                    Vector4 sourceColor = sourcePixels[offsetX, offsetY].ToVector4();
                                    TC packed = default(TC);
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

