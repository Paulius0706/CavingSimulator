
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CavingSimulator2.Render.Meshes;
using CavingSimulator.GameLogic.Components;
using CavingSimulator2.Render;

namespace CavingSimulator2.Render.Meshes.SpaceShipParts
{
    public class BoxMesh : Mesh
    {
        public BoxMesh() : base() { }
        public BoxMesh(Transform transform, bool stoplastAtributeBind = false) : base(transform, "", stoplastAtributeBind) { }
        public BoxMesh(Transform transform, string texture = "", bool stoplastAtributeBind = false) : base(transform, texture, stoplastAtributeBind) { }

        protected override int SetVerticesCount() { return 4 * 6; }
        protected override int SetInidicesCount() { return 6 * 6; }

        protected override void GenerateBuffersAndVertices()
        {
            int verticesCounter = 0;
            int indicesCounter = 0;

            //top
            GenerateQuad(ref verticesCounter, ref indicesCounter, new Vector3[]
            {

                new Vector3(+ 0.5f, + 0.5f, + 0.5f),
                new Vector3(- 0.5f, + 0.5f, + 0.5f),

                new Vector3(- 0.5f, - 0.5f, + 0.5f),
                new Vector3(+ 0.5f, - 0.5f, + 0.5f)
            });
            //down
            GenerateQuad(ref verticesCounter, ref indicesCounter, new Vector3[]
            {
                new Vector3(- 0.5f, + 0.5f, - 0.5f),
                new Vector3(+ 0.5f, + 0.5f, - 0.5f),

                new Vector3(+ 0.5f, - 0.5f, - 0.5f),
                new Vector3(- 0.5f, - 0.5f, - 0.5f)
            });

            //right
            GenerateQuad(ref verticesCounter, ref indicesCounter, new Vector3[]
            {
                new Vector3(+ 0.5f, - 0.5f, + 0.5f),
                new Vector3(+ 0.5f, + 0.5f, + 0.5f),

                new Vector3(+ 0.5f, + 0.5f, - 0.5f),
                new Vector3(+ 0.5f, - 0.5f, - 0.5f)
            });
            //left
            GenerateQuad(ref verticesCounter, ref indicesCounter, new Vector3[]
            {
                new Vector3(- 0.5f, + 0.5f, + 0.5f),
                new Vector3(- 0.5f, - 0.5f, + 0.5f),

                new Vector3(- 0.5f, - 0.5f, - 0.5f),
                new Vector3(- 0.5f, + 0.5f, - 0.5f)
            });

            //back
            GenerateQuad(ref verticesCounter, ref indicesCounter, new Vector3[]
            {
                new Vector3(- 0.5f, - 0.5f, + 0.5f),
                new Vector3(+ 0.5f, - 0.5f, + 0.5f),

                new Vector3(+ 0.5f, - 0.5f, - 0.5f),
                new Vector3(- 0.5f, - 0.5f, - 0.5f)
            });
            //foward
            GenerateQuad(ref verticesCounter, ref indicesCounter, new Vector3[]
            {
                new Vector3(- 0.5f, + 0.5f, + 0.5f),
                new Vector3(+ 0.5f, + 0.5f, + 0.5f),

                new Vector3(+ 0.5f, + 0.5f, - 0.5f),
                new Vector3(- 0.5f, + 0.5f, - 0.5f)
            });



        }
        private void GenerateQuad(ref int verticesCounter, ref int indicesCounter, Vector3[] positions)
        {
            float r = (float)randomColorator.NextDouble();
            float g = (float)randomColorator.NextDouble();
            float b = (float)randomColorator.NextDouble();


            indices[indicesCounter++] = verticesCounter + 0;
            indices[indicesCounter++] = verticesCounter + 1;
            indices[indicesCounter++] = verticesCounter + 2;
            indices[indicesCounter++] = verticesCounter + 0;
            indices[indicesCounter++] = verticesCounter + 2;
            indices[indicesCounter++] = verticesCounter + 3;

            buffer[verticesCounter++] = new VertexPCT(positions[0], new Color4(r, g, b, 1f), new Vector2(0, 1));
            buffer[verticesCounter++] = new VertexPCT(positions[1], new Color4(r, g, b, 1f), new Vector2(1, 1));
            buffer[verticesCounter++] = new VertexPCT(positions[2], new Color4(r, g, b, 1f), new Vector2(1, 0));
            buffer[verticesCounter++] = new VertexPCT(positions[3], new Color4(r, g, b, 1f), new Vector2(0, 0));

        }
    }
}
