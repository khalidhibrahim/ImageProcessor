// <copyright file="EdgeDetectorFilter.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore.Processors
{
    /// <summary>
    /// Defines a filter that detects edges within an image using a single
    /// two dimensional matrix.
    /// </summary>
    public abstract class EdgeDetectorFilter<T, TC, TP> : ConvolutionFilter<T, TC, TP>, IEdgeDetectorFilter<T, TC, TP>
        where T : IPackedVector<TP>
        where TP : struct
    {
        /// <inheritdoc/>
        public bool Grayscale { get; set; }

        /// <inheritdoc/>
        protected override void OnApply(ImageBase<T, TC, TP> target, ImageBase<T, TC, TP> source, Rectangle targetRectangle, Rectangle sourceRectangle)
        {
            if (this.Grayscale)
            {
                new GrayscaleBt709Processor<T, TC, TP>().Apply(source, source, sourceRectangle);
            }
        }
    }
}
