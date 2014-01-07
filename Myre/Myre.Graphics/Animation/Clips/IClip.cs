﻿using System;

namespace Myre.Graphics.Animation.Clips
{
    public interface IClip
    {
        /// <summary>
        /// The name of this animation
        /// </summary>
        string Name { get; }

        /// <summary>
        /// This animation is about to start playing
        /// if this aniamtion is set to loop, this will be called every time a iteration loop starts
        /// </summary>
        void Start();

        /// <summary>
        /// The keyframes of this animation in time order, split by channel
        /// </summary>
        Keyframe[][] Channels { get; }

        /// <summary>
        /// The duration of this animation
        /// </summary>
        TimeSpan Duration { get; }
    }
}
