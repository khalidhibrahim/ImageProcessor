// <copyright file="IPixelAccessor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore
{
    using System;

    /// <summary>
    /// Encapsulates properties to provides per-pixel access to an images pixels.
    /// </summary>
    /// <typeparam name="TC">The pixel format.</typeparam>
    /// <typeparam name="TP">The packed format. <example>long, float.</example></typeparam>
    public interface IPixelAccessor<TC, TP> : IPixelAccessor
        where TC : IPackedVector<TP>
        where TP : struct
    {
        /// <summary>
        /// Gets or sets the pixel at the specified position.
        /// </summary>
        /// <param name="x">
        /// The x-coordinate of the pixel. Must be greater
        /// than zero and smaller than the width of the pixel.
        /// </param>
        /// <param name="y">
        /// The y-coordinate of the pixel. Must be greater
        /// than zero and smaller than the width of the pixel.
        /// </param>
        /// <returns>The <see cref="TC"/> at the specified position.</returns>
        TC this[int x, int y]
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Encapsulates properties to provides per-pixel access to an images pixels.
    /// </summary>
    public interface IPixelAccessor : IDisposable
    {
        /// <summary>
        /// Gets the width of the image in pixels.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets the height of the image in pixels.
        /// </summary>
        int Height { get; }
    }
}
