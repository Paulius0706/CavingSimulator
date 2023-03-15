using CavingSimulator2.Render.Meshes;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.UI.Views.Components
{
    public class HintInfo : View
    {
        public float BackGroundOpacity = 0.3f;
        public float TextOpacity = 0.9f;

        public UIMesh textBackGround;
        public TextLines text;
        public Color4 textBackGroundColor;
        public Color4 textTextColor;


        public readonly Vector2 UpperPosition;

        public const float Letter_Width = 15f;
        public const float Letter_Height = 20f;
        public const float paddling = 10f;
        public const float gap = 20f;
        public bool empty = false;
        public List<string> info = new List<string>();


        public HintInfo(string tag, Vector2 UpperPosition)
        {
            this.tag = tag;
            var lbc = Color4.DarkGray;
            textBackGroundColor = new Color4(lbc.R, lbc.G, lbc.B, BackGroundOpacity);

            var ltc = Color4.GhostWhite;
            textTextColor = new Color4(ltc.R, ltc.G, ltc.B, TextOpacity);
            
            this.tag = tag;
            this.UpperPosition = UpperPosition;


            //text = new TextLines("ItemInfo",
            //    new Vector2(UpperPosition.X - paddling - paddling - info.Select(o => o.Length).Max() * Letter_Width, UpperPosition.Y - paddling),
            //    new Vector2(Letter_Width, Letter_Height));

            //GetCordsFromLowerPositionWidthHeight(
            //    new Vector2(UpperPosition.X, UpperPosition.Y - text.PixelHeight - paddling - paddling),
            //    new Vector2(paddling + text.PixelWidth + paddling, paddling + text.PixelHeight + paddling),
            //    out Vector2 lLabelPosition,
            //    out Vector2 uLabelPosition);
            //textBackGround = new UIMesh(Game.textures.GetIndex("white"), uLabelPosition, lLabelPosition, Vector2.Zero, Vector2.One, textBackGroundColor, 1f);
        }
        public void Update(List<string> lines)
        {
            InternalDispose();
            info = lines;
            if (info.Count == 0) return;
            text = new TextLines("ItemInfo",
                new Vector2(UpperPosition.X - paddling - info.Select(o => o.Length).Max() * Letter_Width, UpperPosition.Y - paddling),
                new Vector2(Letter_Width, Letter_Height));

            foreach(string line in info)
            {
                text.AddLine(line,textTextColor);
            }

            GetCordsFromLowerPositionWidthHeight(
                new Vector2(UpperPosition.X - (paddling + text.PixelWidth + paddling), UpperPosition.Y - (paddling + text.PixelHeight + paddling)),
                new Vector2(paddling + text.PixelWidth + paddling, paddling + text.PixelHeight + paddling),
                out Vector2 lLabelPosition,
                out Vector2 uLabelPosition);
            textBackGround = new UIMesh(Game.textures.GetIndex("white"), uLabelPosition, lLabelPosition, Vector2.Zero, Vector2.One, textBackGroundColor, 1f);
        }
        public void Update(int index, string line)
        {
            if (info.Count == 0) return;
            if (info.Count <= index) return;
            InternalDispose();
            info[index] = line;
            text = new TextLines("ItemInfo",
                new Vector2(UpperPosition.X - paddling - info.Select(o => o.Length).Max() * Letter_Width, UpperPosition.Y - paddling),
                new Vector2(Letter_Width, Letter_Height));

            foreach (string str in info)
            {
                text.AddLine(str, textTextColor);
            }

            GetCordsFromLowerPositionWidthHeight(
                new Vector2(UpperPosition.X - (paddling + text.PixelWidth + paddling), UpperPosition.Y - (paddling + text.PixelHeight + paddling)),
                new Vector2(paddling + text.PixelWidth + paddling, paddling + text.PixelHeight + paddling),
                out Vector2 lLabelPosition,
                out Vector2 uLabelPosition);
            textBackGround = new UIMesh(Game.textures.GetIndex("white"), uLabelPosition, lLabelPosition, Vector2.Zero, Vector2.One, textBackGroundColor, 1f);
        }
        public override void Render()
        {
            if (info.Count == 0) return;
            if (textBackGround is not null) textBackGround.Render();
            if (text is not null) text.Render();
        }
        protected override void InternalDispose()
        {
            if (textBackGround is not null) textBackGround.Dispose();
            if (text is not null) text.Dispose();
        }
    }
}
