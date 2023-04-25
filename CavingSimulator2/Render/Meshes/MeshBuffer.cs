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
    /// <summary>
    /// Palace where blender mesh cordiantes is stored
    /// </summary>
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
            GetVerticesIndicesUV(path, out List<Vector3> vertices, out List<int> indices, out List<Vector2> textures, out List<Vector3> normals);
            indicesCount = indices.Count;
            verticesCount = vertices.Count;
            VertexPCTN[] buffer = new VertexPCTN[verticesCount];
            for (int i = 0; i < verticesCount; i++)
            {
                buffer[i] = new VertexPCTN(vertices[i], new Color4(1f, 1f, 1f, 1f), textures[i], normals[i]);
            }
            vertexBuffer = new VertexBuffer(VertexPCTN.VertexInfo, buffer.Length, BufferUsageHint.DynamicDraw);
            indexBuffer = new IndexBuffer(indicesCount, BufferUsageHint.DynamicDraw);
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

        private void GetVerticesIndicesUV(string path, out List<Vector3> verticesList, out List<int> indicesList, out List<Vector2> textureList, out List<Vector3> normalsList)
        {
            
            List<string> lines = File.ReadLines(path).ToList();
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> textures = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<(int,int,int)> indices = new List<(int,int,int)>();
            for (int i = 0; i<lines.Count;i++)
            {
                string line = lines[i];
                if (line.Length > 0 && line.StartsWith("v "))
                {
                    string[] sectors = line.Split(' ');
                    vertices.Add(new Vector3(
                        float.Parse(sectors[1], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(sectors[2], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(sectors[3], CultureInfo.InvariantCulture.NumberFormat)
                        ));
                }
                if (line.Length > 0 && line.StartsWith("f "))
                {
                    string[] sectors = line.Split(' ').ToArray();
                    for(int j = 1; j < sectors.Length; j++)
                    {
                        int[] indexes = sectors[j].Split('/').Select(x => int.Parse(x) - 1).ToArray();
                        indices.Add((indexes[0], indexes[1], indexes[2]));
                    }
                }
                if (line.Length > 0 && line.StartsWith("vt "))
                {
                    string[] sectors = line.Split(' ');
                    textures.Add(new Vector2(
                        float.Parse(sectors[1], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(sectors[2], CultureInfo.InvariantCulture.NumberFormat)
                        ));
                }
                if(line.Length > 0 && line.StartsWith("vn "))
                {
                    string[] sectors = line.Split(' ');
                    normals.Add(new Vector3(
                        float.Parse(sectors[1], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(sectors[2], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(sectors[3], CultureInfo.InvariantCulture.NumberFormat)
                        ));
                } 
            }

            verticesList = new List<Vector3>();
            textureList = new List<Vector2>();
            normalsList = new List<Vector3>();
            indicesList = new List<int>();

            for(int i = 0; i < indices.Count; i++)
            {
                indicesList.Add(i);
                verticesList.Add(vertices[indices[i].Item1]);
                textureList.Add(textures[indices[i].Item2]);
                normalsList.Add(normals[indices[i].Item3]);
            }

            if (verticesList.Count != textureList.Count) throw new Exception("File is corrupted or not right format");
            if (verticesList.Count == 0 || textureList.Count == 0 || indicesList.Count == 0) throw new Exception("there are no vertices textures or indices");
        }
    }
}
