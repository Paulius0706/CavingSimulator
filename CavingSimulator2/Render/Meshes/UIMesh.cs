using CavingSimulator.GameLogic.Components;
using CavingSimulator.Render;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.Render.Meshes
{
    public class UIMesh
    {
        public readonly int textureID;
        public readonly VertexArray vertexArray;
        public readonly VertexBuffer vertexBuffer;
        public readonly IndexBuffer indexBuffer;

        public VertexPCTp[] vertices;


        public const int indicesCount = 6;
        public const int verticesCount = 4;
        public int[] indices;
        private float depth;
        public float Depth
        {
            get { return depth * 100; }
            set { depth = value / 100; }
        }

        private bool disposed;

        public UIMesh(int textureID, Vector2 upperPosition, Vector2 lowerPosition, float depth)
        {
            this.depth = depth;
            this.textureID = textureID;
            indices = new int[6] { 0, 1, 2, 0, 2, 3 };
            vertices = new VertexPCTp[verticesCount];
            vertices[0] = new VertexPCTp(new Vector3(lowerPosition.X, upperPosition.Y, 0f), Color4.White, new Vector2(0, 1));
            vertices[1] = new VertexPCTp(new Vector3(upperPosition.X, upperPosition.Y, 0f), Color4.White, new Vector2(1, 1));
            vertices[2] = new VertexPCTp(new Vector3(upperPosition.X, lowerPosition.Y, 0f), Color4.White, new Vector2(1, 0));
            vertices[3] = new VertexPCTp(new Vector3(lowerPosition.X, lowerPosition.Y, 0f), Color4.White, new Vector2(0, 0));

            vertexBuffer = new VertexBuffer(VertexPCTp.VertexInfo, vertices.Length, BufferUsageHint.StaticDraw);
            indexBuffer = new IndexBuffer(indicesCount, BufferUsageHint.StaticDraw);
            vertexArray = new VertexArray(vertexBuffer);

            vertexBuffer.SetSubData(ref vertices, vertices.Length);
            indexBuffer.SetData(indices, indices.Length);
        }
        public UIMesh(int textureID, Vector2 upperPosition, Vector2 lowerPosition, Vector2 textureLowerPosition, Vector2 textureUpperPosition, float depth)
        {
            this.depth = depth;
            this.textureID = textureID;
            indices = new int[6] { 0, 1, 2, 0, 2, 3 };
            vertices = new VertexPCTp[verticesCount];
            vertices[0] = new VertexPCTp(new Vector3(lowerPosition.X, upperPosition.Y, 0f), Color4.White, new Vector2(textureLowerPosition.X, textureUpperPosition.Y));
            vertices[1] = new VertexPCTp(new Vector3(upperPosition.X, upperPosition.Y, 0f), Color4.White, textureUpperPosition);
            vertices[2] = new VertexPCTp(new Vector3(upperPosition.X, lowerPosition.Y, 0f), Color4.White, new Vector2(textureUpperPosition.X, textureLowerPosition.Y));
            vertices[3] = new VertexPCTp(new Vector3(lowerPosition.X, lowerPosition.Y, 0f), Color4.White, textureLowerPosition);

            vertexBuffer = new VertexBuffer(VertexPCTp.VertexInfo, vertices.Length, BufferUsageHint.StaticDraw);
            indexBuffer = new IndexBuffer(indicesCount, BufferUsageHint.StaticDraw);
            vertexArray = new VertexArray(vertexBuffer);

            vertexBuffer.SetSubData(ref vertices, vertices.Length);
            indexBuffer.SetData(indices, indices.Length);
        }
        ~UIMesh()
        {
            Dispose();
        }

        public void Render()
        {
            if (Game.textures.ContainsKey(textureID)) Game.textures[textureID].UploadTexture();
            Game.shaderPrograms.Current.SetUniform("Depth", depth);
            GL.BindVertexArray(vertexArray.VertexArrayHandle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer.IndexBufferHandle);
            GL.DrawElements(PrimitiveType.Triangles, indicesCount, DrawElementsType.UnsignedInt, 0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            if (vertexBuffer != null) vertexBuffer.Dispose();
            if (indexBuffer != null) indexBuffer.Dispose();
            if (vertexArray != null) vertexArray.Dispose();
        }
    }
}
