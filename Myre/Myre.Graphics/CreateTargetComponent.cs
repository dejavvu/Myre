﻿using System.Numerics;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace Myre.Graphics
{
    /// <summary>
    /// A renderer component which simply creates a texture resource and clears it to black
    /// </summary>
    public class CreateTargetComponent
        : RendererComponent
    {
        private static int _counter;

        private readonly string _name;
        private readonly RenderTargetInfo _targetInfo;

        public CreateTargetComponent(RenderTargetInfo targetInfo, string resourceName = null)
        {
            _targetInfo = targetInfo;

            _counter = (_counter + 1) % (int.MaxValue - 1);
            _name = resourceName ?? string.Format("anonymous-{0}-{1}", _counter, targetInfo.GetHashCode());
        }

        public override void Initialise(Renderer renderer, ResourceContext context)
        {            
            // define outputs
            context.DefineOutput(_name, true, null, _targetInfo);

            base.Initialise(renderer, context);
        }

        public override void Draw(Renderer renderer)
        {
            var info = _targetInfo;
            if (info.Width == 0 || info.Height == 0)
            {
                var resolution = renderer.Data.GetValue(new TypedName<Vector2>("resolution"));
                info = new RenderTargetInfo(
                    (int) resolution.X,
                    (int) resolution.Y,
                    info.SurfaceFormat,
                    info.DepthFormat,
                    info.MultiSampleCount,
                    info.MipMap,
                    info.Usage
                );
            }

            var target = RenderTargetManager.GetTarget(renderer.Device, info);
            renderer.Device.SetRenderTarget(target);
            renderer.Device.Clear(ClearOptions.Target | ClearOptions.DepthBuffer | ClearOptions.Stencil, Color.Black, 1, 0);

            Output(_name, target);
        }
    }
}
