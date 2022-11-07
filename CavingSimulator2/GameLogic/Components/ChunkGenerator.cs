using CavingSimulator.GameLogic.Components;
using CavingSimulator.Render;
using CavingSimulator2.GameLogic.Objects;
using CavingSimulator2.Render;
using CavingSimulator2.Render.Meshes;
using DotnetNoise;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static CavingSimulator2.GameLogic.Components.ChunkGenerator.Chunk;
using static OpenTK.Graphics.OpenGL.GL;

namespace CavingSimulator2.GameLogic.Components
{
    public class ChunkGenerator : Component
    {
        Transform transform;

        private static Dictionary<Vector3i, Chunk> chunks = new Dictionary<Vector3i, Chunk>();
        public static FastNoise noise = new FastNoise();
        public int loadDistance = 4;

        public ChunkGenerator(Transform transform)
        {
            this.transform = transform;
            noise.UsedNoiseType = FastNoise.NoiseType.Value;

            noise.Frequency = 0.05f;
            noise.Lacunarity = 2f;
            noise.Gain = 0.5f;
            noise.Octaves = 5;
            noise.FractalTypeMethod = FastNoise.FractalType.Fbm;
        }
        public Vector3i targetChunk { get; private set; }

        public void Update()
        {
            //Console.WriteLine(chunks.Count);
            targetChunk = getTargetChunk(transform.GlobalPosition);
            //Console.WriteLine(targetChunk + " " + gameObject.Transform.GlobalPosition);
            for (int x = -loadDistance; x <= loadDistance; x++)
            {
                for (int y = -loadDistance; y <= loadDistance; y++)
                {
                    Vector3i pos = new Vector3i((int)targetChunk.X + x, (int)targetChunk.Y + y, 0);
                    if (!chunks.ContainsKey(pos))
                    {
                        chunks.Add(pos, new Chunk(pos));
                        // update neighborMeshes
                        //if (chunks.ContainsKey(pos + Vector3i.UnitX)) chunks[pos + Vector3i.UnitX].UpdateEdge(pos - Vector3i.UnitX);
                        //if (chunks.ContainsKey(pos - Vector3i.UnitX)) chunks[pos - Vector3i.UnitX].UpdateEdge(pos + Vector3i.UnitX);
                        //if (chunks.ContainsKey(pos + Vector3i.UnitY)) chunks[pos + Vector3i.UnitY].UpdateEdge(pos - Vector3i.UnitY);
                        //if (chunks.ContainsKey(pos - Vector3i.UnitY)) chunks[pos - Vector3i.UnitY].UpdateEdge(pos + Vector3i.UnitY);
                    }
                }
            }
            foreach(Chunk chunk in chunks.Values){ chunk.Update(); }
        }
        public void Render()
        {
            foreach(Chunk chunk in chunks.Values)
            {
                chunk.InstanceRender();
                //chunk.Render();
            }
        }

        public static Vector3i getTargetChunk(Vector3 objectPosition)
        {
            return new Vector3i((int)Math.Floor(objectPosition.X / Chunk.size), (int)Math.Floor(objectPosition.Y / Chunk.size), 0);
        }

        public static bool FullBlockExist(Vector3i chunk, Vector3i position)
        {
            //Console.WriteLine("try get value:" + position + "of chunk" + chunk);
            if (!chunks.ContainsKey(chunk)) { /*Console.WriteLine(chunk + " chunk dont exist");*/ return true; }
            //Console.WriteLine(chunk + " chunk is found");
            return chunks[chunk].FullBlockExist(position);
        }

        public static void UpdateEdgesOfNewChunk(Vector3i position)
        {
            if (chunks.ContainsKey(position + Vector3i.UnitX)) chunks[position + Vector3i.UnitX].UpdateEdge(position - Vector3i.UnitX);
            if (chunks.ContainsKey(position - Vector3i.UnitX)) chunks[position - Vector3i.UnitX].UpdateEdge(position + Vector3i.UnitX);

            if (chunks.ContainsKey(position + Vector3i.UnitY)) chunks[position + Vector3i.UnitY].UpdateEdge(position - Vector3i.UnitY);
            if (chunks.ContainsKey(position - Vector3i.UnitY)) chunks[position - Vector3i.UnitY].UpdateEdge(position + Vector3i.UnitY);
        }

        public class Chunk
        {
            public const int size = 16;
            public const int maxHeight = 10;

