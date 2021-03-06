﻿using System;
using System.Diagnostics.Contracts;
using System.Threading;
using Ninject;

using Game = Microsoft.Xna.Framework.Game;

namespace Myre.Entities.Services
{
    /// <summary>
    /// Manages wallclock time passed and counts the number of frames passed
    /// </summary>
    public class TimeService
        : Service
    {
        /// <summary>
        /// The numbers of seconds which have passed since this scene was constructed or the time was last reset
        /// </summary>
        private double _time;
        public double Time
        {
            get { return _time; }
        }
        /// <summary>
        /// The number of frames which have passed since the scene was constucted or the time was last reset
        /// </summary>
        public uint Tick
        {
            get;
            private set;
        }

        private readonly Game _game;
        /// <summary>
        /// The target for time elapsed each frame
        /// </summary>
        private TimeSpan TargetElapsedTime
        {
            get
            {
                return _game.TargetElapsedTime;
            }
        }

        public TimeService(Game game)
        {
            Contract.Requires(game != null);

            _game = game;
            UpdateOrder = int.MaxValue;
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_game != null);
        }

        public override void Update(float elapsedTime)
        {
            Interlocked.Exchange(ref _time, _time + elapsedTime);

            //Either we overflow, or the game crashes. Either way it doesn't matter because with a 16ms frame time this occurs after 2.1 YEARS of gameplay!
            unchecked { Tick++; }

            base.Update(elapsedTime);
        }

        /// <summary>
        /// Convert the given time into the associated tick value
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static uint ConvertToTick(double time)
        {
            var game = NinjectKernel.Instance.Get<Game>();
            Contract.Assume(game != null);
            var target = game.TargetElapsedTime;
            return (uint)(time / target.TotalSeconds);
        }

        /// <summary>
        /// convert the given tick into the associated time value
        /// </summary>
        /// <param name="tick"></param>
        /// <returns></returns>
        public static double ConvertFromTick(uint tick)
        {
            var game = NinjectKernel.Instance.Get<Game>();
            Contract.Assume(game != null);
            var target = game.TargetElapsedTime;
            return tick * (float)target.TotalSeconds;
        }

        public void Reset()
        {
            Tick = 0;
            Interlocked.Exchange(ref _time, 0);
        }

        public void SetTime(double time)
        {
            Tick = (uint)(time / TargetElapsedTime.TotalSeconds);
            Interlocked.Exchange(ref _time, Tick * TargetElapsedTime.TotalSeconds);
        }
    }
}
