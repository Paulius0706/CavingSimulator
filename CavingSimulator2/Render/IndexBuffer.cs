using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace CavingSimulator.Render
{
    public sealed class IndexBuffer : IDisposable
    {
        public static readonly int MinIndexCount = 1;
        public static readonly int MaxIndexCount = 250_000;

        private bool disposed;

        public readonly int IndexBufferHandle;
        public readonly int IndexCount;
        public readonly BufferUsageHint BufferUsageHint;

        public IndexBuffer(int indexCount, BufferUsageHint bufferUsageHint = BufferUsageHint.StaticDraw)
        {
            if (indexCount < MinIndexCount || indexCount > MaxIndexCount) throw new ArgumentOutOfRangeException(nameof(indexCount));
            IndexCount = indexCount;
            BufferUsageHint = bufferUsageHint;


            IndexBufferHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBufferHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, IndexCount * sizeof(int), IntPtr.Zero, BufferUsageHint);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }
        ~IndexBuffer()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DeleteBuffer(IndexBufferHandle);

            GC.SuppressFinalize(this);
        }

        public void SetData(int[] data, int count)
        {
            if (data is null) { throw new ArgumentNullException(nameof(data)); }
            if (data.Length < 1) { throw new ArgumentOutOfRangeException(nameof(data)); }
            if (count < 1 || count > IndexCount || count > data.Length) { throw new ArgumentOutOfRangeException(nameof(count)); }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexBufferHandle);
            GL.BufferSubData(BufferTarget.ElementArrayBuffer, IntPtr.Zero, count * sizeof(int), data);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }
    }
}
