using CavingSimulator2.GameLogic.Components;
using CavingSimulator2.GameLogic.Objects.SpaceShipParts;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.Objects.SpaceShips
{
    public static class Plane
    {
        public static void Create(PlayerCabin playerCabin)
        {
            // Roll
            playerCabin.inventory.LoadPlaceItem(Inventory.Item.servo, new Vector3i(0, 1, 0), Selector.LookAxisToLooRotation(Vector3.UnitY),Keys.A,Keys.D);
            
            // Thrust
            playerCabin.inventory.LoadPlaceItem(Inventory.Item.thruster, new Vector3i(0, -1, 0), Selector.LookAxisToLooRotation(Vector3.UnitY), Keys.Space);
            playerCabin.inventory.LoadPlaceItem(Inventory.Item.thruster, new Vector3i(0,  2, 0), Selector.LookAxisToLooRotation(Vector3.UnitY), Keys.Space);

            // Pitch
            playerCabin.inventory.LoadPlaceItem(Inventory.Item.servo, new Vector3i( 1, 0, 0), Selector.LookAxisToLooRotation(-Vector3.UnitX), Keys.S, Keys.W);
            playerCabin.inventory.LoadPlaceItem(Inventory.Item.servo, new Vector3i(-1, 0, 0), Selector.LookAxisToLooRotation(-Vector3.UnitX), Keys.S, Keys.W);

            // fin wings
            playerCabin.inventory.LoadPlaceItem(Inventory.Item.wing, new Vector3i(0, -1, 1), Selector.LookAxisToLooRotation(Vector3.UnitY));
            playerCabin.inventory.LoadPlaceItem(Inventory.Item.wing, new Vector3i(0, -1, 2), Selector.LookAxisToLooRotation(Vector3.UnitY));
            playerCabin.inventory.LoadPlaceItem(Inventory.Item.wing, new Vector3i(0,  0, 1), Selector.LookAxisToLooRotation(Vector3.UnitY));
            Selector.LoadRotate(playerCabin, new Vector3i(0, -1, 1), Vector3.UnitY, Selector.RightLeft.left);
            Selector.LoadRotate(playerCabin, new Vector3i(0, -1, 2), Vector3.UnitY, Selector.RightLeft.left);
            Selector.LoadRotate(playerCabin, new Vector3i(0,  0, 1), Vector3.UnitY, Selector.RightLeft.left);

            // back wings
            playerCabin.inventory.LoadPlaceItem(Inventory.Item.wing, new Vector3i( 1, -1, 0), Selector.LookAxisToLooRotation(Vector3.UnitY));
            playerCabin.inventory.LoadPlaceItem(Inventory.Item.wing, new Vector3i(-1, -1, 0), Selector.LookAxisToLooRotation(Vector3.UnitY));
            playerCabin.inventory.LoadPlaceItem(Inventory.Item.wing, new Vector3i( 2, -1, 0), Selector.LookAxisToLooRotation(Vector3.UnitY));
            playerCabin.inventory.LoadPlaceItem(Inventory.Item.wing, new Vector3i(-2, -1, 0), Selector.LookAxisToLooRotation(Vector3.UnitY));
            playerCabin.inventory.LoadPlaceItem(Inventory.Item.wing, new Vector3i( 3, -1, 0), Selector.LookAxisToLooRotation(Vector3.UnitY));
            playerCabin.inventory.LoadPlaceItem(Inventory.Item.wing, new Vector3i(-3, -1, 0), Selector.LookAxisToLooRotation(Vector3.UnitY));

            // midle wings
            playerCabin.inventory.LoadPlaceItem(Inventory.Item.wing, new Vector3i( 2, 0, 0), Selector.LookAxisToLooRotation(Vector3.UnitY));
            playerCabin.inventory.LoadPlaceItem(Inventory.Item.wing, new Vector3i(-2, 0, 0), Selector.LookAxisToLooRotation(Vector3.UnitY));
            playerCabin.inventory.LoadPlaceItem(Inventory.Item.wing, new Vector3i( 3, 0, 0), Selector.LookAxisToLooRotation(Vector3.UnitY));
            playerCabin.inventory.LoadPlaceItem(Inventory.Item.wing, new Vector3i(-3, 0, 0), Selector.LookAxisToLooRotation(Vector3.UnitY));

            playerCabin.inventory.LoadPlaceItem(Inventory.Item.gimbal, new Vector3i( 1, 1, 0), Selector.LookAxisToLooRotation(Vector3.UnitY), Keys.LeftShift);
            playerCabin.inventory.LoadPlaceItem(Inventory.Item.gimbal, new Vector3i(-1, 1, 0), Selector.LookAxisToLooRotation(Vector3.UnitY), Keys.LeftShift);
            playerCabin.inventory.LoadPlaceItem(Inventory.Item.wing, new Vector3i( 2, 1, 0), Selector.LookAxisToLooRotation(Vector3.UnitY));
            playerCabin.inventory.LoadPlaceItem(Inventory.Item.wing, new Vector3i(-2, 1, 0), Selector.LookAxisToLooRotation(Vector3.UnitY));

            playerCabin.inventory.LoadPlaceItem(Inventory.Item.wing, new Vector3i( 1, 2, 0), Selector.LookAxisToLooRotation(Vector3.UnitY));
            playerCabin.inventory.LoadPlaceItem(Inventory.Item.wing, new Vector3i(-1, 2, 0), Selector.LookAxisToLooRotation(Vector3.UnitY));
            playerCabin.inventory.LoadPlaceItem(Inventory.Item.wing, new Vector3i( 2, 2, 0), Selector.LookAxisToLooRotation(Vector3.UnitY));
            playerCabin.inventory.LoadPlaceItem(Inventory.Item.wing, new Vector3i(-2, 2, 0), Selector.LookAxisToLooRotation(Vector3.UnitY));

            //playerCabin.inventory.LoadPlaceItem(Inventory.Item.wing, new Vector3i(0, 3, 0), Selector.LookAxisToLooRotation(Vector3.UnitY));

        }
    }
}
