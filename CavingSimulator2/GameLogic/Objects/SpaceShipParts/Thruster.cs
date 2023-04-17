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
    public class Thruster : Part
    {
        public float force;
        Transform thrusterFireTransform;
        Mesh fire;
        public float angle = 0f;

        bool usingPart = false;

        public Thruster(Transform transform, Quaternion localRotation, float force, Keys key = Keys.Unknown) : base()
        {
            ImageName = "thrusterImage";
            this.transform = transform;
            this.localRotation = localRotation;
            this.thrusterFireTransform = new Transform(transform.Position);
            this.force = force;
            this.key = key;

            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "truster"));
            this.fire = new Mesh(this.thrusterFireTransform, "trusterFire");
        }
        public Thruster(Quaternion localRotation, float force, Keys key = Keys.Unknown) : base()
        {
            ImageName = "thrusterImage";
            this.transform = new Transform(Vector3.Zero);
            this.localRotation = localRotation;
            this.thrusterFireTransform = new Transform(transform.Position);
            this.force = force; 
            this.key = key;

            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "truster"));
            this.fire = new Mesh(this.thrusterFireTransform, "trusterFire");
        }

        public override void Update()
        {
            usingPart = false;
            Vector3 localPos = new Vector3(new Vector4(localPosition) * Matrix4.CreateFromQuaternion(parentTransform.Rotation));
            transform.Position = this.parentTransform.Position + localPos;
            var qRotation = (this.parentTransform.Rotation * this.localRotation);
            transform.Rotation = qRotation;
            thrusterFireTransform.Position = transform.Position;
            thrusterFireTransform.Rotation = transform.Rotation;

            if (Game.UI.Use == "meniu") return;

            if (key == Keys.Unknown || Game.input.IsKeyDown(key))
            {
                usingPart = true;
                angle += MathHelper.DegreesToRadians(90f) * Game.deltaTime;
                thrusterFireTransform.Rotation *= new Quaternion(0, angle, 0);

                Vector3 forceDirection = new Vector3(new Vector4(0f,force,0f,0f) * Matrix4.CreateFromQuaternion(qRotation));
                parentRigbody.AddForce(localPos, forceDirection * Game.deltaTime);
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
            return new Thruster(localRotation, force, key);
        }
    }
}
