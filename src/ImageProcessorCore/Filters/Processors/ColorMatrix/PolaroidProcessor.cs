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
    /// <typeparam name="TColor">The pixel format.</typeparam>
    /// <typeparam name="TPacked">The packed format. <example>uint, long, float.</example></typeparam>
    public class PolaroidProcessor<TColor, TPacked> : ColorMatrixFilter<TColor, TPacked>
        where TColor : IPackedVector<TPacked>
        where TPacked : struct
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
        protected override void AfterApply(ImageBase<TColor, TPacked> target, ImageBase<TColor, TPacked> source, Rectangle targetRectangle, Rectangle sourceRectangle)
        {
            TColor packedV = default(TColor);
            packedV.PackFromBytes(102, 34, 0, 255); // Very dark orange [Brown tone]
            new VignetteProcessor<TColor, TPacked> { VignetteColor = packedV }.Apply(target, target, sourceRectangle);

            TColor packedG = default(TColor);
            packedG.PackFromBytes(255, 153, 102, 178); // Light orange
            new GlowProcessor<TColor, TPacked> { GlowColor = packedG, Radius = target.Width / 4F }.Apply(target, target, sourceRectangle);
        }
    }
}