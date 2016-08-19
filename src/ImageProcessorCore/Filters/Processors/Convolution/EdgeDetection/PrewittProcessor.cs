﻿// <copyright file="PrewittProcessor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore.Processors
{
    /// <summary>
    /// The Prewitt operator filter.
    /// <see href="http://en.wikipedia.org/wiki/Prewitt_operator"/>
    /// </summary>
    /// <typeparam name="T">The pixel accessor.</typeparam>
    /// <typeparam name="TC">The pixel format.</typeparam>
    /// <typeparam name="TP">The packed format. <example>uint, long, float.</example></typeparam>
    public class PrewittProcessor<T, TC, TP> : EdgeDetector2DFilter<T, TC, TP>
        where T : IPixelAccessor<TC, TP>
        where TC : IPackedVector<TP>
        where TP : struct
    {
        /// <inheritdoc/>
        public override float[,] KernelX => new float[,]
        {
            { -1, 0, 1 },
            { -1, 0, 1 },
            { -1, 0, 1 }
        };

        /// <inheritdoc/>
        public override float[,] KernelY => new float[,]
        {
            { 1, 1, 1 },
            { 0, 0, 0 },
            { -1, -1, -1 }
        };
    }
}
