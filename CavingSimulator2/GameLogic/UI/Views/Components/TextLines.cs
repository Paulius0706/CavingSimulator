using CavingSimulator2.Render.Meshes;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.UI.Views.Components
{
    public class TextLines : View
    {
        public readonly float LetterHeight = 30f;
        public readonly float LetterWidth = 30f;
        public readonly float LineGap = 5f;

        List<UITextMesh> lines = new List<UITextMesh>();
        public readonly Vector2 LowerUpperPosition;
        public TextLines(string tag, Vector2 LowerUpperPosition, Vector2 LetterWidthHeight)
        {
            LetterWidth = LetterWidthHeight.X;
            LetterHeight = LetterWidthHeight.Y;
            this.tag = tag;
            this.LowerUpperPosition = LowerUpperPosition;

            //AddLine("A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z".Replace(", ", ""));
            //AddLine(string.Concat("A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z".Replace(", ", "").ToCharArray().Select(o => Char.ToLower(o))));
            //AddLine("0123456789");
            //AddLine("?.,+=-' '_/@#$%^&*");

        }
        public int MaxLineLetterWidth { get { return lines.Select(o => o.letters.Count).Max(); } }
        public float PixelWidth { get { return MaxLineLetterWidth * LetterWidth; } }
        public int LinesCount { get { return lines.Count; } }
        public float PixelHeight { get { return LinesCount * LetterHeight + (LineGap * Math.Max(0,LinesCount -1)); } }

        public void AddLine(string str, Color4 color)
        {
            GetCordsFromLowerPositionWidthHeight(LowerUpperPosition - Vector2.UnitY * LetterHeight * (lines.Count + 1) - Vector2.UnitY * LineGap * (lines.Count), new Vector2(LetterWidth, LetterHeight), out Vector2 lPosition, out Vector2 uPosition);
            lines.Add(new UITextMesh(str, lPosition, uPosition, color, 1f));
        }
        public void UpdateLine(int line, string str, Color4 color)
        {
            RemoveLine(line);
            GetCordsFromLowerPositionWidthHeight(LowerUpperPosition - Vector2.UnitY * LetterHeight * (line + 1) - Vector2.UnitY * LineGap * (line), new Vector2(LetterWidth, LetterHeight), out Vector2 lPosition, out Vector2 uPosition);
            lines.Insert(line,new UITextMesh(str, lPosition, uPosition,color, 1f));
        }
        public void RemoveLine(int index)
        {
            lines[index].Dispose();
            lines.RemoveAt(index);
        }
        public void RemoveLines()
        {
            foreach (UITextMesh line in lines)
            {
                line.Dispose();
            }
            lines = new List<UITextMesh>();
        }
        public override void Render()
        {
            foreach (UITextMesh line in lines)
            {
                line.Render();
            }
        }
        protected override void InternalDispose()
        {
            foreach(UITextMesh line in lines)
            {
                line.Dispose();
            }
            lines = new List<UITextMesh>();
        }

    }
}
