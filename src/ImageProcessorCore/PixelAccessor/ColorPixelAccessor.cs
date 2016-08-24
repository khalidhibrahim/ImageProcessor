﻿// <copyright file="Bgra32PixelAccessor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore
{
    using System;
    using System.Runtime.InteropServices;

    public sealed unsafe class ColorPixelAccessor : PixelAccessor<Color, uint>
    {
        public ColorPixelAccessor(ImageBase<PixelAccessor<Color, uint>, Color, uint> image)
            : base(image)
        {
        }
    }

    //    /// <summary>
    //    /// Provides per-pixel access to an images pixels.
    //    /// </summary>
    //    /// <remarks>
    //    /// The image data is always stored in <see cref="Color"/> format, where the red, green, blue, and
    //    /// alpha values are 8 bit unsigned bytes.
    //    /// </remarks>
    //    public sealed unsafe class ColorPixelAccessor : IPixelAccessor<Color, uint>
    //    {
    //        /// <summary>
    //        /// The position of the first pixel in the bitmap.
    //        /// </summary>
    //        private Color* pixelsBase;

    //        /// <summary>
    //        /// Provides a way to access the pixels from unmanaged memory.
    //        /// </summary>
    //        private GCHandle pixelsHandle;

    //        /// <summary>
    //        /// A value indicating whether this instance of the given entity has been disposed.
    //        /// </summary>
    //        /// <value><see langword="true"/> if this instance has been disposed; otherwise, <see langword="false"/>.</value>
    //        /// <remarks>
    //        /// If the entity is disposed, it must not be disposed a second
    //        /// time. The isDisposed field is set the first time the entity
    //        /// is disposed. If the isDisposed field is true, then the Dispose()
    //        /// method will not dispose again. This help not to prolong the entity's
    //        /// life in the Garbage Collector.
    //        /// </remarks>
    //        private bool isDisposed;

    //        /// <summary>
    //        /// Initializes a new instance of the <see cref="ColorPixelAccessor"/> class.
    //        /// </summary>
    //        /// <param name="image">
    //        /// The image to provide pixel access for.
    //        /// </param>
    //        public ColorPixelAccessor(ImageBase<ColorPixelAccessor, Color, uint> image)
    //        {
    //            Guard.NotNull(image, nameof(image));
    //            Guard.MustBeGreaterThan(image.Width, 0, "image width");
    //            Guard.MustBeGreaterThan(image.Height, 0, "image height");

    //            this.Width = image.Width;
    //            this.Height = image.Height;

    //            this.pixelsHandle = GCHandle.Alloc(image.Pixels, GCHandleType.Pinned);
    //            this.pixelsBase = (Color*)this.pixelsHandle.AddrOfPinnedObject().ToPointer();
    //        }

    //        /// <summary>
    //        /// Finalizes an instance of the <see cref="ColorPixelAccessor"/> class. 
    //        /// </summary>
    //        ~ColorPixelAccessor()
    //        {
    //            this.Dispose();
    //        }

    //        /// <summary>
    //        /// Gets the width of the image.
    //        /// </summary>
    //        public int Width { get; }

    //        /// <summary>
    //        /// Gets the height of the image.
    //        /// </summary>
    //        public int Height { get; }

    //        /// <summary>
    //        /// Gets or sets the color of a pixel at the specified position.
    //        /// </summary>
    //        /// <param name="x">
    //        /// The x-coordinate of the pixel. Must be greater
    //        /// than zero and smaller than the width of the pixel.
    //        /// </param>
    //        /// <param name="y">
    //        /// The y-coordinate of the pixel. Must be greater
    //        /// than zero and smaller than the width of the pixel.
    //        /// </param>
    //        /// <returns>The <see cref="IPackedVector"/> at the specified position.</returns>
    //        public Color this[int x, int y]
    //        {
    //            get
    //            {
    //#if DEBUG
    //                if ((x < 0) || (x >= this.Width))
    //                {
    //                    throw new ArgumentOutOfRangeException(nameof(x), "Value cannot be less than zero or greater than the bitmap width.");
    //                }

    //                if ((y < 0) || (y >= this.Height))
    //                {
    //                    throw new ArgumentOutOfRangeException(nameof(y), "Value cannot be less than zero or greater than the bitmap height.");
    //                }
    //#endif
    //                return *(this.pixelsBase + ((y * this.Width) + x));
    //            }

    //            set
    //            {
    //#if DEBUG
    //                if ((x < 0) || (x >= this.Width))
    //                {
    //                    throw new ArgumentOutOfRangeException(nameof(x), "Value cannot be less than zero or greater than the bitmap width.");
    //                }

    //                if ((y < 0) || (y >= this.Height))
    //                {
    //                    throw new ArgumentOutOfRangeException(nameof(y), "Value cannot be less than zero or greater than the bitmap height.");
    //                }
    //#endif
    //                *(this.pixelsBase + ((y * this.Width) + x)) = value;
    //            }
    //        }

    //        /// <summary>
    //        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    //        /// </summary>
    //        public void Dispose()
    //        {
    //            if (this.isDisposed)
    //            {
    //                return;
    //            }

    //            if (this.pixelsHandle.IsAllocated)
    //            {
    //                this.pixelsHandle.Free();
    //            }

    //            this.pixelsBase = null;

    //            // Note disposing is done.
    //            this.isDisposed = true;

    //            // This object will be cleaned up by the Dispose method.
    //            // Therefore, you should call GC.SuppressFinalize to
    //            // take this object off the finalization queue 
    //            // and prevent finalization code for this object
    //            // from executing a second time.
    //            GC.SuppressFinalize(this);
    //        }
    //    }
}
