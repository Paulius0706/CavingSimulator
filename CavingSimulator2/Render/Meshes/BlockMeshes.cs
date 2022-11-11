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
        public Dictionary<int, BlockMeshPart> meshes = new Dictionary<int, BlockMeshPart>();
        public const float blockCut = 0.2f;

        // O--> x
        // |
        // v y

        //  0  1  2
        //  3  4  5
        //  6  7  8

        //  9 10 11
        // 12 13 14
        // 15 16 17
        
        // 18 19 20
        // 21 22 23
        // 24 25 26
        public enum FaceBufferId
        {
            top = 22,
            right = 14,
            foward = 16,
            left = 12,
            back = 10,
            bottom = 4
        }

        public BlockMeshes()
        {
            meshes = new Dictionary<int, BlockMeshPart>();
            GenerateFaces();



        }

        private void GenerateFaces()
        {
            VertexPCTOT[] vertices = new VertexPCTOT[]
                    {
                        new VertexPCTOT(new Vector3(+0.5f, +0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(0,1, BlockFace.top), Vector3.Zero, 0),
                        new VertexPCTOT(new Vector3(-0.5f, +0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(1,1, BlockFace.top), Vector3.Zero, 0),
                        new VertexPCTOT(new Vector3(-0.5f, -0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(1,0, BlockFace.top), Vector3.Zero, 0),
                        new VertexPCTOT(new Vector3(+0.5f, -0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(0,0, BlockFace.top), Vector3.Zero, 0),
                    };
            int[] indices = new int[] { 0, 1, 2, 0, 2, 3 };
            // face top
            meshes.Add(
                (int)FaceBufferId.top,
                new BlockMeshPart(
                    new VertexPCTOT[]
                    {
                        new VertexPCTOT(new Vector3(+0.5f, +0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(0,1, BlockFace.top), Vector3.Zero, 0),
                        new VertexPCTOT(new Vector3(-0.5f, +0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(1,1, BlockFace.top), Vector3.Zero, 0),
                        new VertexPCTOT(new Vector3(-0.5f, -0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(1,0, BlockFace.top), Vector3.Zero, 0),
                        new VertexPCTOT(new Vector3(+0.5f, -0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(0,0, BlockFace.top), Vector3.Zero, 0),
                    },
                    new int[] { 0, 1, 2, 0, 2, 3 },
                    Matrix4.Identity)
                );
            
            // face bottom
            meshes.Add(
                (int)FaceBufferId.bottom,
                new BlockMeshPart(
                    new VertexPCTOT[]
                    {
                        new VertexPCTOT(new Vector3(+0.5f, +0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(0,1, BlockFace.bottom), Vector3.Zero, 0),
                        new VertexPCTOT(new Vector3(-0.5f, +0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(1,1, BlockFace.bottom), Vector3.Zero, 0),
                        new VertexPCTOT(new Vector3(-0.5f, -0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(1,0, BlockFace.bottom), Vector3.Zero, 0),
                        new VertexPCTOT(new Vector3(+0.5f, -0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(0,0, BlockFace.bottom), Vector3.Zero, 0),
                    },
                    new int[] { 0, 1, 2, 0, 2, 3 },
                    Matrix4.CreateRotationX(MathHelper.DegreesToRadians(180f)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(180f)))
                );
            //return;
            // face right
            meshes.Add(
                (int)FaceBufferId.right,
                new BlockMeshPart(
                    new VertexPCTOT[]
                    {
                        new VertexPCTOT(new Vector3(+0.5f, +0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(0,1, BlockFace.right), Vector3.Zero, 0),
                        new VertexPCTOT(new Vector3(-0.5f, +0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(1,1, BlockFace.right), Vector3.Zero, 0),
                        new VertexPCTOT(new Vector3(-0.5f, -0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(1,0, BlockFace.right), Vector3.Zero, 0),
                        new VertexPCTOT(new Vector3(+0.5f, -0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(0,0, BlockFace.right), Vector3.Zero, 0),
                    },
                    new int[] { 0, 1, 2, 0, 2, 3 },
                    Matrix4.CreateRotationX(MathHelper.DegreesToRadians(90f)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90f)))
                );
            
            // face left
            meshes.Add(
                (int)FaceBufferId.left,
                new BlockMeshPart(
                    new VertexPCTOT[]
                    {
                        new VertexPCTOT(new Vector3(+0.5f, +0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(0,1, BlockFace.left), Vector3.Zero, 0),
                        new VertexPCTOT(new Vector3(-0.5f, +0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(1,1, BlockFace.left), Vector3.Zero, 0),
                        new VertexPCTOT(new Vector3(-0.5f, -0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(1,0, BlockFace.left), Vector3.Zero, 0),
                        new VertexPCTOT(new Vector3(+0.5f, -0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(0,0, BlockFace.left), Vector3.Zero, 0),
                    },
                    new int[] { 0, 1, 2, 0, 2, 3 },
                    Matrix4.CreateRotationX(MathHelper.DegreesToRadians(90f)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(-90f)))
                );
            
            // face foward
            meshes.Add(
                (int)FaceBufferId.back,
                new BlockMeshPart(
                    new VertexPCTOT[]
                    {
                        new VertexPCTOT(new Vector3(+0.5f, +0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(0,1, BlockFace.back), Vector3.Zero, 0),
                        new VertexPCTOT(new Vector3(-0.5f, +0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(1,1, BlockFace.back), Vector3.Zero, 0),
                        new VertexPCTOT(new Vector3(-0.5f, -0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(1,0, BlockFace.back), Vector3.Zero, 0),
                        new VertexPCTOT(new Vector3(+0.5f, -0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(0,0, BlockFace.back), Vector3.Zero, 0),
                    },
                    new int[] { 0, 1, 2, 0, 2, 3 },
                    Matrix4.CreateRotationX(MathHelper.DegreesToRadians(90f)))
                );
            // face back
            meshes.Add(
                (int)FaceBufferId.foward,
                new BlockMeshPart(
                    new VertexPCTOT[]
                    {
                        new VertexPCTOT(new Vector3(+0.5f, +0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(0,1, BlockFace.foward), Vector3.Zero, 0),
                        new VertexPCTOT(new Vector3(-0.5f, +0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(1,1, BlockFace.foward), Vector3.Zero, 0),
                        new VertexPCTOT(new Vector3(-0.5f, -0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(1,0, BlockFace.foward), Vector3.Zero, 0),
                        new VertexPCTOT(new Vector3(+0.5f, -0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(0,0, BlockFace.foward), Vector3.Zero, 0),
                    },
                    new int[] { 0, 1, 2, 0, 2, 3 },
                    Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-90f)) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(180)))
                );
        }

    }
}
