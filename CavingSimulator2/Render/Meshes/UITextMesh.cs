using BepuUtilities;
using CavingSimulator.Render;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.Render.Meshes
{
    public class UITextMesh
    {
        public const string Character_Set_Name = "font";
        public const float Pixels_Width = 271;
        public const float Pixels_Height = 209;
        public const int Letters_Width_Count = 17;
        public const int Letters_Height_Count = 17;
        public const int Real_Letters_Width_Count = 16;
        public const int Real_Letters_Height_Count = 16;
        public const int Real_Letters_Width_Start = 1;
        public const int Real_Letters_Height_Start = 1;
        public const int ACSII_Start = 0;

        private bool disposed;

        public List<UIMesh> letters = new List<UIMesh>();

        public UITextMesh(string str, Vector2 firstLetterUpperPosition, Vector2 firstLetterLowerPosition, float depth)
        {
            for(int i = 0; i< str.Length;i++)
            {
                char character = str[i];
                GetTexturePos(character, out Vector2 textureLowerPosition, out Vector2 textureUpperPosition);
                Vector2 lowerPosition = firstLetterLowerPosition + Vector2.UnitX * Math.Abs(firstLetterUpperPosition.X - firstLetterLowerPosition.X) * i;
                Vector2 upperPosition = firstLetterUpperPosition + Vector2.UnitX * Math.Abs(firstLetterUpperPosition.X - firstLetterLowerPosition.X) * i;
                UIMesh letter = new UIMesh(Game.textures.GetIndex(Character_Set_Name), lowerPosition, upperPosition, textureLowerPosition, textureUpperPosition, 1f);
                letters.Add(letter);
            }
        }
        public static void GetTexturePos(char character, out Vector2 lowerPosition, out Vector2 upperPostion)
        {
            lowerPosition = new Vector2();
            upperPostion = new Vector2();
            int pos = character;
            //int height = pos / Real_Letters_Width_Count;
            int height = (int)Math.Ceiling((float)pos / (float)Real_Letters_Width_Count);
            int width = pos % Real_Letters_Width_Count;
            height += Real_Letters_Height_Start;
            width += Real_Letters_Width_Start;

            float letterWidth = 1f / Letters_Width_Count;
            float LetterHeight = 1f / Letters_Height_Count;

            lowerPosition.X = letterWidth * width;
            lowerPosition.Y = 1f - (LetterHeight * height - LetterHeight * 0.15f);
            upperPostion.X = lowerPosition.X + letterWidth;
            upperPostion.Y = lowerPosition.Y + LetterHeight;
        }

        public void Render()
        {
            foreach(UIMesh letter in letters)
            {
                letter.Render();
            }
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            foreach(UIMesh letter in letters)
            {
                letter.Dispose();
            }
            letters = new List<UIMesh>();
        }
    }
}
