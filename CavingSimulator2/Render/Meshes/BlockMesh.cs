using CavingSimulator.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.Render.Meshes
{
    public class BlockMesh : IDisposable
    {
        public VertexArray vertexArray;
        public VertexBuffer instanceBuffer;
        public bool active;
        private bool disposed;
        public BlockMesh(VertexBuffer vertexBuffer)
        {
            vertexArray = new VertexArray(vertexBuffer);
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;


            if (vertexArray != null) vertexArray.Dispose();
            if(instanceBuffer != null) instanceBuffer.Dispose();

        }
    }
}
