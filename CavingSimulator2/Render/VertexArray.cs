using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using OpenTK.Graphics.OpenGL;

namespace CavingSimulator2.Render
{
    /// <summary>
    /// VAO VertexArrayObject
    /// </summary>
    public sealed class VertexArray : IDisposable
    {
        public static readonly int MinVertexCount = 1;
        public static readonly int MaxVertexCount = 100_000;

        private bool disposed;

        public readonly int VertexArrayHandle;
        public readonly VertexBuffer VertexBuffer;

        public VertexArray(VertexBuffer vertexBuffer, bool stopLastBind = false)
        {
            if (vertexBuffer is null) throw new ArgumentNullException(nameof(vertexBuffer));
            VertexBuffer = vertexBuffer;
            VertexAttribute[] vertexAtributes = VertexBuffer.VertexInfo.VertexAttributes;

            VertexArrayHandle = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBuffer.VertexBufferHandle);
            for(int i = 0; i < vertexAtributes.Length; i++)
            {
                if (stopLastBind && vertexAtributes.Length - 1 == i) continue;
                GL.VertexAttribPointer(vertexAtributes[i].Index, vertexAtributes[i].ComponentCount, VertexAttribPointerType.Float, false, VertexBuffer.VertexInfo.SizeInBytes, vertexAtributes[i].Offset);
                GL.EnableVertexAttribArray(vertexAtributes[i].Index);
            }
            //foreach (VertexAttribute atribute in vertexAtributes)
            //{
            //    GL.VertexAttribPointer(atribute.Index, atribute.ComponentCount, VertexAttribPointerType.Float, false, VertexBuffer.VertexInfo.SizeInBytes, atribute.Offset);
            //    GL.EnableVertexAttribArray(atribute.Index);
            //}

            GL.BindVertexArray(0);

        }
        ~VertexArray()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            GL.BindVertexArray(0);
            GL.DeleteVertexArray(VertexArrayHandle);

            GC.SuppressFinalize(this);
        }
    }
}
