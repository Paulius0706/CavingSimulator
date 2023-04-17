using BepuPhysics;
using CavingSimulator2;
using CavingSimulator2.Debugger;
using CavingSimulator2.GameLogic.Components;
using CavingSimulator2.GameLogic.Components.Physics;
using CavingSimulator2.GameLogic.Objects.SpaceShipParts;
using CavingSimulator2.Helpers;
using CavingSimulator2.Render;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator.GameLogic.Components
{
    public class Player : IDisposable
    {
        public readonly Transform transform;
        public readonly RigBody rigBody;
        public readonly PlayerCabin playerCabin;
        public float viewSensitivity = 0.01f;

        public bool lockMouse = false;

        public Player(Transform transform, RigBody rigBody, PlayerCabin playerCabin)
        {
            this.transform = transform;
            this.rigBody = rigBody;
            this.playerCabin = playerCabin;
        }
        public void Update() 
        {

            CameraMovement();
            MeniuControl();
            //Debug.WriteLine(Camera.lenght + "");
        }
        public void CameraMovement()
        {

            Camera.relative_position = transform.Position;
            
            if (Game.UI.Use == "meniu") return;

            if (Inputs.ShiftScroolUp) { Camera.lenght += Game.deltaTime * 5f; }
            if (Inputs.ShiftScroolDown) { Camera.lenght -= Game.deltaTime * 5f; }
            
            KeyboardState input = Game.input;
            if (input.IsKeyPressed(Keys.Escape)) { lockMouse = !lockMouse; Game.cursorState = Game.cursorState == CursorState.Grabbed ? CursorState.Normal : CursorState.Grabbed; }
            
            if (lockMouse)
            {
                Vector2 delta = Game.mouse.Delta;
                Camera.SetDeltaYawPitch(delta.X * viewSensitivity, delta.Y * viewSensitivity);
            }

        }
        public void MeniuControl()
        {
            if (Game.UI.Use == "meniu") return;
            if (!Inputs.Pause) return;
            Game.UI.UseView("meniu");
            Game.cursorState = CursorState.Normal;
        }

        public void Dispose()
        {
            
        }
    }
}
