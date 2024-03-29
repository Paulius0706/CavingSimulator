﻿using CavingSimulator.GameLogic.Components;
using CavingSimulator.Render;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.Render.Meshes
{
    public class BlockMeshBuffer : IDisposable
    {
        public int texture = -1;
        public VertexBuffer vertexBuffer;
        public IndexBuffer indexBuffer;


        public readonly int indicesCount;
        public readonly int verticesCount;

        private bool disposed;
        private bool invalid;

        protected Random randomColorator;
        public BlockMeshBuffer(VertexPOTTiN[] vertices, int[] indices, Matrix4 transform)
        {
            verticesCount = vertices.Length;
            indicesCount = indices.Length;
            for(int i = 0; i < verticesCount; i++)
            {
                vertices[i] = new VertexPOTTiN((new Vector4(vertices[i].Position, 1) * transform).Xyz, vertices[i].Offset, vertices[i].Texture, vertices[i].TextureId, vertices[i].Normal);
            }
            vertexBuffer = new VertexBuffer(VertexPOTTiN.VertexInfo, vertices.Length, BufferUsageHint.StaticDraw);
            indexBuffer = new IndexBuffer(indicesCount, BufferUsageHint.StaticDraw);
            vertexBuffer.SetSubData(ref vertices, vertices.Length);
            indexBuffer.SetData(indices, indices.Length);
        }
        ~BlockMeshBuffer() { Dispose(); }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;


            if (vertexBuffer != null) vertexBuffer.Dispose();
            if (indexBuffer != null) indexBuffer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
