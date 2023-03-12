using CavingSimulator2.Render.Meshes;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.UI.Views.Components
{
    public class ItemTextInfo : View
    {
        public const float Letter_Height = 40f;
        public const float Letter_Width = 40f;

        List<UITextMesh> lines = new List<UITextMesh>();
        public readonly Vector2 LowerUpperPosition;
        public ItemTextInfo(string tag, Vector2 LowerUpperPosition)
        {
            this.tag = tag;
            this.LowerUpperPosition = LowerUpperPosition;

            AddLine("Test1");
        }
        
        public void AddLine(string str)
        {

            GetCordsFromLowerPositionWidthHeight(LowerUpperPosition - Vector2.UnitY * Letter_Height * (lines.Count + 1), new Vector2(Letter_Width, Letter_Height), out Vector2 lPosition, out Vector2 uPosition);
            lines.Add(new UITextMesh("Text", lPosition, uPosition, 1f));
        }
        public void RemoveLine(int index)
        {
            lines[index].Dispose();
            lines.RemoveAt(index);
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
