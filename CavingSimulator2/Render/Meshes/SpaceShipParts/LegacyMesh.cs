using CavingSimulator.GameLogic.Components;
using CavingSimulator.Render;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.Render.Meshes.SpaceShipParts
{
    public abstract class LegacyMesh : IDisposable
    {
        public int texture = -1;
        protected VertexArray vertexArray;
        protected VertexBuffer vertexBuffer;
        protected IndexBuffer indexBuffer;


        protected int indicesCount = 0;
        protected int verticesCount = 0;
        protected VertexPCT[] buffer;
        protected int[] indices;


        private bool disposed;
        private bool invalid;

        public Transform transform;

        protected Vector3 lastPosition;
        protected Vector3 lastScale;
        protected Vector3 lastRotation;

        private Matrix4 model;

        protected Random randomColorator;


        protected LegacyMesh(Transform transform, string textureString = "", bool stopLastAributeBind = false)
        {
            this.transform = transform;
            model = Matrix4.Identity;

            if (textureString != "") texture = Game.textures.GetIndex(textureString);

            indicesCount = SetInidicesCount();
            verticesCount = SetVerticesCount();
            if (indicesCount > 0 && verticesCount > 0)
            {
                buffer = new VertexPCT[verticesCount];
                indices = new int[indicesCount];
                randomColorator = new Random();

                vertexBuffer = new VertexBuffer(VertexPCT.VertexInfo, buffer.Length, BufferUsageHint.StaticDraw);
                indexBuffer = new IndexBuffer(indicesCount, BufferUsageHint.StaticDraw);
                vertexArray = new VertexArray(vertexBuffer, stopLastAributeBind);

                GenerateBuffersAndVertices();
                vertexBuffer.SetSubData(ref buffer, buffer.Length);

                UpdateRender();

                indexBuffer.SetData(indices, indices.Length);

                buffer = null;
                indices = null;
            }
            else { invalid = true; }

        }
        protected LegacyMesh(string textureString = "", bool stopLastAributeBind = false)
        {
            transform = new Transform(Vector3.Zero);
            model = Matrix4.Identity;

            if (textureString != "") texture = Game.textures.GetIndex(textureString);


            indicesCount = SetInidicesCount();
            verticesCount = SetVerticesCount();
            if (indicesCount > 0 && verticesCount > 0)
            {
                buffer = new VertexPCT[verticesCount];
                indices = new int[indicesCount];
                randomColorator = new Random();

                vertexBuffer = new VertexBuffer(VertexPCT.VertexInfo, buffer.Length, BufferUsageHint.StaticDraw);
                indexBuffer = new IndexBuffer(indicesCount, BufferUsageHint.StaticDraw);
                vertexArray = new VertexArray(vertexBuffer, stopLastAributeBind);

                GenerateBuffersAndVertices();
                vertexBuffer.SetSubData(ref buffer, buffer.Length);

                UpdateRender();

                indexBuffer.SetData(indices, indices.Length);

                buffer = null;
                indices = null;
            }
            else { invalid = true; }
        }
        ~LegacyMesh()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;


            if (vertexArray != null) vertexArray.Dispose();
            if (vertexBuffer != null) vertexBuffer.Dispose();
            if (indexBuffer != null) indexBuffer.Dispose();
            GC.SuppressFinalize(this);
        }

        protected virtual int SetVerticesCount() { return 0; }
        protected virtual int SetInidicesCount() { return 0; }
        protected abstract void GenerateBuffersAndVertices();

        public void Render()
        {
            if (!invalid)
            {
                UpdateRender();
                UploadModel();
                if (Game.textures.ContainsKey(texture)) Game.textures[texture].UploadTexture();

                GL.BindVertexArray(vertexArray.VertexArrayHandle);
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer.IndexBufferHandle);
                GL.DrawElements(PrimitiveType.Triangles, indicesCount, DrawElementsType.UnsignedInt, 0);
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
        }
        private void UploadModel()
        {
            if (!Game.shaderPrograms.Current.GetShaderUniform("Model", out ShaderUniform shaderUniform)) { throw new ArgumentException("uniformName not found " + "Model"); }
            if (shaderUniform.Type != ActiveUniformType.FloatMat4) { throw new ArgumentException("uniform is not floatMarix4"); }

            GL.UniformMatrix4(shaderUniform.Location, true, ref model);
        }
        protected virtual void UpdateRender()
        {

            if (transform.Position == lastPosition && transform.Rotation == lastRotation && transform.Scale == lastScale) return;
            //Console.WriteLine("BlockPos" + transform.GlobalPosition);
            lastPosition = transform.Position;
            lastRotation = transform.Rotation;
            lastScale = transform.Scale;
            model =
                Matrix4.CreateScale(transform.Scale)
                * Matrix4.CreateFromQuaternion(new Quaternion(transform.Rotation))
                * Matrix4.CreateTranslation(transform.Position);

        }

    }
}
