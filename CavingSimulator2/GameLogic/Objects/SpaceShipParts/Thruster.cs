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
        public Keys key;
        public Thruster(Transform transform, Vector3 localRotation, float force, Keys key = Keys.Unknown) : base()
        {
            this.transform = transform;
            this.localRotation = localRotation;
            this.force = force;
            this.key = key;

            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "truster"));
        }
        public Thruster(Vector3 localRotation, float force, Keys key = Keys.Unknown) : base()
        {
            this.transform = new Transform(Vector3.Zero);
            this.localRotation = localRotation;
            this.force = force; 
            this.key = key;

            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "truster"));
        }

        public override void Update()
        {
            transform.Position = new Vector3(new Vector4(this.parentTransform.Position) + new Vector4(localPosition) * Matrix4.CreateFromQuaternion(new Quaternion(this.parentTransform.Rotation)));
            transform.Rotation = this.parentTransform.Rotation + this.localRotation;

            if (key == Keys.Unknown || Game.input.IsKeyDown(key))
            {
                Vector3 forceDirection = new Vector3(new Vector4(0f,force,0f,0f) * Matrix4.CreateFromQuaternion(new Quaternion(this.transform.Rotation)));
                parentRigbody.AddForce(transform.Position - parentTransform.Position, forceDirection * Game.deltaTime);
            }


        }
    }
}
