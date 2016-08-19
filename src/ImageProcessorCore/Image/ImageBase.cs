// <copyright file="ImageBase.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// The base class of all images. Encapsulates the basic properties and methods required to manipulate 
    /// images in different pixel formats.
    /// </summary>
    /// <typeparam name="T">The pixel accessor.</typeparam>
    /// <typeparam name="TC">The pixel format.</typeparam>
    /// <typeparam name="TP">The packed format. <example>uint, long, float.</example></typeparam>
    [DebuggerDisplay("Image: {Width}x{Height}")]
    public abstract class ImageBase<T, TC, TP> : IImageBase<T, TC, TP>
        where T : IPixelAccessor<TC, TP>
        where TC : IPackedVector<TP>
        where TP : struct
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageBase{T,TC,TP}"/> class.
        /// </summary>
        protected ImageBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageBase{T,TC,TP}"/> class.
        /// </summary>
        /// <param name="width">The width of the image in pixels.</param>
        /// <param name="height">The height of the image in pixels.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown if either <paramref name="width"/> or <paramref name="height"/> are less than or equal to 0.
        /// </exception>
        protected ImageBase(int width, int height)
        {
            Guard.MustBeGreaterThan(width, 0, nameof(width));
            Guard.MustBeGreaterThan(height, 0, nameof(height));

            this.Width = width;
            this.Height = height;
            this.Pixels = new TC[width * height];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageBase{T,TC,TP}"/> class.
        /// </summary>
        /// <param name="other">
        /// The other <see cref="ImageBase{T,TC,TP}"/> to create this instance from.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown if the given <see cref="ImageBase{T,TC,TP}"/> is null.
        /// </exception>
        protected ImageBase(ImageBase<T, TC, TP> other)
        {
            Guard.NotNull(other, nameof(other), "Other image cannot be null.");

            this.Width = other.Width;
            this.Height = other.Height;
            this.CopyProperties(other);

            // Copy the pixels.
            this.Pixels = new TC[this.Width * this.Height];
            Array.Copy(other.Pixels, this.Pixels, other.Pixels.Length);
        }

        /// <inheritdoc/>
        public int MaxWidth { get; set; } = int.MaxValue;

        /// <inheritdoc/>
        public int MaxHeight { get; set; } = int.MaxValue;

        /// <inheritdoc/>
        public TC[] Pixels { get; private set; }

        /// <inheritdoc/>
        public int Width { get; private set; }

        /// <inheritdoc/>
        public int Height { get; private set; }

        /// <inheritdoc/>
        public double PixelRatio => (double)this.Width / this.Height;

        /// <inheritdoc/>
        public Rectangle Bounds => new Rectangle(0, 0, this.Width, this.Height);

        /// <inheritdoc/>
        public int Quality { get; set; }

        /// <inheritdoc/>
        public int FrameDelay { get; set; }

        /// <inheritdoc/>
        public void SetPixels(int width, int height, TC[] pixels)
        {
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width), "Width must be greater than or equals than zero.");
            }

            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height), "Height must be greater than or equal than zero.");
            }

            if (pixels.Length != width * height)
            {
                throw new ArgumentException("Pixel array must have the length of Width * Height.");
            }

            this.Width = width;
            this.Height = height;
            this.Pixels = pixels;
        }

        /// <inheritdoc/>
        public void ClonePixels(int width, int height, TC[] pixels)
        {
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width), "Width must be greater than or equals than zero.");
            }

            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height), "Height must be greater than or equal than zero.");
            }

            if (pixels.Length != width * height)
            {
                throw new ArgumentException("Pixel array must have the length of Width * Height.");
            }

            this.Width = width;
            this.Height = height;

            // Copy the pixels.
            this.Pixels = new TC[pixels.Length];
            Array.Copy(pixels, this.Pixels, pixels.Length);
        }

        /// <inheritdoc/>
        public T Lock()
        {
            return (T)Activator.CreateInstance(typeof(T), this);
        }

        /// <summary>
        /// Copies the properties from the other <see cref="ImageBase{T,TC,TP}"/>.
        /// </summary>
        /// <param name="other">
        /// The other <see cref="ImageBase{T,TC,TP}"/> to copy the properties from.
        /// </param>
        protected void CopyProperties(ImageBase<T, TC, TP> other)
        {
            this.Quality = other.Quality;
            this.FrameDelay = other.FrameDelay;
        }
    }
}
