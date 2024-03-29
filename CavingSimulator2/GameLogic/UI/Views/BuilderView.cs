﻿using CavingSimulator2.GameLogic.UI.Views.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using CavingSimulator2.GameLogic.Components;

namespace CavingSimulator2.GameLogic.UI.Views
{
    public class BuilderView : View
    {

        public BuilderView()
        {
            //views.Add("frame",new ItemHolder("imageDebugframe", new Vector2(100,100), new Vector2(100,100)));
            int count = Inventory.ITEM_LINE_LENGHT;
            float size = 80f;
            Vector2 center = new Vector2(Game.ViewPortSize.X / 2f, 80f);
            Vector2 lineSize = new Vector2(size * ((float)count+1f), size);
            Vector2 lowerCorner = center - lineSize / 2f;
            views.Add("Line", new ItemsLine("placeHolder", lowerCorner, lineSize,count,size));
            views.Add("ItemInfo", new ItemInfo("ItemInfo", new Vector2(20f, Game.ViewPortSize.Y - 20f)));
            HintInfo hintInfo = new HintInfo("HintInfo", new Vector2(Game.ViewPortSize.X - 20f, Game.ViewPortSize.Y - 20f));
            hintInfo.Update(new List<string>()
            {
                "Exist Build - B   ",
                "   Bind Key - L   ",
                "Set Key Neg - ALT ",
                "     Rotate - Q,E ",
                "       Move - WASD",
            });
            
            views.Add("HintLine", hintInfo);
            //views.Add("Info", new TextLines("ItemInfo",new Vector2(50f,Game.ViewPortSize.Y -50f),new Vector2(30f,30f)));
        }
    }
}
