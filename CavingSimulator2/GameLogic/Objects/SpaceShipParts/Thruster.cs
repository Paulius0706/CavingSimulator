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
        
        public Thruster(Transform transform, Vector3 localRotation, float force, Keys key = Keys.Unknown) : base()
        {
            ImageName = "thrusterImage";
            this.transform = transform;
            this.localRotation = localRotation;
            this.force = force;
            this.key = key;

            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "truster"));
        }
        public Thruster(Vector3 localRotation, float force, Keys key = Keys.Unknown) : base()
        {
            ImageName = "thrusterImage";
            this.transform = new Transform(Vector3.Zero);
            this.localRotation = localRotation;
            this.force = force; 
            this.key = key;

            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "truster"));
        }

        public override void Update()
        {
            Vector3 localPos = new Vector3(new Vector4(localPosition) * Matrix4.CreateFromQuaternion(new Quaternion(this.parentTransform.Rotation)));
            transform.Position = this.parentTransform.Position + localPos;
            var qRotation = (Quaternion.FromEulerAngles(this.parentTransform.Rotation) * Quaternion.FromEulerAngles(this.localRotation));
            transform.Rotation = qRotation.ToEulerAngles();

            if (key == Keys.Unknown || Game.input.IsKeyDown(key))
            {
                Vector3 forceDirection = new Vector3(new Vector4(0f,force,0f,0f) * Matrix4.CreateFromQuaternion(qRotation));
                parentRigbody.AddForce(localPos, forceDirection * Game.deltaTime);
            }


        }

        public override Part Create()
        {
            return new Thruster(localRotation, force, key);
        }
    }
}