            public readonly Vector3i chunkPosition;
            public readonly Vector3i chunkPositionOffset;
            public readonly Vector3i chunkPositionOffset1;
            private Dictionary<Vector3i, Block> blocks = new Dictionary<Vector3i, Block>();
            private Queue<Vector3i> UpdateMeshesQueue = new Queue<Vector3i>();


            private bool loaded;
            private bool meshed;
            public Chunk(Vector3i chunkPosition)
            {
                this.chunkPosition = chunkPosition;
                chunkPositionOffset = new Vector3i(this.chunkPosition.X, this.chunkPosition.Y, 0) * size;
                chunkPositionOffset1 = new Vector3i(chunkPositionOffset.X + size - 1, chunkPositionOffset.Y + size - 1, 0);
                //Task task = GenerateBlocks();

            }
            public void UpdateEdge(Vector3i edge)
            {

                switch ((edge.X, edge.Y))
                {
                    case (1, 0):
                        for (int y = chunkPositionOffset.Y; y <= chunkPositionOffset1.Y; y++) for (int z = 0; z < maxHeight; z++)
                                UpdateMeshQueue(new Vector3i(chunkPositionOffset1.X, y, z)); break;
                    case (-1, 0):
                        for (int y = chunkPositionOffset.Y; y <= chunkPositionOffset1.Y; y++) for (int z = 0; z < maxHeight; z++)
                                UpdateMeshQueue(new Vector3i(chunkPositionOffset.X, y, z)); break;

                    case (0, 1):
                        for (int x = chunkPositionOffset.X; x <= chunkPositionOffset1.X; x++) for (int z = 0; z < maxHeight; z++)
                                UpdateMeshQueue(new Vector3i(x, chunkPositionOffset1.Y, z)); break;
                    case (0, -1):
                        for (int x = chunkPositionOffset.X; x <= chunkPositionOffset1.X; x++) for (int z = 0; z < maxHeight; z++)
                                UpdateMeshQueue(new Vector3i(x, chunkPositionOffset.Y, z)); break;
                }
            }
            public void Update()
            {
                GenerateBlocks();
                GenerateMeshes();
            }

            private void GenerateBlocks()
            {
                if (loaded) return;
                for (int x = chunkPositionOffset.X; x < chunkPositionOffset.X + size; x++)
                {
                    for (int y = chunkPositionOffset.Y; y < chunkPositionOffset.Y + size; y++)
                    {
                        //if (x != chunkPositionOffset.X && x != chunkPositionOffset1.X && y != chunkPositionOffset.Y && y != chunkPositionOffset1.Y) continue;
                        int height = (int)(ChunkGenerator.noise.GetPerlin(x, y) * 10f) + 5;
                        for (int z = 0; z <= height; z++)
                        {
                            Vector3i blockPos = new Vector3i(x, y, z);
                            Block block = new Block() { position = blockPos, texture = Game.textures.GetIndex("grassBlock") };
                            blocks.Add(blockPos, block);
                            //Instantiate(block);
                        }
                    }
                }
                //Console.WriteLine("Chunk:"+chunkPosition+" gen blocksC:" + blocks.Count);
                loaded = true;
                ChunkGenerator.UpdateEdgesOfNewChunk(chunkPosition);
            }
            private void GenerateMeshes()
            {
                if (!loaded || meshed) return;
                foreach (Block block in blocks.Values)
                {
                    block.mesh = "" +
                        (FullBlockExist(block.position + Vector3i.UnitZ) ? "0" : "1") +
                        (FullBlockExist(block.position - Vector3i.UnitZ) ? "0" : "1") +
                        (FullBlockExist(block.position + Vector3i.UnitX) ? "0" : "1") +
                        (FullBlockExist(block.position - Vector3i.UnitX) ? "0" : "1") +
                        (FullBlockExist(block.position + Vector3i.UnitY) ? "0" : "1") +
                        (FullBlockExist(block.position - Vector3i.UnitY) ? "0" : "1");
                    if(block.mesh != "000000") SetMeshes(block.mesh);
                }
                SetRenderBuffer();

                meshed = true;
            }

