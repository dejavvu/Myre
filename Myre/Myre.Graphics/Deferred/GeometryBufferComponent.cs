﻿using System.Numerics;
using Microsoft.Xna.Framework.Graphics;
using Myre.Graphics.Geometry;
using Myre.Graphics.Materials;
using Myre.Graphics.PostProcessing;

using Color = Microsoft.Xna.Framework.Color;

namespace Myre.Graphics.Deferred
{
    public class GeometryBufferComponent
        : RendererComponent
    {
        private readonly Resample _scale;
        private readonly Material _clear;
        private readonly Quad _quad;
        private GeometryRenderer _geometry;

        public GeometryBufferComponent(GraphicsDevice device)
        {
            _clear = new Material(Content.Load<Effect>("ClearGBuffer"));
            _scale = new Resample(device);
            _quad = new Quad(device);
        }

        public override void Initialise(Renderer renderer, ResourceContext context)
        {
            _geometry = new GeometryRenderer(renderer.Scene.FindManagers<IGeometryProvider>());

            // define outputs
            context.DefineOutput("gbuffer_depth", isLeftSet: false, surfaceFormat: SurfaceFormat.Single, depthFormat: DepthFormat.Depth24Stencil8);
            context.DefineOutput("gbuffer_normals", isLeftSet: false, surfaceFormat: SurfaceFormat.Rgba1010102);
            context.DefineOutput("gbuffer_diffuse", isLeftSet: false, surfaceFormat: SurfaceFormat.Color);
            context.DefineOutput("gbuffer_depth_downsample", isLeftSet: true, surfaceFormat: SurfaceFormat.Single);

            base.Initialise(renderer, context);
        }

        public override void Draw(Renderer renderer)
        {
            var metadata = renderer.Data;
            var device = renderer.Device;

            var resolution = metadata.GetValue(new TypedName<Vector2>("resolution"));
            var width = (int)resolution.X;
            var height = (int)resolution.Y;

            var depth = RenderTargetManager.GetTarget(device, width, height, SurfaceFormat.Single, DepthFormat.Depth24Stencil8, name:"depth");
            var normals = RenderTargetManager.GetTarget(device, width, height, SurfaceFormat.Rgba1010102, name: "normals");
            var diffuse = RenderTargetManager.GetTarget(device, width, height, SurfaceFormat.Color, name: "diffuse");

            device.SetRenderTargets(depth, normals, diffuse);
            device.BlendState = BlendState.Opaque;
            device.Clear(Color.Black);

            device.DepthStencilState = DepthStencilState.None;
            _quad.Draw(_clear, metadata);
            device.DepthStencilState = DepthStencilState.Default;

            device.BlendState = BlendState.Opaque;

            _geometry.Draw("gbuffer", renderer);

            Output("gbuffer_depth", depth);
            Output("gbuffer_normals", normals);
            Output("gbuffer_diffuse", diffuse);

            DownsampleDepth(renderer, depth);
        }

        private void DownsampleDepth(Renderer renderer, RenderTarget2D depth)
        {
            var downsampled = RenderTargetManager.GetTarget(renderer.Device, depth.Width / 2, depth.Height / 2, SurfaceFormat.Single, name: "downsample depth", usage: RenderTargetUsage.DiscardContents);
            _scale.Scale(depth, downsampled);
            Output("gbuffer_depth_downsample", downsampled);
        }
    }
}