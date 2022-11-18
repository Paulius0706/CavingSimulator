using CavingSimulator.GameLogic.Components;
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
    public class Mesh : IDisposable
    {
        public readonly MeshBuffer meshBuffer;

        public Transform transform;

        private Vector3 lastPosition = Vector3.Zero;
        private Vector3 lastScale = Vector3.Zero;
        private Vector3 lastRotation = Vector3.Zero;

        private Matrix4 model;

        private bool disposed;
        public Mesh(string name)
        {
            this.meshBuffer = Game.meshes[name];
        }
        public Mesh(Transform transform, string name)
        {
            this.transform = transform;
            this.meshBuffer = Game.meshes[name];
        }

        public void Render()
        {
            UpdateRender();
            UploadModel();
            if (Game.textures.ContainsKey(meshBuffer.textureID)) Game.textures[meshBuffer.textureID].UploadTexture();

            GL.BindVertexArray(meshBuffer.vertexArray.VertexArrayHandle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, meshBuffer.indexBuffer.IndexBufferHandle);
            GL.DrawElements(PrimitiveType.Triangles, meshBuffer.indicesCount, DrawElementsType.UnsignedInt, 0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
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
            lastPosition = transform.Position;
            lastRotation = transform.Rotation;
            lastScale = transform.Scale;
            model = Matrix4.CreateScale(transform.Scale)
                * Matrix4.CreateFromQuaternion(new Quaternion(transform.Rotation))
                * Matrix4.CreateTranslation(transform.Position);

        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

        }
    }
}
