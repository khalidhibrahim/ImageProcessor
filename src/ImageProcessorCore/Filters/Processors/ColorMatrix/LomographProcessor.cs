// <copyright file="LomographProcessor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore.Processors
{
    using System.Numerics;

    /// <summary>
    /// Converts the colors of the image recreating an old Lomograph effect.
    /// </summary>
    /// <typeparam name="T">The pixel accessor.</typeparam>
    /// <typeparam name="TC">The pixel format.</typeparam>
    /// <typeparam name="TP">The packed format. <example>uint, long, float.</example></typeparam>
    public class LomographProcessor<T, TC, TP> : ColorMatrixFilter<T, TC, TP>
        where T : IPixelAccessor<TC, TP>
        where TC : IPackedVector<TP>
        where TP : struct
    {
        /// <inheritdoc/>
        public override Matrix4x4 Matrix => new Matrix4x4()
        {
            M11 = 1.5f,
            M22 = 1.45f,
            M33 = 1.11f,
            M41 = -.1f,
            M42 = .0f,
            M43 = -.08f
        };

        /// <inheritdoc/>
        protected override void AfterApply(ImageBase<T, TC, TP> target, ImageBase<T, TC, TP> source, Rectangle targetRectangle, Rectangle sourceRectangle)
        {
            TC packed = default(TC);
            packed.PackFromBytes(0, 10, 0, 255); // Very dark (mostly black) lime green.
            new VignetteProcessor<T, TC, TP> { VignetteColor = packed }.Apply(target, target, sourceRectangle);
        }
    }
}
