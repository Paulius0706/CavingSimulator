using CavingSimulator2.Render.Meshes;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.Render
{
    public enum BlockImage
    {
        grass = 2,
        sand = 1,
        stone = 0
    }
    public enum BlockFace
    {
        top = 0,
        right = 1,
        front = 2,
        left = 3,
        back = 4,
        bottom = 5
    }
    public class BlockTextures
    {
        public readonly int textureHandle;
        public readonly int textureWidth;
        public readonly int textureHeight;

        private bool disposed;

        public const int spriteSize = 16;
        public readonly int spriteWidth;
        public readonly int spriteHeight;

        

        public BlockTextures(string path)
        {
            textureHandle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureHandle);
            // stb_image loads from the top-left pixel, whereas OpenGL loads from the bottom-left, causing the texture to be flipped vertically.
            // This will correct that, making the texture display properly.
            StbImage.stbi_set_flip_vertically_on_load(1);

            // Load the image.
            ImageResult image = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);
            textureWidth = image.Width;
            textureHeight = image.Height;
            spriteHeight = textureHeight / spriteSize;
            spriteWidth = textureWidth / spriteSize;

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            //GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
        ~BlockTextures()
        {
            Dispose();
        }
        public Vector2 GetTextureCord(Vector2 textureCord, BlockFace blockFace, BlockImage blockImage = BlockImage.stone)
        {
            return new Vector2(
                (textureCord.X * (float)spriteSize + (float)blockFace * (float)spriteSize) / textureWidth, 
                (textureCord.Y * (float)spriteSize + (float)blockImage * (float)spriteSize) / textureHeight);
        }
        public Vector2 GetTextureCord(float x, float y, BlockFace blockFace, BlockImage blockImage = BlockImage.stone)
        {
            return GetTextureCord(new Vector2(x,y), blockFace, blockImage);
        }
        public static BlockFace MeshKeyToFace(int key)
        {
            /*top = 22,
            right = 14,
            foward = 16,
            left = 12,
            back = 10,
            bottom = 4
             */
            switch (key)
            {
                case (int)BlockMeshes.FaceBufferId.top: return BlockFace.top;
                case (int)BlockMeshes.FaceBufferId.bottom: return 0;
                case (int)BlockMeshes.FaceBufferId.right: return 0;
                case (int)BlockMeshes.FaceBufferId.left: return 0;
                case (int)BlockMeshes.FaceBufferId.back: return 0;
                case (int)BlockMeshes.FaceBufferId.front: return 0;
            }
            return 0;
        }




        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            GL.DeleteTexture(textureHandle);
            GC.SuppressFinalize(this);
        }

        public void UploadTexture(int index = 0)
        {
            GL.BindTexture(TextureTarget.Texture2D, textureHandle);
            Game.shaderPrograms.Current.SetUniform("Texture", index);
        }
    }
}
