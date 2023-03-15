using CavingSimulator2.Debugger;
using CavingSimulator2.Render.Meshes;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.UI.Views.Components
{
    public class ItemInfo : View
    {
        public enum Label
        {
            name = 0,
            position = 1,
            toggle = 2,
            key = 3
        }
        public float BackGroundOpacity = 0.3f;
        public float TextOpacity = 0.9f;

        public UIMesh labelBackGround;
        public UIMesh valueBackGround;
        public TextLines labels;
        public TextLines values;
        public Color4 labelsBackGroundColor;
        public Color4 valuesBackGroundColor;
        public Color4 labelsTextColor;
        public Color4 valuesTextColor;


        public readonly Vector2 LowerUpperPosition;

        public const float Letter_Width = 20f;
        public const float Letter_Height = 25f;
        public const float paddling = 10f;
        public const float gap = 20f;
        public bool empty = false;

        public ItemInfo(string tag, Vector2 LowerUpperPosition)
        {
            var lbc = Color4.DarkGray;
            var vbc = Color4.DarkGray;
            labelsBackGroundColor = new Color4(lbc.R, lbc.G, lbc.B, BackGroundOpacity);
            valuesBackGroundColor = new Color4(vbc.R, vbc.G, vbc.B, BackGroundOpacity);

            var ltc = Color4.GhostWhite;
            var vtc = Color4.GhostWhite;
            labelsTextColor = new Color4(ltc.R, ltc.G, ltc.B, TextOpacity);
            valuesTextColor = new Color4(vtc.R, vtc.G, vtc.B, TextOpacity);

            this.tag = tag;
            this.LowerUpperPosition = LowerUpperPosition;

            labels = new TextLines("ItemInfo", 
                new Vector2(LowerUpperPosition.X + paddling, LowerUpperPosition.Y - paddling), 
                new Vector2(Letter_Width, Letter_Height));
            labels.AddLine("Name", labelsTextColor);
            labels.AddLine("Position", labelsTextColor);
            labels.AddLine("Toggle", labelsTextColor);
            labels.AddLine("Trigger", labelsTextColor);


            GetCordsFromLowerPositionWidthHeight(
                new Vector2(LowerUpperPosition.X, LowerUpperPosition.Y - labels.PixelHeight - paddling - paddling), 
                new Vector2(paddling + labels.PixelWidth + paddling, paddling + labels.PixelHeight + paddling), 
                out Vector2 lLabelPosition, 
                out Vector2 uLabelPosition);
            labelBackGround = new UIMesh(Game.textures.GetIndex("white"),uLabelPosition,lLabelPosition,Vector2.Zero,Vector2.One,labelsBackGroundColor,1f);

            values = new TextLines("ItemInfo", 
                new Vector2(LowerUpperPosition.X + paddling + labels.PixelWidth + paddling + gap + paddling, LowerUpperPosition.Y - paddling), 
                new Vector2(Letter_Width, Letter_Height));
            values.AddLine("value", valuesTextColor);
            values.AddLine("valueasdc", valuesTextColor);
            values.AddLine("valueedrfv", valuesTextColor);
            values.AddLine("valueqerf", valuesTextColor);

            GetCordsFromLowerPositionWidthHeight(
                new Vector2(LowerUpperPosition.X + paddling + labels.PixelWidth + paddling + gap, LowerUpperPosition.Y - values.PixelHeight - paddling - paddling),
                new Vector2(paddling + values.PixelWidth + paddling, paddling + values.PixelHeight + paddling),
                out Vector2 lValuePosition,
                out Vector2 uValuePosition);
            valueBackGround = new UIMesh(Game.textures.GetIndex("white"), uValuePosition, lValuePosition, Vector2.Zero, Vector2.One,valuesBackGroundColor, 1f);

        }
        public void UpdateValue(Label label, string str)
        {
            int index = (int)label;
            values.UpdateLine(index, str, valuesTextColor);
            UpdateValueBackGround();
        }
        public void UpdateValueBackGround()
        {
            GetCordsFromLowerPositionWidthHeight(
                new Vector2(LowerUpperPosition.X + paddling + labels.PixelWidth + paddling + gap, LowerUpperPosition.Y - values.PixelHeight - paddling - paddling),
                new Vector2(paddling + values.PixelWidth + paddling, paddling + values.PixelHeight + paddling),
                out Vector2 lValuePosition,
                out Vector2 uValuePosition);
            valueBackGround.Dispose();
            valueBackGround = new UIMesh(Game.textures.GetIndex("white"), uValuePosition, lValuePosition, Vector2.Zero, Vector2.One, valuesBackGroundColor, 1f);
        }
        public override void Render()
        {
            if (empty) return; 
            if (labelBackGround != null) labelBackGround.Render();
            if (valueBackGround != null) valueBackGround.Render();
            if (values != null) values.Render();
            if (labels != null) labels.Render();
        }
        protected override void InternalDispose()
        {
            if (labelBackGround != null) labelBackGround.Dispose();
            if (valueBackGround != null) valueBackGround.Dispose();
            if (values != null) values.Dispose();
            if (labels != null) labels.Dispose();
        }


    }
}
