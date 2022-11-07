using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace CavingSimulator2.Render
{
    /// <summary>
    /// VBO VertexBuffer Object
    /// </summary>
    public sealed class VertexBuffer : IDisposable
    {
        public static readonly int MinVertexCount = 1;
        public static readonly int MaxVertexCount = 100_000;

        private bool disposed;

        public readonly int VertexBufferHandle;

        public readonly int VertexCount;
        public readonly VertexInfo VertexInfo;
        public readonly BufferUsageHint BufferUsageHint;

        public VertexBuffer(VertexInfo vertexInfo, int vertexCount, BufferUsageHint bufferUsageHint = BufferUsageHint.StreamDraw, bool keepBind = false) 
        {
            if (vertexCount < MinVertexCount || vertexCount > MaxVertexCount) throw new ArgumentOutOfRangeException(nameof(vertexCount));
            VertexInfo = vertexInfo;
            VertexCount = vertexCount;
            BufferUsageHint = bufferUsageHint;

            VertexBufferHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferHandle);
            if (!keepBind)
            {
                GL.BufferData(BufferTarget.ArrayBuffer, VertexCount * VertexInfo.SizeInBytes, IntPtr.Zero, BufferUsageHint);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            }

        }
        ~VertexBuffer()
        {
            Dispose();
        }


        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VertexBufferHandle);

            GC.SuppressFinalize(this);
        }

        public void SetSubData<T>(ref T[] data, int count) where T : struct
        {
            if (typeof(T) != VertexInfo.Type) { throw new ArgumentException("Not correct type"); }
            if (data is null) { throw new ArgumentNullException(nameof(data)); }
            if (data.Length < 1) { throw new ArgumentOutOfRangeException(nameof(data)); }
            if (count < 1 || count > VertexCount || count > data.Length) { throw new ArgumentOutOfRangeException(nameof(count)); }

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferHandle);
            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, count * VertexInfo.SizeInBytes, data);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void SetData<T>(ref T[] data, int count) where T : struct
        {
            if (typeof(T) != VertexInfo.Type) { throw new ArgumentException("Not correct type"); }
            if (data is null) { throw new ArgumentNullException(nameof(data)); }
            if (data.Length < 1) { throw new ArgumentOutOfRangeException(nameof(data)); }
            if (count < 1 || count > VertexCount || count > data.Length) { throw new ArgumentOutOfRangeException(nameof(count)); }

            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferHandle);
            GL.BufferData<T>(BufferTarget.ArrayBuffer, count * VertexInfo.SizeInBytes, data,BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
        public void SetDataAlt<T>(ref T[] data, int count) where T : struct
        {
            if (typeof(T) != VertexInfo.Type) { throw new ArgumentException("Not correct type"); }
            if (data is null) { throw new ArgumentNullException(nameof(data)); }
            if (data.Length < 1) { throw new ArgumentOutOfRangeException(nameof(data)); }
            if (count < 1 || count > VertexCount || count > data.Length) { throw new ArgumentOutOfRangeException(nameof(count)); }

            GL.BufferData<T>(BufferTarget.ArrayBuffer, count * VertexInfo.SizeInBytes, data, BufferUsageHint.StaticDraw);
        }
    }
}
