using CavingSimulator2.Render.Meshes;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.UI.Views.Components
{
    public class Cursor : View
    {
        public UIMesh frame;
        public string imageName = "";
        public Cursor(string tag, Vector2 LowerPosition, Vector2 WidthHeight)
        {
            base.tag = tag;
            this.imageName = imageName;
            GetCordsFromLowerPositionWidthHeight(LowerPosition, WidthHeight, out Vector2 lPosition, out Vector2 uPosition);
            frame = new UIMesh(Game.textures.GetIndex("itemFrame"), uPosition, lPosition, 1f);
        }
        public override void Render()
        {
            if (frame is not null) frame.Render();
        }
        protected override void InternalDispose()
        {
            if(frame is not null) frame.Dispose();
        }
    }
}
