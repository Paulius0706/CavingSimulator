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
        public struct ItemSlot
        {
            public Item item;
            public Part part;

            public ItemSlot(Item item,Part part)
            {
                this.item = item;
                this.part = part;
            }
        }
        public enum Item
        {
            none,
            frame,
            gimbal,
            gyro,
            thruster
        }
        public const int ITEM_LINE_LENGHT = 9;
        public PlayerCabin playerCabin;

        public int index = 0;
        public bool cursorStateHasChanged = false;
        public bool updated = true;

        public Keys[] keys = new Keys[]
        {
            Keys.Q,
            Keys.W,
            Keys.E,
            Keys.R,
            Keys.T,
            Keys.Y,
            Keys.U,
            Keys.I,
            Keys.O,
            Keys.P,
            Keys.A,
            Keys.S,
            Keys.D,
            Keys.F,
            Keys.G,
            Keys.H,
            Keys.J,
            Keys.K,
            //Keys.L,
            Keys.Z,
            Keys.X,
            Keys.C,
            Keys.V,
            //Keys.B,
            Keys.N,
            Keys.M,
            Keys.Space,
            Keys.LeftShift,
            Keys.RightShift,
        };

        public ItemSlot[] itemSlots = new ItemSlot[9] 
        {
            new ItemSlot(Item.frame,new  Frame()),
            new ItemSlot(Item.gimbal,new  Gimbal(new Vector3(0f, 0f,+120f), Keys.Space)),
            new ItemSlot(Item.gyro,new  GyroScope(Vector3.Zero,150f,Vector3.Zero,Keys.G)),
            new ItemSlot(Item.thruster,new  Thruster(Vector3.Zero,60f,Keys.Unknown)),
            new ItemSlot(Item.none,null),
            new ItemSlot(Item.none,null),
            new ItemSlot(Item.none,null),
            new ItemSlot(Item.none,null),
            new ItemSlot(Item.none,null)
        };
        

        public Inventory(PlayerCabin playerCabin)
        {
            this.playerCabin = playerCabin;
            
        }
        public void Update()
        {
            if (updated && Game.UI.Current is not null )
            {
                ItemsLine itemsLine = Game.UI.GetView<ItemsLine>("placeHolder");
                if (itemsLine is not null)
                {
                    for(int i = 0; i < ITEM_LINE_LENGHT; i++)
                    {
                        if (itemSlots[i].item == Item.none)
                        {
                            itemsLine.RemoveImage(i, "");
                            continue;
                        }
                        itemsLine.RemoveImage(i, itemSlots[i].item.ToString() + "Image");
                        itemsLine.AddImage(i, itemSlots[i].item.ToString() + "Image");
                    }
                }
                updated = false;
            }
            Scroll();
            PlaceItem();
            KeyBind();
        }
        private void PlaceItem()
        {
            if (!playerCabin.player.isBuilderMode) return;
            if (Game.mouse.IsButtonPressed(MouseButton.Left) && !playerCabin.parts.ContainsKey(playerCabin.selector.localPosition))
            {
                Part part = itemSlots[index].part.Create();
                Vector3 rotation = new Vector3(playerCabin.selector.lookRotation.X, playerCabin.selector.lookRotation.Y, playerCabin.selector.lookRotation.Z);
                part.localRotation = rotation;
                playerCabin.AddPart(playerCabin.selector.localPosition, part);
            }
            if(Game.mouse.IsButtonPressed(MouseButton.Right) && playerCabin.parts.ContainsKey(playerCabin.selector.localPosition))
            {
                playerCabin.RemovePart(playerCabin.selector.localPosition);
            }

        }
        private void KeyBind()
        {
            if (playerCabin.selector is null) return;
            if (!playerCabin.selector.KeyBind) return;
            if (!playerCabin.parts.ContainsKey(playerCabin.selector.localPosition)) return;

            foreach(Keys key in keys)
            {
                if (Game.input.IsKeyPressed(key)) 
                { 
                    playerCabin.parts[playerCabin.selector.localPosition].key = key;
                    Debug.WriteLine(playerCabin.parts[playerCabin.selector.localPosition].GetType().Name + " <= " + key);
                }
            }
            
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
