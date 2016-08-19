// <copyright file="Blend.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>-------------------------------------------------------------------------------------------------------------------

namespace ImageProcessorCore
{
    using Processors;

    /// <summary>
    /// Extension methods for the <see cref="Image"/> type.
    /// </summary>
    public static partial class ImageExtensions
    {
        /// <summary>
        /// Combines the given image together with the current one by blending their pixels.
        /// </summary>
        /// <typeparam name="T">The pixel accessor.</typeparam>
        /// <typeparam name="TC">The pixel format.</typeparam>
        /// <typeparam name="TP">The packed format. <example>uint, long, float.</example></typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="image">The image to blend with the currently processing image.</param>
        /// <param name="percent">The opacity of the image image to blend. Must be between 0 and 100.</param>
        /// <param name="progressHandler">A delegate which is called as progress is made processing the image.</param>
        /// <returns>The <see cref="Image{T, TC, TP}"/>.</returns>
        public static Image<T, TC, TP> Blend<T, TC, TP>(this Image<T, TC, TP> source, ImageBase<T, TC, TP> image, int percent = 50, ProgressEventHandler progressHandler = null)
            where T : IPixelAccessor<TC, TP>
            where TC : IPackedVector<TP>
            where TP : struct
        {
            return Blend(source, image, percent, source.Bounds, progressHandler);
        }

        /// <summary>
        /// Combines the given image together with the current one by blending their pixels.
        /// </summary>
        /// <param name="source">The image this method extends.</param>
        /// <param name="image">The image to blend with the currently processing image.</param>
        /// <typeparam name="T">The pixel accessor.</typeparam>
        /// <typeparam name="TC">The pixel format.</typeparam>
        /// <typeparam name="TP">The packed format. <example>uint, long, float.</example></typeparam>
        /// <param name="percent">The opacity of the image image to blend. Must be between 0 and 100.</param>
        /// <param name="rectangle">
        /// The <see cref="Rectangle"/> structure that specifies the portion of the image object to alter.
        /// </param>
        /// <param name="progressHandler">A delegate which is called as progress is made processing the image.</param>
        /// <returns>The <see cref="Image{T, TC, TP}"/>.</returns>
        public static Image<T, TC, TP> Blend<T, TC, TP>(this Image<T, TC, TP> source, ImageBase<T, TC, TP> image, int percent, Rectangle rectangle, ProgressEventHandler progressHandler = null)
            where T : IPixelAccessor<TC, TP>
            where TC : IPackedVector<TP>
            where TP : struct
        {
            BlendProcessor<T, TC, TP> processor = new BlendProcessor<T, TC, TP>(image, percent);
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
