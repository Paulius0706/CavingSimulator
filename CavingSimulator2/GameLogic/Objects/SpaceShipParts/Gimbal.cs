using CavingSimulator.GameLogic.Components;
using CavingSimulator2.Render.Meshes;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.Objects.SpaceShipParts
{
    public class Gimbal : Part
    {
        Transform thrusterTrasform;
        Transform thrusterFireTransform;
        Mesh fire;
        bool usingPart = false;

        public Vector3 forceDirection;
        //public Keys key;
        public Gimbal(Transform transform, Vector3 forceDirection, Keys key = Keys.Unknown) : base()
        {
            ImageName = "gimbalImage";
            this.transform = transform;
            this.thrusterTrasform = new Transform(transform.Position);
            this.thrusterTrasform.Rotation = new Quaternion(MathHelper.DegreesToRadians(90f), 0, 0);
            this.thrusterTrasform.Scale = Vector3.One * 0.7f;

            this.thrusterFireTransform = new Transform(transform.Position);
            this.thrusterFireTransform.Rotation = new Quaternion(MathHelper.DegreesToRadians(90f), 0, 0);
            this.thrusterFireTransform.Scale = Vector3.One * 0.7f;

            this.forceDirection = forceDirection;
            this.key = key;

            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "gimbal"));
            this.renderer.AddMesh(new Mesh(this.thrusterTrasform, "truster"));
            this.fire = new Mesh(this.thrusterFireTransform, "trusterFire");
        }
        public Gimbal(Vector3 forceDirection, Keys key = Keys.Unknown) : base()
        {
            ImageName = "gimbalImage";

            this.transform = new Transform(Vector3.Zero);
            this.thrusterTrasform = new Transform(transform.Position);
            this.thrusterTrasform.Rotation = new Quaternion(MathHelper.DegreesToRadians(90f),0, 0);
            this.thrusterTrasform.Scale = Vector3.One * 0.7f;

            this.thrusterFireTransform = new Transform(transform.Position);
            this.thrusterFireTransform.Rotation = new Quaternion(MathHelper.DegreesToRadians(90f), 0, 0);
            this.thrusterFireTransform.Scale = Vector3.One * 0.7f;

            this.forceDirection = forceDirection;
            this.key = key;

            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "gimbal"));
            this.renderer.AddMesh(new Mesh(this.thrusterTrasform, "truster"));
            this.fire = new Mesh(this.thrusterFireTransform, "trusterFire");
        }

        public override void Update()
        {
            usingPart = false;
            Vector3 localPos = new Vector3(new Vector4(localPosition) * Matrix4.CreateFromQuaternion(this.parentTransform.Rotation));
            transform.Position = this.parentTransform.Position + localPos;
            transform.Rotation = this.parentTransform.Rotation;
            thrusterTrasform.Position = this.transform.Position; 
            thrusterFireTransform.Position = this.transform.Position;

            if (Game.UI.Use == "meniu") return;

            if (key == Keys.Unknown || Game.input.IsKeyDown(key))
            {
                usingPart = true;
                thrusterFireTransform.Rotation *= new Quaternion(0, MathHelper.DegreesToRadians(90f) * Game.deltaTime, 0);
                parentRigbody.AddForce(localPos, forceDirection * Game.deltaTime);
                //parentRigbody.AddForce(transform.Position - parentTransform.Position, forceDirection * Game.deltaTime);
            }

        }
        public override void Render()
        {
            base.Render();
            if (usingPart && fire is not null) 
                fire.Render();
        }
        public override Part Create()
        {
            return new Gimbal(forceDirection, key);
        }
    }
}
