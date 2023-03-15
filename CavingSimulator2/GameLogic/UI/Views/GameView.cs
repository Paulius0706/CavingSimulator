using CavingSimulator2.GameLogic.UI.Views.Components;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.UI.Views
{
    public class GameView : View
    {
        public GameView()
        {
            HintInfo hintInfo = new HintInfo("HintInfo", new Vector2(Game.ViewPortSize.X - 20f, Game.ViewPortSize.Y - 20f));
            hintInfo.Update(new List<string>()
            {
                "Build - B",
                "Pause - P",

            });
            views.Add("HintLine", hintInfo);

        }
    }
}
