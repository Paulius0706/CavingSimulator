using CavingSimulator2.GameLogic.Objects.SpaceShipParts;
using CavingSimulator2.Render.Meshes;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.UI.Views.Components
{
    public class ItemsLine : View
    {
        private float gapLenght;
        private float frameSize;
        private float gapsLenght;
        private float lowerHeight;
        private Vector2 LowerPosition;
        public ItemsLine(string tag,Vector2 LowerPosition, Vector2 WidthHeight, int count, float frameSize)
        {
            base.tag = tag;
            this.frameSize = frameSize;
            this.LowerPosition = LowerPosition;
            gapsLenght = WidthHeight.X - count * frameSize;
            gapLenght = gapsLenght / (count - 1);
            lowerHeight = LowerPosition.Y + WidthHeight.Y / 2f - frameSize / 2f;
            for(int i = 0;i < count; i++)
            {
                views.Add("frame" + i, new ItemHolder("frame"+i,"imageDebugframe", new Vector2(this.LowerPosition.X + this.frameSize * i + gapLenght * i, lowerHeight), new Vector2(frameSize, frameSize)));
            }
            views.Add("cursor", new Cursor("cursor", new Vector2(this.LowerPosition.X + this.frameSize * 0 + gapLenght * 0 - frameSize * 0.1f, lowerHeight - frameSize * 0.1f), new Vector2(frameSize * 1.2f, frameSize * 1.2f)));
        }
        public void UpdateCursor(int index)
        {
            if(views.ContainsKey("cursor") && views["cursor"] is not null)
            {
                views["cursor"].Dispose();
                views.Remove("cursor");
                views.Add("cursor", 
                    new Cursor(
                        "cursor", 
                        new Vector2(
                            this.LowerPosition.X + this.frameSize * index + gapLenght * index - frameSize * 0.1f,
                            lowerHeight - frameSize * 0.1f
                            ), 
                        new Vector2(frameSize * 1.2f, frameSize * 1.2f)
                        )
                    );
            }
        }
        public void AddImage(int index, string imageName)
        {
            if (views.ContainsKey("frame" + index) && views["frame" + index] is not null)
            {
                ItemHolder itemHolder = (ItemHolder)views["frame" + index];
                if (itemHolder.item is null)
                {
                    itemHolder.imageName = imageName;
                    itemHolder.item = new UIMesh(Game.textures.GetIndex(imageName), itemHolder.upperPosition, itemHolder.lowerPosition, 1f);
                }
            }
        }
        public void RemoveImage(int index, string notDelete)
        {
            if (views.ContainsKey("frame" + index) && views["frame" + index] is not null )
            {
                ItemHolder itemHolder = (ItemHolder)views["frame" + index];
                if(itemHolder.item is not null && itemHolder.imageName != notDelete)
                {
                    itemHolder.item.Dispose();
                    itemHolder.item = null;
                }
            }
        }
        
    }
}
