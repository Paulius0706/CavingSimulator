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
    public static class Drone
    {
        public static void Create(PlayerCabin playerCabin)
        {
            // Up Down
            playerCabin.AddPart(new Vector3i(0, 0, 1), new Gimbal(new Vector3(0f, 0f, +120f), Keys.Space));
            playerCabin.AddPart(new Vector3i(0, 0, -1), new Gimbal(new Vector3(0f, 0f, -30f), Keys.LeftShift));

            // rotate right left
            playerCabin.AddPart(new Vector3i(+1, -1, 0), new Thruster(new Quaternion(0f, 0f, MathHelper.DegreesToRadians(+90f)), 60f, Keys.D));
            playerCabin.AddPart(new Vector3i(-1, +1, 0), new Thruster(new Quaternion(0f, 0f, MathHelper.DegreesToRadians(-90f)), 60f, Keys.D));

            playerCabin.AddPart(new Vector3i(-1, -1, 0), new Thruster(new Quaternion(0f, 0f, MathHelper.DegreesToRadians(-90f)), 60f, Keys.A));
            playerCabin.AddPart(new Vector3i(+1, +1, 0), new Thruster(new Quaternion(0f, 0f, MathHelper.DegreesToRadians(+90f)), 60f, Keys.A));

            // foward back
            playerCabin.AddPart(new Vector3i(0, -1, 0), new Thruster(new Quaternion(0f, 0f, 0f), 30f, Keys.W));
            playerCabin.AddPart(new Vector3i(0, +2, 0), new Thruster(new Quaternion(0f, 0f, MathHelper.DegreesToRadians(+180f)), 60f, Keys.S));
        }
    }
}
