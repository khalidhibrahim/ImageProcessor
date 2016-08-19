// <copyright file="DeuteranomalyProcessor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore.Processors
{
    using System.Numerics;

    /// <summary>
    /// Converts the colors of the image recreating Deuteranomaly (Green-Weak) color blindness.
    /// </summary>
    /// <typeparam name="T">The pixel accessor.</typeparam>
    /// <typeparam name="TC">The pixel format.</typeparam>
    /// <typeparam name="TP">The packed format. <example>uint, long, float.</example></typeparam>
    public class DeuteranomalyProcessor<T, TC, TP> : ColorMatrixFilter<T, TC, TP>
        where T : IPixelAccessor<TC, TP>
        where TC : IPackedVector<TP>
        where TP : struct
    {
        /// <inheritdoc/>
        public override Matrix4x4 Matrix => new Matrix4x4()
        {
            M11 = 0.8f,
            M12 = 0.258f,
            M21 = 0.2f,
            M22 = 0.742f,
            M23 = 0.142f,
            M33 = 0.858f
        };

        /// <inheritdoc/>
        public override bool Compand => false;
    }
}
