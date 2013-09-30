﻿using System.Collections.ObjectModel;
using System.Linq;
using Myre.Graphics.Geometry;

namespace Myre.Graphics.Translucency
{
    public class TranslucentComponent
        : RendererComponent
    {
        private ReadOnlyCollection<IGeometryProvider> _geometryProviders;

        public override void Initialise(Renderer renderer, ResourceContext context)
        {
            _geometryProviders = renderer.Scene.FindManagers<IGeometryProvider>();

            // define inputs
            if (context.AvailableResources.Any(r => r.Name == "gbuffer_depth"))
                context.DefineInput("gbuffer_depth");

            // define outputs
            foreach (var resource in context.SetRenderTargets)
                context.DefineOutput(resource);

            base.Initialise(renderer, context);
        }

        public override void Draw(Renderer renderer)
        {
            foreach (var geometryProvider in _geometryProviders)
                geometryProvider.Draw("translucent", renderer.Data);
        }
    }
}