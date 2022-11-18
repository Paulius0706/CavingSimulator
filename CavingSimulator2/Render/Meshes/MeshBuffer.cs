using CavingSimulator.GameLogic.Components;
using CavingSimulator.Render;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.Render.Meshes
{
    public class MeshBuffer : IDisposable
    {
        public readonly int textureID;
        public readonly VertexArray vertexArray;
        public readonly VertexBuffer vertexBuffer;
        public readonly IndexBuffer indexBuffer;


        public readonly int indicesCount = 0;
        public readonly int verticesCount = 0;

        private bool disposed;

        public MeshBuffer(string path, int textureID)
        {
            this.textureID = textureID;
            GetVerticesIndicesUV(path, out List<Vector3> vertices, out List<int> indices, out List<Vector2> textures);
            indicesCount = indices.Count;
            verticesCount = vertices.Count;
            VertexPCT[] buffer = new VertexPCT[verticesCount];
            for (int i = 0; i < verticesCount; i++)
            {
                buffer[i] = new VertexPCT(vertices[i], new Color4(1f, 1f, 1f, 1f), textures[i]);
            }
            vertexBuffer = new VertexBuffer(VertexPCT.VertexInfo, buffer.Length, BufferUsageHint.StaticDraw);
            indexBuffer = new IndexBuffer(indicesCount, BufferUsageHint.StaticDraw);
            vertexArray = new VertexArray(vertexBuffer);

            vertexBuffer.SetSubData(ref buffer, buffer.Length);
            indexBuffer.SetData(indices.ToArray(), indices.Count);
        }
        ~MeshBuffer()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            if (vertexBuffer != null) vertexBuffer.Dispose();
            if (indexBuffer != null) indexBuffer.Dispose();
        }

        private void GetVerticesIndicesUV(string path, out List<Vector3> verticesList, out List<int> indicesList, out List<Vector2> textureList)
        {
            string[] lines = File.ReadLines(path).ToArray();
            verticesList = new List<Vector3>();
            textureList = new List<Vector2>();
            indicesList = new List<int>();
            for(int i = 0; i<lines.Length;i++)
            {
                string line = lines[i];
                if (line.Length > 0 && line.StartsWith("v "))
                {
                    string[] sectors = line.Split(' ');
                    verticesList.Add(new Vector3(
                        float.Parse(sectors[1], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(sectors[2], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(sectors[3], CultureInfo.InvariantCulture.NumberFormat)
                        ));
                }
                if (line.Length > 0 && line.StartsWith("f "))
                {
                    string[] sectors = line.Split(' ').Select(l => l.Split('/')[0]).ToArray();
                    indicesList.Add(int.Parse(sectors[1]) - 1);
                    indicesList.Add(int.Parse(sectors[2]) - 1);
                    indicesList.Add(int.Parse(sectors[3]) - 1);
                }
                if (line.Length > 0 && line.StartsWith("vt "))
                {
                    string[] sectors = line.Split(' ');
                    textureList.Add(new Vector2(
                        float.Parse(sectors[1], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(sectors[2], CultureInfo.InvariantCulture.NumberFormat)
                        ));
                }
            }
            if (verticesList.Count != textureList.Count) throw new Exception("File is corrupted or not right format");
            if (verticesList.Count == 0 || textureList.Count == 0 || indicesList.Count == 0) throw new Exception("there are no vertices textures or indices");
        }
    }
}
