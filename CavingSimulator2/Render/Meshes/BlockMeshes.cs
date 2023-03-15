using CavingSimulator2.GameLogic.Components;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.Render.Meshes
{
    public class BlockMeshes
    {
        public Dictionary<int, BlockMeshBuffer> meshes = new Dictionary<int, BlockMeshBuffer>();
        public const float blockCut = 0.2f;

        // ^ y
        // |
        // O--> x
        
        //  6  7  8
        //  3  4  5
        //  0  1  2

        // 15 16 17
        // 12 13 14
        //  9 10 11

        // 24 25 26
        // 21 22 23
        // 18 19 20



        public enum FaceBufferId
        {
            top = 22,
            right = 14,
            front = 16,
            left = 12,
            back = 10,
            bottom = 4
        }

        public BlockMeshes()
        {
            meshes = new Dictionary<int, BlockMeshBuffer>();
            GenerateFaces();



        }

        private void GenerateFaces()
        {
            // face top
            meshes.Add(
                (int)FaceBufferId.top,
                new BlockMeshBuffer(
                    new VertexPOTTiN[]
                    {
                        new VertexPOTTiN(new Vector3(+0.5f, +0.5f, +0.5f),Vector3.Zero, Game.blockTextures.GetTextureCord(0,1, BlockFace.top), 0, Vector3.UnitZ),
                        new VertexPOTTiN(new Vector3(-0.5f, +0.5f, +0.5f),Vector3.Zero, Game.blockTextures.GetTextureCord(1,1, BlockFace.top), 0, Vector3.UnitZ),
                        new VertexPOTTiN(new Vector3(-0.5f, -0.5f, +0.5f),Vector3.Zero, Game.blockTextures.GetTextureCord(1,0, BlockFace.top), 0, Vector3.UnitZ),
                        new VertexPOTTiN(new Vector3(+0.5f, -0.5f, +0.5f),Vector3.Zero, Game.blockTextures.GetTextureCord(0,0, BlockFace.top), 0, Vector3.UnitZ),
                    },
                    new int[] { 0, 1, 2, 0, 2, 3 },
                    Matrix4.Identity)
                );
            
            // face bottom
            meshes.Add(
                (int)FaceBufferId.bottom,
                new BlockMeshBuffer(
                    new VertexPOTTiN[]
                    {
                        new VertexPOTTiN(new Vector3(+0.5f, +0.5f, +0.5f),Vector3.Zero, Game.blockTextures.GetTextureCord(0,1, BlockFace.bottom), 0, -Vector3.UnitZ),
                        new VertexPOTTiN(new Vector3(-0.5f, +0.5f, +0.5f),Vector3.Zero, Game.blockTextures.GetTextureCord(1,1, BlockFace.bottom), 0, -Vector3.UnitZ),
                        new VertexPOTTiN(new Vector3(-0.5f, -0.5f, +0.5f),Vector3.Zero, Game.blockTextures.GetTextureCord(1,0, BlockFace.bottom), 0, -Vector3.UnitZ),
                        new VertexPOTTiN(new Vector3(+0.5f, -0.5f, +0.5f),Vector3.Zero, Game.blockTextures.GetTextureCord(0,0, BlockFace.bottom), 0, -Vector3.UnitZ),
                    },
                    new int[] { 0, 1, 2, 0, 2, 3 },
                    Matrix4.CreateRotationX(MathHelper.DegreesToRadians(180f)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(180f)))
                );
            //return;
            // face right
            meshes.Add(
                (int)FaceBufferId.right,
                new BlockMeshBuffer(
                    new VertexPOTTiN[]
                    {
                        new VertexPOTTiN(new Vector3(+0.5f, +0.5f, +0.5f),Vector3.Zero, Game.blockTextures.GetTextureCord(0,1, BlockFace.right), 0, -Vector3.UnitX),
                        new VertexPOTTiN(new Vector3(-0.5f, +0.5f, +0.5f),Vector3.Zero, Game.blockTextures.GetTextureCord(1,1, BlockFace.right), 0, -Vector3.UnitX),
                        new VertexPOTTiN(new Vector3(-0.5f, -0.5f, +0.5f),Vector3.Zero, Game.blockTextures.GetTextureCord(1,0, BlockFace.right), 0, -Vector3.UnitX),
                        new VertexPOTTiN(new Vector3(+0.5f, -0.5f, +0.5f),Vector3.Zero, Game.blockTextures.GetTextureCord(0,0, BlockFace.right), 0, -Vector3.UnitX),
                    },
                    new int[] { 0, 1, 2, 0, 2, 3 },
                    Matrix4.CreateRotationX(MathHelper.DegreesToRadians(90f)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90f)))
                );
            
            // face left
            meshes.Add(
                (int)FaceBufferId.left,
                new BlockMeshBuffer(
                    new VertexPOTTiN[]
                    {
                        new VertexPOTTiN(new Vector3(+0.5f, +0.5f, +0.5f),Vector3.Zero, Game.blockTextures.GetTextureCord(0,1, BlockFace.left), 0, Vector3.UnitX),
                        new VertexPOTTiN(new Vector3(-0.5f, +0.5f, +0.5f),Vector3.Zero, Game.blockTextures.GetTextureCord(1,1, BlockFace.left), 0, Vector3.UnitX),
                        new VertexPOTTiN(new Vector3(-0.5f, -0.5f, +0.5f),Vector3.Zero, Game.blockTextures.GetTextureCord(1,0, BlockFace.left), 0, Vector3.UnitX),
                        new VertexPOTTiN(new Vector3(+0.5f, -0.5f, +0.5f),Vector3.Zero, Game.blockTextures.GetTextureCord(0,0, BlockFace.left), 0, Vector3.UnitX),
                    },
                    new int[] { 0, 1, 2, 0, 2, 3 },
                    Matrix4.CreateRotationX(MathHelper.DegreesToRadians(90f)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(-90f)))
                );
            
            // face foward
            meshes.Add(
                (int)FaceBufferId.back,
                new BlockMeshBuffer(
                    new VertexPOTTiN[]
                    {
                        new VertexPOTTiN(new Vector3(+0.5f, +0.5f, +0.5f),Vector3.Zero, Game.blockTextures.GetTextureCord(0,1, BlockFace.back), 0, -Vector3.UnitY),
                        new VertexPOTTiN(new Vector3(-0.5f, +0.5f, +0.5f),Vector3.Zero, Game.blockTextures.GetTextureCord(1,1, BlockFace.back), 0, -Vector3.UnitY),
                        new VertexPOTTiN(new Vector3(-0.5f, -0.5f, +0.5f),Vector3.Zero, Game.blockTextures.GetTextureCord(1,0, BlockFace.back), 0, -Vector3.UnitY),
                        new VertexPOTTiN(new Vector3(+0.5f, -0.5f, +0.5f),Vector3.Zero, Game.blockTextures.GetTextureCord(0,0, BlockFace.back), 0, -Vector3.UnitY),
                    },
                    new int[] { 0, 1, 2, 0, 2, 3 },
                    Matrix4.CreateRotationX(MathHelper.DegreesToRadians(90f)))
                );
            // face back
            meshes.Add(
                (int)FaceBufferId.front,
                new BlockMeshBuffer(
                    new VertexPOTTiN[]
                    {
                        new VertexPOTTiN(new Vector3(+0.5f, +0.5f, +0.5f),Vector3.Zero, Game.blockTextures.GetTextureCord(0,1, BlockFace.front), 0, Vector3.UnitY),
                        new VertexPOTTiN(new Vector3(-0.5f, +0.5f, +0.5f),Vector3.Zero, Game.blockTextures.GetTextureCord(1,1, BlockFace.front), 0, Vector3.UnitY),
                        new VertexPOTTiN(new Vector3(-0.5f, -0.5f, +0.5f),Vector3.Zero, Game.blockTextures.GetTextureCord(1,0, BlockFace.front), 0, Vector3.UnitY),
                        new VertexPOTTiN(new Vector3(+0.5f, -0.5f, +0.5f),Vector3.Zero, Game.blockTextures.GetTextureCord(0,0, BlockFace.front), 0, Vector3.UnitY),
                    },
                    new int[] { 0, 1, 2, 0, 2, 3 },
                    Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-90f)) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(180)))
                );
        }

    }
}
