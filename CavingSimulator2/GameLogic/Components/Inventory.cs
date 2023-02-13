using CavingSimulator2.Debugger;
using CavingSimulator2.GameLogic.Objects.SpaceShipParts;
using CavingSimulator2.GameLogic.UI.Views.Components;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.Components
{
    public class Inventory
    {
        public const int ITEM_LINE_LENGHT = 9;
        public PlayerCabin playerCabin;

        public int index = 0;
        public bool cursorStateHasChanged = false;
        public Inventory(PlayerCabin playerCabin)
        {
            this.playerCabin = playerCabin;
        }
        public void Update()
        {
            Scroll();
        }
        private void Scroll()
        {
            if (!playerCabin.player.isBuilderMode) return;
            Vector2 scroll = Game.mouse.ScrollDelta;
            if(scroll.Y == -1)
            {
                index = index > 0 ? index - 1 : ITEM_LINE_LENGHT - 1;
                cursorStateHasChanged = true;
            }
            if(scroll.Y == 1)
            {
                index = index < ITEM_LINE_LENGHT - 1 ? index + 1 : 0;
                cursorStateHasChanged = true;
            }
            if (cursorStateHasChanged)
            {
                ItemsLine itemsLine = Game.UI.GetView<ItemsLine>("placeHolder");
                if(itemsLine is not null) itemsLine.UpdateCursor(index);
            }
        }
        public void Render()
        {

        }
    }
}
