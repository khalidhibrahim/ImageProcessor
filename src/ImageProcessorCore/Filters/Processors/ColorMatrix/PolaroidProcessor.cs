﻿// <copyright file="PolaroidProcessor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore.Processors
{
    using System.Numerics;

    /// <summary>
    /// Converts the colors of the image recreating an old Polaroid effect.
    /// </summary>
    /// <typeparam name="T">The pixel accessor.</typeparam>
    /// <typeparam name="TC">The pixel format.</typeparam>
    /// <typeparam name="TP">The packed format. <example>uint, long, float.</example></typeparam>
    public class PolaroidProcessor<T, TC, TP> : ColorMatrixFilter<T, TC, TP>
        where T : IPixelAccessor<TC, TP>
        where TC : IPackedVector<TP>
        where TP : struct
    {
        /// <inheritdoc/>
        public override Matrix4x4 Matrix => new Matrix4x4()
        {
            M11 = 1.538f,
            M12 = -0.062f,
            M13 = -0.262f,
            M21 = -0.022f,
            M22 = 1.578f,
            M23 = -0.022f,
            M31 = .216f,
            M32 = -.16f,
            M33 = 1.5831f,
            M41 = 0.02f,
            M42 = -0.05f,
            M43 = -0.05f
        };

        /// <inheritdoc/>
        protected override void AfterApply(ImageBase<T, TC, TP> target, ImageBase<T, TC, TP> source, Rectangle targetRectangle, Rectangle sourceRectangle)
        {
            TC packedV = default(TC);
            packedV.PackFromBytes(102, 34, 0, 255); // Very dark orange [Brown tone]
            new VignetteProcessor<T, TC, TP> { VignetteColor = packedV }.Apply(target, target, sourceRectangle);

            TC packedG = default(TC);
            packedG.PackFromBytes(255, 153, 102, 178); // Light orange
            new GlowProcessor<T, TC, TP> { GlowColor = packedG, Radius = target.Width / 4F }.Apply(target, target, sourceRectangle);
        }
    }
}