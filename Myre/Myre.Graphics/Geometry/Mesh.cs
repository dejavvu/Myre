﻿using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Myre.Extensions;
using Myre.Graphics.Materials;
using SwizzleMyVectors.Geometry;

namespace Myre.Graphics.Geometry
{
    public sealed class Mesh
        :IDisposable
    {
        public string Name { get; set; }
        public int TriangleCount { get; set; }
        public int VertexCount { get; set; }
        public VertexBuffer VertexBuffer { get; set; }
        public IndexBuffer IndexBuffer { get; set; }
        public Dictionary<string, Material> Materials { get; set; }
        public BoundingSphere BoundingSphere { get; set; }
        public int StartIndex { get; set; }
        public int BaseVertex { get; set; }
        public int MinVertexIndex { get; set; }
        public Matrix4x4 MeshTransform { get; set; }
        public ushort? ParentBoneIndex { get; set; }

        public Mesh()
        {
            MeshTransform = Matrix4x4.Identity;
            ParentBoneIndex = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (VertexBuffer != null)
                        VertexBuffer.Dispose();
                    VertexBuffer = null;

                    if (IndexBuffer != null)
                        IndexBuffer.Dispose();
                    IndexBuffer = null;
                }

                _disposed = true;
            } 
        }

        ~Mesh()
        {
            Dispose(false);
        }
    }

    public class MeshReader : ContentTypeReader<Mesh>
    {
        protected override Mesh Read(ContentReader input, Mesh existingInstance)
        {
            var mesh = existingInstance ?? new Mesh();

            mesh.MeshTransform = Matrix4x4.Identity;

            mesh.Name = input.ReadString();
            mesh.VertexCount = input.ReadInt32();
            mesh.TriangleCount = input.ReadInt32();

            mesh.ParentBoneIndex = input.ReadBoolean() ? input.ReadUInt16() : (ushort?)null;

            bool hasVertexData = input.ReadBoolean();
            if (hasVertexData)
                mesh.VertexBuffer = input.ReadObject<VertexBuffer>();

            bool hasIndexData = input.ReadBoolean();
            if (hasIndexData)
                mesh.IndexBuffer = input.ReadObject<IndexBuffer>();

            mesh.StartIndex = input.ReadInt32();
            mesh.BaseVertex = input.ReadInt32();
            mesh.MinVertexIndex = input.ReadInt32();

            var size = input.ReadInt32();
            mesh.Materials = new Dictionary<string, Material>(size);
            for (int i = 0; i < size; i++)
            {
                var key = input.ReadString();
                var material = input.ReadObject<Material>();
                mesh.Materials.Add(key, material);
            }

            mesh.BoundingSphere = input.ReadObject<Microsoft.Xna.Framework.BoundingSphere>().FromXNA();

            return mesh;
        }
    }
}
