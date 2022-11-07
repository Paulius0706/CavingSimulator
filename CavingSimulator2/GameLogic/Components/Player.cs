using CavingSimulator2;
using CavingSimulator2.GameLogic.Components;
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
    public class Player : Component
    {
        public readonly Transform transform;
        public readonly RigBody rigBody;
        public float acceleration = 25f;
        public float viewSensitivity = 0.01f;
        public float maxSpeed = 25f;

        public bool lockMouse = false;

        public Player(Transform transform, RigBody rigBody)
        {
            this.transform = transform;
            this.rigBody = rigBody;
        }
        public void Update() 
        {
            Movement();
            Camera.position = transform.GlobalPosition - Camera.lookToPoint * 5f;
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
                Inputs.MovementPlane.Y * (Camera.lookToPoint - Inputs.MovementPlane.Y * Camera.lookToPoint * Vector3.UnitZ).Normalized();
            if (velocityTarget != Vector3.Zero)
            {
                rigBody.velocity += RigBody.GoTowordsDelta(rigBody.velocity, velocityTarget * maxSpeed, acceleration * Game.deltaTime);
            }
            if (Inputs.JumpCrouchAxis != 0) { rigBody.velocity += Inputs.JumpCrouchAxis * Camera.up * acceleration * Game.deltaTime; }
        }
    }
}
