using CavingSimulator2.Render.Meshes;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.UI.Views.Components
{
    public class ItemHolder : View
    {
        public UIMesh frame;
        public UIMesh backGround;
        public UIMesh item;
        public string imageName = "";
        public ItemHolder(string tag,string imageName, Vector2 LowerPosition, Vector2 WidthHeight)
        {
            base.tag = tag;
            this.imageName = imageName;
            GetCordsFromLowerPositionWidthHeight(LowerPosition, WidthHeight, out Vector2 lPosition, out Vector2 uPosition);
            frame = new UIMesh(Game.textures.GetIndex("itemFrame"), uPosition, lPosition, 1f);
            //uPosition.Y += 0.1f;
            //lPosition.Y += 0.1f;
            backGround = new UIMesh(Game.textures.GetIndex("itemBackGround"), uPosition, lPosition, 1f);
        }
        public override void Render()
        {
            if (backGround is not null) backGround.Render();
            if (frame is not null) frame.Render();
            if (item is not null) item.Render();
        }
        protected override void InternalDispose()
        {
            frame.Dispose();
        }

    }
}
