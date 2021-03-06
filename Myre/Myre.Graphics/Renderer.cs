﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Myre.Entities;
using Myre.Entities.Services;
using Myre.Extensions;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;

#if PROFILE
using Myre.Debugging.Statistics;
#endif

using Color = Microsoft.Xna.Framework.Color;

namespace Myre.Graphics
{
    public class Renderer
        : Service
    {
        private readonly RendererMetadata _data;
        private readonly RendererSettings _settings;
        private readonly Queue<RenderPlan.Output> _viewResults;

        private readonly IKernel _kernel;
        private readonly GraphicsDevice _device;
        private readonly SpriteBatch _spriteBatch;

        private Scene _scene;
        private IEnumerable<View> _views;

        public GraphicsDevice Device
        {
            get { return _device; }
        }

        public Scene Scene
        {
            get { return _scene; }
        }

        public RendererMetadata Data
        {
            get { return _data; }
        }

        public RendererSettings Settings
        {
            get { return _settings; }
        }

        public RenderPlan Plan { get; set; }

        public Renderer(IKernel kernel, GraphicsDevice device, IServiceProvider services)
        {
            _kernel = kernel;
            _device = device;
            _data = new RendererMetadata();
            _settings = new RendererSettings(this);
            _viewResults = new Queue<RenderPlan.Output>();
            _spriteBatch = new SpriteBatch(device);

            Content.Initialise(kernel.Get<ContentManager>(), services);
        }

        public override void Initialise(Scene scene)
        {
            _scene = scene;

            _views = from manager in scene.FindManagers<View.Manager>()
                     from view in manager.Views
                     select view;

            base.Initialise(scene);
        }

        protected override void Dispose(bool disposeManagedResources)
        {
            base.Dispose(disposeManagedResources);

            if (disposeManagedResources)
                Plan.Dispose();
        }

        public override void Update(float elapsedTime)
        {
            _data.Set<float>(Names.TimeDelta, elapsedTime);
            base.Update(elapsedTime);
        }

        public override void Draw()
        {
            var targets = _device.GetRenderTargets();

#if PROFILE
            Statistic.Create("Graphics.Primitives").Set(0);
            Statistic.Create("Graphics.Draws").Set(0);
#endif

            foreach (var view in _views)
            {
                view.Begin(this);
                {
                    view.SetMetadata(_data);
                    var output = Plan.Execute();

                    _viewResults.Enqueue(output);
                }
                view.End(this);
            }

            if (targets.Length != 0)
                _device.SetRenderTargets(targets);
            else
                _device.SetRenderTarget(null);


            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            foreach (var view in _views)
            {
                var output = _viewResults.Dequeue();
                var viewport = view.Viewport;

                if (output.Image.Format.IsFloatingPoint())
                    _device.SamplerStates[0] = SamplerState.PointClamp;
                else
                    _device.SamplerStates[0] = SamplerState.LinearClamp;

                _spriteBatch.Draw(output.Image, viewport.Bounds, Color.White);
                output.Finalise();
            }
            _spriteBatch.End();

            base.Draw();
        }

        public RenderPlan StartPlan()
        {
            return new RenderPlan(_kernel, this);
        }
    }
}
