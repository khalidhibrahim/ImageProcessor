// <copyright file="Hue.cs" company="James Jackson-South">
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
        /// Alters the hue component of the image.
        /// </summary>
        /// <typeparam name="T">The pixel accessor.</typeparam>
        /// <typeparam name="TC">The pixel format.</typeparam>
        /// <typeparam name="TP">The packed format. <example>uint, long, float.</example></typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="degrees">The angle in degrees to adjust the image.</param>
        /// <param name="progressHandler">A delegate which is called as progress is made processing the image.</param>
        /// <returns>The <see cref="Image{T, TC, TP}"/>.</returns>
        public static Image<T, TC, TP> Hue<T, TC, TP>(this Image<T, TC, TP> source, float degrees, ProgressEventHandler progressHandler = null)
            where T : IPixelAccessor<TC, TP>
            where TC : IPackedVector<TP>
            where TP : struct
        {
            return Hue(source, degrees, source.Bounds, progressHandler);
        }

        /// <summary>
        /// Alters the hue component of the image.
        /// </summary>
        /// <typeparam name="T">The pixel accessor.</typeparam>
        /// <typeparam name="TC">The pixel format.</typeparam>
        /// <typeparam name="TP">The packed format. <example>uint, long, float.</example></typeparam>
        /// <param name="source">The image this method extends.</param>
        /// <param name="degrees">The angle in degrees to adjust the image.</param>
        /// <param name="rectangle">
        /// The <see cref="Rectangle"/> structure that specifies the portion of the image object to alter.
        /// </param>
        /// <param name="progressHandler">A delegate which is called as progress is made processing the image.</param>
        /// <returns>The <see cref="Image{T, TC, TP}"/>.</returns>
        public static Image<T, TC, TP> Hue<T, TC, TP>(this Image<T, TC, TP> source, float degrees, Rectangle rectangle, ProgressEventHandler progressHandler = null)
            where T : IPixelAccessor<TC, TP>
            where TC : IPackedVector<TP>
            where TP : struct
        {
            HueProcessor<T, TC, TP> processor = new HueProcessor<T, TC, TP>(degrees);
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
