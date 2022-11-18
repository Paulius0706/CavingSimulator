using BepuPhysics;
using CavingSimulator2;
using CavingSimulator2.GameLogic.Components;
using CavingSimulator2.GameLogic.Components.Physics;
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
        public float acceleration = 25f;
        public float viewSensitivity = 0.01f;
        public float maxSpeed = 15f;

        public bool lockMouse = false;

        public Player(Transform transform, RigBody rigBody)
        {
            this.transform = transform;
            this.rigBody = rigBody;
        }
        public void Update() 
        {
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

            //Vector3 velocityTarget;
            Vector3 velocityTarget =
                Inputs.MovementPlane.X * Vector3.Normalize(Vector3.Cross(Camera.lookToPoint, Camera.up)) +
                Inputs.MovementPlane.Y * Vector3.Normalize(Camera.lookToPoint - Inputs.MovementPlane.Y * Camera.lookToPoint.Z * Vector3.UnitZ);
            velocityTarget.Z = 0;

            //if (velocityTarget != Vector3.Zero && rigBody != null)
            //{
            //    rigBody.LinearVelocity += (RigBody.GoTowordsDelta(rigBody.LinearVelocity, velocityTarget * maxSpeed, acceleration * Game.deltaTime));
            //}
            //if (Inputs.JumpCrouchAxis != 0 && rigBody != null) 
            //{
            //    rigBody.LinearVelocity += (RigBody.GoTowordsDelta(rigBody.LinearVelocity, Inputs.JumpCrouchAxis * Camera.up * maxSpeed, acceleration * Game.deltaTime));
            //}
        }
    }
}
