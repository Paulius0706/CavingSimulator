using BepuPhysics;
using CavingSimulator2;
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
    public class Player
    {
        public readonly Transform transform;
        public readonly RigBody rigBody;
        public readonly PlayerCabin playerCabin;
        public float viewSensitivity = 0.01f;


        public bool isBuilderMode = false;

        public bool lockMouse = false;

        public Player(Transform transform, RigBody rigBody, PlayerCabin playerCabin)
        {
            this.transform = transform;
            this.rigBody = rigBody;
            this.playerCabin = playerCabin;
        }
        public void Update() 
        {
            BuilderModeInstancing();

            Movement();
            Camera.position = transform.Position - Camera.lookToPoint * 5f;
        }
        public void Movement()
        {
            KeyboardState input = Game.input;
            if (input.IsKeyPressed(Keys.Escape)) { lockMouse = !lockMouse; Game.cursorState = Game.cursorState == CursorState.Grabbed ? CursorState.Normal : CursorState.Grabbed; }
            
            if (lockMouse)
            {
                Vector2 delta = Game.mouse.Delta;
                Camera.SetDeltaYawPitch(delta.X * viewSensitivity, delta.Y * viewSensitivity);
            }
            // moving
            Vector3 velocityTarget =
                Inputs.MovementPlane.X * Vector3.Normalize(Vector3.Cross(Camera.lookToPoint, Camera.up)) +
                Inputs.MovementPlane.Y * Vector3.Normalize(Camera.lookToPoint - Inputs.MovementPlane.Y * Camera.lookToPoint.Z * Vector3.UnitZ);
            velocityTarget.Z = 0;
        }
        public void BuilderModeInstancing()
        {
            KeyboardState input = Game.input;
            if (input.IsKeyPressed(Keys.B)) 
            {
                isBuilderMode = !isBuilderMode;
            }
            if (isBuilderMode)
            {
                Game.UI.UseView("builder");
            }
            if(!isBuilderMode && Game.UI.Use == "builder")
            {
                Game.UI.UnUseView();
            }

        }
    }
}