            public bool FullBlockExist(Vector3i position)
            {
                if (position.X < chunkPositionOffset.X) { /*Console.WriteLine("redirect" + (chunkPosition - Vector3i.UnitX));*/ return ChunkGenerator.FullBlockExist(chunkPosition - Vector3i.UnitX, position); }
                if (position.X > chunkPositionOffset1.X) { /*Console.WriteLine("redirect" + (chunkPosition + Vector3i.UnitX));*/ return ChunkGenerator.FullBlockExist(chunkPosition + Vector3i.UnitX, position); }
                if (position.Y < chunkPositionOffset.Y) { /*Console.WriteLine("redirect" + (chunkPosition - Vector3i.UnitY));*/ return ChunkGenerator.FullBlockExist(chunkPosition - Vector3i.UnitY, position); }
                if (position.Y > chunkPositionOffset1.Y) { /*Console.WriteLine("redirect" + (chunkPosition + Vector3i.UnitY));*/ return ChunkGenerator.FullBlockExist(chunkPosition + Vector3i.UnitY, position); }
                //Console.WriteLine("return correct stuff");
                return blocks.ContainsKey(position);
            }



            //Queue Commands
            public void UpdateMeshQueue(Vector3i position) { UpdateMeshesQueue.Enqueue(position); }



            
            public Dictionary<string, BlockMesh> meshes = new Dictionary<string, BlockMesh>();
            public void SetMeshes(string mesh)
            {
                if (!meshes.ContainsKey(mesh)) meshes.Add(mesh, new BlockMesh(mesh, false));
                //meshes[mesh].instances++;
            }
            
            private void SetRenderBuffer()
            {
                //Console.WriteLine("SetRenderBuffer:" + chunkPosition + " C:" + meshes.Count);
                foreach (BlockMesh mesh in meshes.Values)
                {
                    VertexP[] positions = blocks.Values.Where(block => block.mesh == mesh.key).Select(block => new VertexP((Vector3)block.position)).ToArray();

                    mesh.positionsBuffer = new VertexBuffer(VertexP.VertexInfo, positions.Length, BufferUsageHint.StaticDraw);
                    mesh.positionsBuffer.SetData(ref positions, positions.Length);

                    GL.BindVertexArray(mesh.vertexArray.VertexArrayHandle);
                    //GL.BindBuffer(BufferTarget.ArrayBuffer, mesh.vertexBuffer.VertexBufferHandle);
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.indexBuffer.IndexBufferHandle);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, mesh.positionsBuffer.VertexBufferHandle);

                    GL.VertexAttribPointer(
                        VertexPCTO.VertexInfo.VertexAttributes[3].Index,
                        VertexPCTO.VertexInfo.VertexAttributes[3].ComponentCount,
                        VertexAttribPointerType.Float, false,
                        VertexP.VertexInfo.SizeInBytes,
                        VertexP.VertexInfo.VertexAttributes[0].Offset);

                    GL.EnableVertexAttribArray(VertexPCTO.VertexInfo.VertexAttributes[3].Index);
                    GL.VertexAttribDivisor(3, 1);
                    GL.BindVertexArray(0);

                    GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                    //Console.WriteLine("     Register mesh:" + mesh.key + " C:" + mesh.positionsBuffer.VertexCount);

                }

            }

            
            public void InstanceRender()
            {
                //Console.WriteLine("ChunkR :" + chunkPosition);
                //Console.WriteLine("ChunkRC:" + meshes.Count);
                Game.textures[Game.textures.GetIndex("grassBlock")].UploadTexture();
                foreach (BlockMesh mesh in meshes.Values)
                {
                    //Console.WriteLine("     MeshesR Count:" + mesh.positionsBuffer.VertexCount);
                    GL.BindVertexArray(mesh.vertexArray.VertexArrayHandle);
                    GL.DrawElementsInstanced(PrimitiveType.Triangles, mesh.indicesCount, DrawElementsType.UnsignedInt, IntPtr.Zero, mesh.positionsBuffer.VertexCount);
                    //GL.DrawElements(PrimitiveType.Triangles, mesh.indicesCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
                    GL.BindVertexArray(0);
                }
            }
            public void Render()
            {
                foreach(Block block in blocks.Values)
                {
                    if (block.mesh == "000000") continue;
                    Game.shaderPrograms.Current.SetUniform("Offset", block.position);
                    if (block.texture != -1) Game.textures[block.texture].UploadTexture();
                    GL.BindVertexArray(meshes[block.mesh].vertexArray.VertexArrayHandle);
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, meshes[block.mesh].indexBuffer.IndexBufferHandle);
                    GL.DrawElements(PrimitiveType.Triangles, meshes[block.mesh].indicesCount, DrawElementsType.UnsignedInt, 0);
                    GL.BindTexture(TextureTarget.Texture2D, 0);

                }
            }
            

            public class Block
            {
                public Vector3i position;
                public int texture = -1;
                public string mesh = "000000";

            }
        }



    }
}
