using CavingSimulator.GameLogic.Components;
using CavingSimulator.Render;
using CavingSimulator2.GameLogic.Components.Colliders;
using CavingSimulator2.GameLogic.Components.Noises;
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

namespace CavingSimulator2.GameLogic.Components
{
    public class ChunkGenerator
    {
        Transform transform;

        public static Dictionary<Vector3i, Chunk> chunks = new Dictionary<Vector3i, Chunk>();
        public Queue<Vector3i> chunkMeshUpdates = new Queue<Vector3i>();
        public static FastNoise noise = new FastNoise();
        public int loadDistance = 8;

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
                    
                    if (!chunks.ContainsKey(pos) && (transform.GlobalPosition - Vector3.UnitZ * transform.GlobalPosition.Z - (Vector3)(pos * Chunk.size)).Length < loadDistance * Chunk.size)
                    {
                        chunks.Add(pos, new Chunk(pos));
                        
                        // TODO: there should be bug when loading few chunks at the time
                        if (chunks.ContainsKey(pos + Vector3i.UnitX)) chunkMeshUpdates.Enqueue(pos + Vector3i.UnitX);
                        if (chunks.ContainsKey(pos - Vector3i.UnitX)) chunkMeshUpdates.Enqueue(pos - Vector3i.UnitX);
                        if (chunks.ContainsKey(pos + Vector3i.UnitY)) chunkMeshUpdates.Enqueue(pos + Vector3i.UnitY);
                        if (chunks.ContainsKey(pos - Vector3i.UnitY)) chunkMeshUpdates.Enqueue(pos - Vector3i.UnitY);
                    }
                }
            }
            // unsafe
            if (chunkMeshUpdates.Count > 0)
            {
                Vector3i chunk = chunkMeshUpdates.Dequeue();
                if (chunks.ContainsKey(chunk)) chunks[chunk].UpdateMeshes();
                else chunkMeshUpdates.Enqueue(chunk);
            }
            var keys = chunks.Keys;
            foreach (Vector3i key in keys) 
            {
                if ((transform.GlobalPosition - Vector3.UnitZ * transform.GlobalPosition.Z - (Vector3)(key * Chunk.size)).Length > loadDistance * Chunk.size) 
                {
                    chunks[key].Dispose();
                    chunks.Remove(key);
                }
            }
            

            foreach (Chunk chunk in chunks.Values){ chunk.Update(); }
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


        public class Chunk : IDisposable
        {
            private bool disposed;
            public const int size = 16;
            public const int maxHeight = 10;

            public readonly Vector3i chunkPosition;
            public readonly Vector3i chunkPositionOffset;
            public readonly Vector3i chunkPositionOffset1;
            private Dictionary<Vector3i, Block> blocks = new Dictionary<Vector3i, Block>();
            public Dictionary<int, BlockChunkMesh> meshes = new Dictionary<int, BlockChunkMesh>();


            private bool loaded;
            private bool meshed;
            public Chunk(Vector3i chunkPosition)
            {
                this.chunkPosition = chunkPosition;
                chunkPositionOffset = new Vector3i(this.chunkPosition.X, this.chunkPosition.Y, 0) * size;
                chunkPositionOffset1 = new Vector3i(chunkPositionOffset.X + size - 1, chunkPositionOffset.Y + size - 1, 0);
                //Task task = GenerateBlocks();

            }
            ~Chunk()
            {
                Dispose();
            }
            
            public void Update()
            {
                GenerateBlocks();
                GenerateMeshes();
            }
            public void UpdateMeshes() { meshed = false; }

            private void GenerateBlocks()
            {
                if (loaded) return;
                Random random = new Random();
                for (int x = chunkPositionOffset.X; x < chunkPositionOffset.X + size; x++)
                {
                    for (int y = chunkPositionOffset.Y; y < chunkPositionOffset.Y + size; y++)
                    {
                        //if (x != chunkPositionOffset.X && x != chunkPositionOffset1.X && y != chunkPositionOffset.Y && y != chunkPositionOffset1.Y) continue;
                        //int height = (int)(ChunkGenerator.noise.GetPerlin(x, y) * 10f) + 5;
                        int height = HeightNoise.GetHeight(x, y);
                        for (int z = 0; z <= height; z++)
                        {
                            Vector3i blockPos = new Vector3i(x, y, z);
                            //random.Next(3) 
                            Block block = new Block() { position = blockPos, id = 1 };
                            blocks.Add(blockPos, block);
                        }
                    }
                }
                loaded = true;
            }
            private void GenerateMeshes()
            {
                if (!loaded || meshed) return;
                SetMeshes();
                string neighbors;
                foreach (Block block in blocks.Values)
                {
                    neighbors = "";
                    for(int z= -1; z <= 1; z++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            for (int x = -1; x <= 1; x++)
                            {
                                if(FullBlockExist(block.position + Vector3i.UnitZ * z + Vector3i.UnitY * y + Vector3i.UnitX * x)){ neighbors += "1"; }
                                else { neighbors += "0"; }
                            }
                        }
                    }
                    block.mesh = neighbors;
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




            
            

            public void SetMeshes()
            {
                if (meshes != null) foreach (BlockChunkMesh mesh in meshes.Values) mesh.Dispose();
                meshes.Clear();
                for(int i = 0; i < 3 * 3 * 3; i++)
                {
                    if (Game.blockMeshes.meshes.ContainsKey(i))
                        meshes.Add(i, new BlockChunkMesh(Game.blockMeshes.meshes[i].vertexBuffer));
                }
            }
            

            private void SetRenderBuffer()
            {
                foreach (int key in meshes.Keys)
                {
                    VertexPCI[] positions = blocks.Values.
                        Where(block => block.mesh[key] == '0').
                        Select(block => 
                            new VertexPCI(
                                (Vector3)block.position ,
                                Game.blockMeshes.SetColor(block.mesh,key),
                                //new Color4(1f, 1f, 1f, 1f),
                                block.id )).ToArray();
                    if(positions.Length > 0)
                    {
                        meshes[key].active = true;
                        meshes[key].instanceBuffer = new VertexBuffer(VertexPCI.VertexInfo, positions.Length, BufferUsageHint.StaticDraw);
                        meshes[key].instanceBuffer.SetData(ref positions, positions.Length);

                        GL.BindVertexArray(meshes[key].vertexArray.VertexArrayHandle);
                        GL.BindBuffer(BufferTarget.ElementArrayBuffer, Game.blockMeshes.meshes[key].indexBuffer.IndexBufferHandle);
                        GL.BindBuffer(BufferTarget.ArrayBuffer, meshes[key].instanceBuffer.VertexBufferHandle);

                        GL.VertexAttribPointer(
                            VertexPCTOTI.VertexInfo.VertexAttributes[3].Index,
                            VertexPCTOTI.VertexInfo.VertexAttributes[3].ComponentCount,
                            VertexAttribPointerType.Float, false,
                            VertexPCI.VertexInfo.SizeInBytes,
                            VertexPCI.VertexInfo.VertexAttributes[0].Offset);
                        GL.EnableVertexAttribArray(VertexPCTOTI.VertexInfo.VertexAttributes[3].Index);

                        GL.VertexAttribPointer(
                            VertexPCTOTI.VertexInfo.VertexAttributes[1].Index,
                            VertexPCTOTI.VertexInfo.VertexAttributes[1].ComponentCount,
                            VertexAttribPointerType.Float, false,
                            VertexPCI.VertexInfo.SizeInBytes,
                            VertexPCI.VertexInfo.VertexAttributes[1].Offset);
                        GL.EnableVertexAttribArray(VertexPCTOTI.VertexInfo.VertexAttributes[1].Index);

                        GL.VertexAttribPointer(
                            VertexPCTOTI.VertexInfo.VertexAttributes[4].Index,
                            VertexPCTOTI.VertexInfo.VertexAttributes[4].ComponentCount,
                            VertexAttribPointerType.Float, false,
                            VertexPCI.VertexInfo.SizeInBytes,
                            VertexPCI.VertexInfo.VertexAttributes[2].Offset);
                        GL.EnableVertexAttribArray(VertexPCTOTI.VertexInfo.VertexAttributes[4].Index);


                        GL.VertexAttribDivisor(3, 1);
                        GL.VertexAttribDivisor(1, 1);
                        GL.VertexAttribDivisor(4, 1);


                        GL.BindVertexArray(0);

                        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                    }
                    
                }
                foreach (Block block in blocks.Values) block.mesh = null;

            }
            

            
            public void InstanceRender()
            {
                //Game.textures[Game.textures.GetIndex("grassBlock")].UploadTexture();
                foreach (int key in meshes.Keys)
                {
                    if (meshes[key].active)
                    {
                        GL.BindVertexArray(meshes[key].vertexArray.VertexArrayHandle);
                        GL.DrawElementsInstanced(PrimitiveType.Triangles, Game.blockMeshes.meshes[key].indicesCount, DrawElementsType.UnsignedInt, IntPtr.Zero, meshes[key].instanceBuffer.VertexCount);
                        GL.BindVertexArray(0);
                    }
                    
                }
            }

            public void Dispose()
            {
                if (disposed) return;
                disposed = true;

                foreach (BlockChunkMesh mesh in meshes.Values) mesh.Dispose();
            }

            public class Block
            {
                public Vector3i position;
                public int id = -1;
                public string mesh = "";


                public static Collider ConstructBlockCollider(Vector3i position)
                {
                    return new Collider(
                        new Transform(position),
                        new Vector2(-0.5f, 0.5f),
                        new Vector2(-0.5f, 0.5f),
                        new Vector2(-0.5f, 0.5f),
                        Vector3.Zero,
                        Vector3i.One * 2);
                }

            }
        }



    }
}
