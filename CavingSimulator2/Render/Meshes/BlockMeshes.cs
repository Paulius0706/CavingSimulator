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
        public Dictionary<int, BlockMeshPart> meshes = new Dictionary<int, BlockMeshPart>();
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
        private const float blockCoverage = 0.2f;
        // 1 0
        // 2 3
        public Color4 SetColor(string neighbors, int face) 
        {
            //if (face != (int)FaceBufferId.top) return Color4.White;
            (int, int, int, int) axis = FaceAxis(face);
            float x0 = 1f;
            float x1 = 1f;
            float x2 = 1f;
            float x3 = 1f;

            x0 -= neighbors[axis.Item1] == '1' ? blockCoverage : 0f;
            x0 -= neighbors[axis.Item3] == '1' ? blockCoverage : 0f;
            
            x1 -= neighbors[axis.Item1] == '1' ? blockCoverage : 0f;
            x1 -= neighbors[axis.Item4] == '1' ? blockCoverage : 0f;

            x2 -= neighbors[axis.Item2] == '1' ? blockCoverage : 0f;
            x2 -= neighbors[axis.Item4] == '1' ? blockCoverage : 0f;

            x3 -= neighbors[axis.Item2] == '1' ? blockCoverage : 0f;
            x3 -= neighbors[axis.Item3] == '1' ? blockCoverage : 0f;

            return new Color4(x0,x1, x2, x3);
        }
        // front, left, right, back
        private (int, int, int, int) FaceAxis(int face)
        {
            switch (face)
            {
                //                                front, left, right, back
                case (int)FaceBufferId.top:    return (25, 19, 23, 21);
                case (int)FaceBufferId.right:  return (23,  5, 17, 11);

                case (int)FaceBufferId.front:  return (25,  7, 15, 17);

                case (int)FaceBufferId.left:   return (21,  3,  9, 15);
                case (int)FaceBufferId.back:   return (19,  1, 11,  9);
                case (int)FaceBufferId.bottom: return ( 7,  1,  5,  3);
            }
            return (13,13,13,13);
        }

        public BlockMeshes()
        {
            meshes = new Dictionary<int, BlockMeshPart>();
            GenerateFaces();



        }

        private void GenerateFaces()
        {
            // face top
            meshes.Add(
                (int)FaceBufferId.top,
                new BlockMeshPart(
                    new VertexPCTOTI[]
                    {
                        new VertexPCTOTI(new Vector3(+0.5f, +0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(0,1, BlockFace.top), Vector3.Zero, 0,0f),
                        new VertexPCTOTI(new Vector3(-0.5f, +0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(1,1, BlockFace.top), Vector3.Zero, 0,1f),
                        new VertexPCTOTI(new Vector3(-0.5f, -0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(1,0, BlockFace.top), Vector3.Zero, 0,2f),
                        new VertexPCTOTI(new Vector3(+0.5f, -0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(0,0, BlockFace.top), Vector3.Zero, 0,3f),
                    },
                    new int[] { 0, 1, 2, 0, 2, 3 },
                    Matrix4.Identity)
                );
            
            // face bottom
            meshes.Add(
                (int)FaceBufferId.bottom,
                new BlockMeshPart(
                    new VertexPCTOTI[]
                    {
                        new VertexPCTOTI(new Vector3(+0.5f, +0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(0,1, BlockFace.bottom), Vector3.Zero, 0,0f),
                        new VertexPCTOTI(new Vector3(-0.5f, +0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(1,1, BlockFace.bottom), Vector3.Zero, 0,1f),
                        new VertexPCTOTI(new Vector3(-0.5f, -0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(1,0, BlockFace.bottom), Vector3.Zero, 0,2f),
                        new VertexPCTOTI(new Vector3(+0.5f, -0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(0,0, BlockFace.bottom), Vector3.Zero, 0,3f),
                    },
                    new int[] { 0, 1, 2, 0, 2, 3 },
                    Matrix4.CreateRotationX(MathHelper.DegreesToRadians(180f)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(180f)))
                );
            //return;
            // face right
            meshes.Add(
                (int)FaceBufferId.right,
                new BlockMeshPart(
                    new VertexPCTOTI[]
                    {
                        new VertexPCTOTI(new Vector3(+0.5f, +0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(0,1, BlockFace.right), Vector3.Zero, 0,0f),
                        new VertexPCTOTI(new Vector3(-0.5f, +0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(1,1, BlockFace.right), Vector3.Zero, 0,1f),
                        new VertexPCTOTI(new Vector3(-0.5f, -0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(1,0, BlockFace.right), Vector3.Zero, 0,2f),
                        new VertexPCTOTI(new Vector3(+0.5f, -0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(0,0, BlockFace.right), Vector3.Zero, 0,3f),
                    },
                    new int[] { 0, 1, 2, 0, 2, 3 },
                    Matrix4.CreateRotationX(MathHelper.DegreesToRadians(90f)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(90f)))
                );
            
            // face left
            meshes.Add(
                (int)FaceBufferId.left,
                new BlockMeshPart(
                    new VertexPCTOTI[]
                    {
                        new VertexPCTOTI(new Vector3(+0.5f, +0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(0,1, BlockFace.left), Vector3.Zero, 0,0f),
                        new VertexPCTOTI(new Vector3(-0.5f, +0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(1,1, BlockFace.left), Vector3.Zero, 0,1f),
                        new VertexPCTOTI(new Vector3(-0.5f, -0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(1,0, BlockFace.left), Vector3.Zero, 0,2f),
                        new VertexPCTOTI(new Vector3(+0.5f, -0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(0,0, BlockFace.left), Vector3.Zero, 0,3f),
                    },
                    new int[] { 0, 1, 2, 0, 2, 3 },
                    Matrix4.CreateRotationX(MathHelper.DegreesToRadians(90f)) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(-90f)))
                );
            
            // face foward
            meshes.Add(
                (int)FaceBufferId.back,
                new BlockMeshPart(
                    new VertexPCTOTI[]
                    {
                        new VertexPCTOTI(new Vector3(+0.5f, +0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(0,1, BlockFace.back), Vector3.Zero, 0,0f),
                        new VertexPCTOTI(new Vector3(-0.5f, +0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(1,1, BlockFace.back), Vector3.Zero, 0,1f),
                        new VertexPCTOTI(new Vector3(-0.5f, -0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(1,0, BlockFace.back), Vector3.Zero, 0,2f),
                        new VertexPCTOTI(new Vector3(+0.5f, -0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(0,0, BlockFace.back), Vector3.Zero, 0,3f),
                    },
                    new int[] { 0, 1, 2, 0, 2, 3 },
                    Matrix4.CreateRotationX(MathHelper.DegreesToRadians(90f)))
                );
            // face back
            meshes.Add(
                (int)FaceBufferId.front,
                new BlockMeshPart(
                    new VertexPCTOTI[]
                    {
                        new VertexPCTOTI(new Vector3(+0.5f, +0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(0,1, BlockFace.front), Vector3.Zero, 0,0f),
                        new VertexPCTOTI(new Vector3(-0.5f, +0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(1,1, BlockFace.front), Vector3.Zero, 0,1f),
                        new VertexPCTOTI(new Vector3(-0.5f, -0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(1,0, BlockFace.front), Vector3.Zero, 0,2f),
                        new VertexPCTOTI(new Vector3(+0.5f, -0.5f, +0.5f),new Color4(1f,1f,1f,1f), Game.blockTextures.GetTextureCord(0,0, BlockFace.front), Vector3.Zero, 0,3f),
                    },
                    new int[] { 0, 1, 2, 0, 2, 3 },
                    Matrix4.CreateRotationX(MathHelper.DegreesToRadians(-90f)) * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(180)))
                );
        }

    }
}
