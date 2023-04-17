using CavingSimulator2.Debugger;
using CavingSimulator2.Helpers;
using CavingSimulator2.Render.Meshes;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.UI.Views.Components
{
    public class Button : View
    {
        public float BackGroundOpacity = 0.3f;
        public float TextOpacity = 0.9f;

        public UIMesh textBackGround;
        public TextLines text;
        public string textString;
        public Color4 textBackGroundColor;
        public Color4 textTextColor;


        public readonly Vector2 UpperPosition;
        public readonly Vector2 LowerPosition;

        public readonly float Letter_Width = 15f;
        public readonly float Letter_Height = 20f;
        public readonly float paddling = 10f;
        public bool empty = false;
        public string info = "";
        public Action action;

        public Button(string tag, Vector2 centerPosition, string text, Vector2 LetterWidthHeight, float paddling, Action action = null)
        {
            this.action = action;
            textString = text;
            Letter_Height = LetterWidthHeight.Y;
            Letter_Width = LetterWidthHeight.X;
            this.paddling = paddling;
            this.tag = tag;
            var lbc = Color4.DarkGray;
            textBackGroundColor = new Color4(lbc.R, lbc.G, lbc.B, BackGroundOpacity);

            var ltc = Color4.GhostWhite;
            textTextColor = new Color4(ltc.R, ltc.G, ltc.B, TextOpacity);

            this.UpperPosition = centerPosition + 
                Vector2.UnitX * text.Length * Letter_Width / 2f + 
                Vector2.UnitX * this.paddling +
                Vector2.UnitY * Letter_Height / 2f +
                Vector2.UnitY * this.paddling;
            this.LowerPosition = centerPosition -
                Vector2.UnitX * text.Length * Letter_Width / 2f -
                Vector2.UnitX * this.paddling -
                Vector2.UnitY * Letter_Height / 2f -
                Vector2.UnitY * this.paddling;

            info = text;
            this.text = new TextLines("ItemInfo",
                new Vector2(UpperPosition.X - paddling - info.Length * Letter_Width, UpperPosition.Y - paddling),
                new Vector2(Letter_Width, Letter_Height));
            this.text.AddLine(text, ltc);

            GetCordsFromLowerPositionWidthHeight(
                new Vector2(UpperPosition.X - (paddling + text.Length * Letter_Width + paddling), UpperPosition.Y - (paddling + Letter_Height + paddling)),
                new Vector2(paddling + text.Length * Letter_Width + paddling, paddling + Letter_Height + paddling),
                out Vector2 lLabelPosition,
                out Vector2 uLabelPosition);
            textBackGround = new UIMesh(Game.textures.GetIndex("white"), uLabelPosition, lLabelPosition, Vector2.Zero, Vector2.One, textBackGroundColor, 1f);
        }
        public override void UpdateInternal()
        {
            Vector2 mosePos = Game.mouse.Position;
            mosePos.Y = Game.ViewPortSize.Y - mosePos.Y;
            if (mosePos.Y < LowerPosition.Y || UpperPosition.Y < mosePos.Y) return;
            if (mosePos.X < LowerPosition.X || UpperPosition.X < mosePos.X) return;
            if(Inputs.LMouseClick)
            {
                if (action is not null) action.Invoke();
            }
        }

        public override void Render()
        {
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
