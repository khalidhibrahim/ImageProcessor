// <copyright file="Glow.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore
{
    using Processors;

    /// <summary>
    /// Extension methods for the <see cref="Image{T, TC, TP}"/> type.
    /// </summary>
    public static partial class ImageExtensions
    {
        /// <summary>
        /// Applies a radial glow effect to an image.
        /// </summary>
        /// <typeparam name="T">The pixel accessor.</typeparam>
        /// <typeparam name="TC">The pixel format.</typeparam>
        /// <typeparam name="TP">The packed format. <example>uint, long, float.</example></typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="progressHandler">A delegate which is called as progress is made processing the image.</param>
        /// <returns>The <see cref="Image{T, TC, TP}"/>.</returns>
        public static Image<T, TC, TP> Glow<T, TC, TP>(this Image<T, TC, TP> source, ProgressEventHandler progressHandler = null)
            where T : IPixelAccessor<TC, TP>
            where TC : IPackedVector<TP>
            where TP : struct
        {
            return Glow(source, default(TC), source.Bounds.Width * .5F, source.Bounds, progressHandler);
        }

        /// <summary>
        /// Applies a radial glow effect to an image.
        /// </summary>
        /// <typeparam name="T">The pixel accessor.</typeparam>
        /// <typeparam name="TC">The pixel format.</typeparam>
        /// <typeparam name="TP">The packed format. <example>uint, long, float.</example></typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="color">The color to set as the glow.</param>
        /// <param name="progressHandler">A delegate which is called as progress is made processing the image.</param>
        /// <returns>The <see cref="Image{T, TC, TP}"/>.</returns>
        public static Image<T, TC, TP> Glow<T, TC, TP>(this Image<T, TC, TP> source, TC color, ProgressEventHandler progressHandler = null)
            where T : IPixelAccessor<TC, TP>
            where TC : IPackedVector<TP>
            where TP : struct
        {
            return Glow(source, color, source.Bounds.Width * .5F, source.Bounds, progressHandler);
        }

        /// <summary>
        /// Applies a radial glow effect to an image.
        /// </summary>
        /// <typeparam name="T">The pixel accessor.</typeparam>
        /// <typeparam name="TC">The pixel format.</typeparam>
        /// <typeparam name="TP">The packed format. <example>uint, long, float.</example></typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="radius">The the radius.</param>
        /// <param name="progressHandler">A delegate which is called as progress is made processing the image.</param>
        /// <returns>The <see cref="Image{T, TC, TP}"/>.</returns>
        public static Image<T, TC, TP> Glow<T, TC, TP>(this Image<T, TC, TP> source, float radius, ProgressEventHandler progressHandler = null)
            where T : IPixelAccessor<TC, TP>
            where TC : IPackedVector<TP>
            where TP : struct
        {
            return Glow(source, default(TC), radius, source.Bounds, progressHandler);
        }

        /// <summary>
        /// Applies a radial glow effect to an image.
        /// </summary>
        /// <typeparam name="T">The pixel accessor.</typeparam>
        /// <typeparam name="TC">The pixel format.</typeparam>
        /// <typeparam name="TP">The packed format. <example>uint, long, float.</example></typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="rectangle">
        /// The <see cref="Rectangle"/> structure that specifies the portion of the image object to alter.
        /// </param>
        /// <param name="progressHandler">A delegate which is called as progress is made processing the image.</param>
        /// <returns>The <see cref="Image{T, TC, TP}"/>.</returns>
        public static Image<T, TC, TP> Glow<T, TC, TP>(this Image<T, TC, TP> source, Rectangle rectangle, ProgressEventHandler progressHandler = null)
            where T : IPixelAccessor<TC, TP>
            where TC : IPackedVector<TP>
            where TP : struct
        {
            return Glow(source, default(TC), 0, rectangle, progressHandler);
        }

        /// <summary>
        /// Applies a radial glow effect to an image.
        /// </summary>
        /// <typeparam name="T">The pixel accessor.</typeparam>
        /// <typeparam name="TC">The pixel format.</typeparam>
        /// <typeparam name="TP">The packed format. <example>uint, long, float.</example></typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="color">The color to set as the glow.</param>
        /// <param name="radius">The the radius.</param>
        /// <param name="rectangle">
        /// The <see cref="Rectangle"/> structure that specifies the portion of the image object to alter.
        /// </param>
        /// <param name="progressHandler">A delegate which is called as progress is made processing the image.</param>
        /// <returns>The <see cref="Image{T, TC, TP}"/>.</returns>
        public static Image<T, TC, TP> Glow<T, TC, TP>(this Image<T, TC, TP> source, TC color, float radius, Rectangle rectangle, ProgressEventHandler progressHandler = null)
            where T : IPixelAccessor<TC, TP>
            where TC : IPackedVector<TP>
            where TP : struct
        {
            GlowProcessor<T, TC, TP> processor = new GlowProcessor<T, TC, TP> { Radius = radius, };

            if (!color.Equals(default(TC)))
            {
                processor.GlowColor = color;
            }

            processor.OnProgress += progressHandler;

            try
            {
                return source.Process(rectangle, processor);
            }
            finally
            {
                processor.OnProgress -= progressHandler;
            }
        }
    }
}
