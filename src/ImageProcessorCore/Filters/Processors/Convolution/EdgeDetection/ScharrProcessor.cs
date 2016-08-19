// <copyright file="ScharrProcessor.cs" company="James Jackson-South">
// Copyright (c) James Jackson-South and contributors.
// Licensed under the Apache License, Version 2.0.
// </copyright>

namespace ImageProcessorCore.Processors
{
    /// <summary>
    /// The Scharr operator filter.
    /// <see href="http://en.wikipedia.org/wiki/Sobel_operator#Alternative_operators"/>
    /// <typeparam name="T">The pixel accessor.</typeparam>
    /// <typeparam name="TC">The pixel format.</typeparam>
    /// <typeparam name="TP">The packed format. <example>uint, long, float.</example></typeparam>
    public class ScharrProcessor<T, TC, TP> : EdgeDetector2DFilter<T, TC, TP>
        where T : IPixelAccessor<TC, TP>
        where TC : IPackedVector<TP>
        where TP : struct
    {
        /// <inheritdoc/>
        public override float[,] KernelX => new float[,]
        {
            { -3, 0, 3 },
            { -10, 0, 10 },
            { -3, 0, 3 }
        };

        /// <inheritdoc/>
        public override float[,] KernelY => new float[,]
        {
            { 3, 10, 3 },
            { 0, 0, 0 },
            { -3, -10, -3 }
        };
    }
}
