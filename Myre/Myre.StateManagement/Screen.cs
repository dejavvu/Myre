﻿using System;

using GameTime = Microsoft.Xna.Framework.GameTime;

namespace Myre.StateManagement
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Screen
        :IDisposable
    {
        private TransitionState _transitionState = TransitionState.Hidden;

        /// <summary>
        /// Gets the <see cref="ScreenManager"/> which this screen was last pushed onto.
        /// </summary>
        /// <value>The manager.</value>
        public ScreenManager Manager { get; internal set; }

        ~Screen()
        {
            Dispose(false);
        }

        /// <summary>
        /// Updates the screen.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Draws this instance.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public virtual void Draw(GameTime gameTime)
        {
        }

        /// <summary>
        /// Prepares the draw.
        /// </summary>
        public virtual void PrepareDraw()
        {
        }

        /// <summary>
        /// Gets or sets the transition on time.
        /// </summary>
        /// <value>The transition on.</value>
        public TimeSpan TransitionOn
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the transition off time.
        /// </summary>
        /// <value>The transition off.</value>
        public TimeSpan TransitionOff
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or transition progress.
        /// 0 is hidden, and 1 is shown.
        /// </summary>
        /// <value>The transition progress.</value>
        public float TransitionProgress
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the state of the transition.
        /// </summary>
        /// <value>The state of the transition.</value>
        public TransitionState TransitionState
        {
            get { return _transitionState; }
            internal set
            {
                if (_transitionState == value)
                    return;

                _transitionState = value;
                switch (_transitionState)
                {
                    case TransitionState.Hidden:
                        OnHidden();
                        break;
                    case TransitionState.On:
                        BeginTransitionOn();
                        break;
                    case TransitionState.Off:
                        BeginTransitionOff();
                        break;
                    case TransitionState.Shown:
                        OnShown();
                        break;
                }
            }
        }

        /// <summary>
        /// Called when the screen begins transitioning on
        /// </summary>
        protected virtual void BeginTransitionOn()
        {
        }

        /// <summary>
        /// Called when the screen begins transitioning off
        /// </summary>
        protected virtual void BeginTransitionOff()
        {
        }

        /// <summary>
        /// Called when the screen state changes to visible.
        /// </summary>
        protected virtual void OnShown()
        {
        }

        /// <summary>
        /// Called when the screen state changes to hidden.
        /// </summary>
        protected virtual void OnHidden()
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _disposed = true;
        }
    }
}
