using CavingSimulator.GameLogic.Components;
using CavingSimulator.Render;
using CavingSimulator2.GameLogic.Components;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.Render.Meshes
{
    public class BlockMesh
    {
        public int instances = 0;
        public VertexArray vertexArray;
        public VertexBuffer vertexBuffer;
        public IndexBuffer indexBuffer;

        public VertexBuffer positionsBuffer;


        public int indicesCount = 0;
        public int verticesCount = 0;
        public string key;


        private bool disposed;
        private bool invalid;



        public enum Face
        {
            top = 0,
            down = 1,
            right = 2,
            left = 3,
            foward = 4,
            back = 5
        }

        public BlockMesh(string key, bool stopLastAtributeArray = false)
        {
            this.key = key;
            indicesCount = key.Where(x => x == '1').Count() * 6;
            verticesCount = key.Where(x => x == '1').Count() * 4;
            if (indicesCount > 0 && verticesCount > 0)
            {
                VertexPCTO[] vertices = new VertexPCTO[verticesCount];
                int[] indices = new int[indicesCount];

                vertexBuffer = new VertexBuffer(VertexPCTO.VertexInfo, vertices.Length, BufferUsageHint.StaticDraw);
                indexBuffer = new IndexBuffer(indicesCount, BufferUsageHint.StaticDraw);
                vertexArray = new VertexArray(vertexBuffer, stopLastAtributeArray);

                GenerateIndicesAndVertices(ref vertices, ref indices);
                vertexBuffer.SetSubData(ref vertices, vertices.Length);
                indexBuffer.SetData(indices, indices.Length);
            }
            else { invalid = true; }

        }
        ~BlockMesh()
        {
            Dispose();
        }

        public void BindMesh()
        {
            GL.BindVertexArray(vertexArray.VertexArrayHandle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer.IndexBufferHandle);
        }
        public void UnBindMesh()
        {
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        private void GenerateIndicesAndVertices(ref VertexPCTO[] vertices, ref int[] indices)
        {
            int verticesCounter = 0;
            int indicesCounter = 0;

            //top
            if (key[(int)Face.top] == '1')
            {
                GenerateQuad(ref verticesCounter, ref indicesCounter, ref vertices, ref indices, new Vector3[]
                {
                new Vector3(+ 0.5f, + 0.5f, + 0.5f),
                new Vector3(- 0.5f, + 0.5f, + 0.5f),

                new Vector3(- 0.5f, - 0.5f, + 0.5f),
                new Vector3(+ 0.5f, - 0.5f, + 0.5f)
                });
            }
            //down
            if (key[(int)Face.down] == '1')
            {
                GenerateQuad(ref verticesCounter, ref indicesCounter, ref vertices, ref indices, new Vector3[]
                {
                new Vector3(- 0.5f, + 0.5f, - 0.5f),
                new Vector3(+ 0.5f, + 0.5f, - 0.5f),

                new Vector3(+ 0.5f, - 0.5f, - 0.5f),
                new Vector3(- 0.5f, - 0.5f, - 0.5f)
                });
            }

            //right
            if (key[(int)Face.right] == '1')
            {
                GenerateQuad(ref verticesCounter, ref indicesCounter, ref vertices, ref indices, new Vector3[]
                {
                new Vector3(+ 0.5f, - 0.5f, + 0.5f),
                new Vector3(+ 0.5f, + 0.5f, + 0.5f),

                new Vector3(+ 0.5f, + 0.5f, - 0.5f),
                new Vector3(+ 0.5f, - 0.5f, - 0.5f)
                });
            }
            //left
            if (key[(int)Face.left] == '1')
            {
                GenerateQuad(ref verticesCounter, ref indicesCounter, ref vertices, ref indices, new Vector3[]
                {
                new Vector3(- 0.5f, + 0.5f, + 0.5f),
                new Vector3(- 0.5f, - 0.5f, + 0.5f),

                new Vector3(- 0.5f, - 0.5f, - 0.5f),
                new Vector3(- 0.5f, + 0.5f, - 0.5f)
                });
            }
            //foward
            if (key[(int)Face.foward] == '1')
            {
                GenerateQuad(ref verticesCounter, ref indicesCounter, ref vertices, ref indices, new Vector3[]
                {
                new Vector3(- 0.5f, + 0.5f, + 0.5f),
                new Vector3(+ 0.5f, + 0.5f, + 0.5f),

                new Vector3(+ 0.5f, + 0.5f, - 0.5f),
                new Vector3(- 0.5f, + 0.5f, - 0.5f)
                });
            }
            //back
            if (key[(int)Face.back] == '1')
            {
                GenerateQuad(ref verticesCounter, ref indicesCounter, ref vertices, ref indices, new Vector3[]
                {
                new Vector3(- 0.5f, - 0.5f, + 0.5f),
                new Vector3(+ 0.5f, - 0.5f, + 0.5f),

                new Vector3(+ 0.5f, - 0.5f, - 0.5f),
                new Vector3(- 0.5f, - 0.5f, - 0.5f)
                });
            }
            
        }
        private void GenerateQuad(ref int verticesCounter, ref int indicesCounter, ref VertexPCTO[] vertices, ref int[] indices, Vector3[] positions)
        {
            Random random = new Random();
            float r = (float)random.NextDouble();
            float g = (float)random.NextDouble();
            float b = (float)random.NextDouble();


            indices[indicesCounter++] = verticesCounter + 0;
            indices[indicesCounter++] = verticesCounter + 1;
            indices[indicesCounter++] = verticesCounter + 2;
            indices[indicesCounter++] = verticesCounter + 0;
            indices[indicesCounter++] = verticesCounter + 2;
            indices[indicesCounter++] = verticesCounter + 3;

            vertices[verticesCounter++] = new VertexPCTO(positions[0], new Color4(r, g, b, 1f), new Vector2(0, 1), new Vector3());
            vertices[verticesCounter++] = new VertexPCTO(positions[1], new Color4(r, g, b, 1f), new Vector2(1, 1), new Vector3());
            vertices[verticesCounter++] = new VertexPCTO(positions[2], new Color4(r, g, b, 1f), new Vector2(1, 0), new Vector3());
            vertices[verticesCounter++] = new VertexPCTO(positions[3], new Color4(r, g, b, 1f), new Vector2(0, 0), new Vector3());

        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;


            if (vertexArray != null) vertexArray.Dispose();
            if (vertexBuffer != null) vertexBuffer.Dispose();
            if (indexBuffer != null) indexBuffer.Dispose();
            if (positionsBuffer != null) positionsBuffer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
