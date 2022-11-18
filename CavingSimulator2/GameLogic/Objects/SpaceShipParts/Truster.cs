using CavingSimulator.GameLogic.Components;
using CavingSimulator2.Render.Meshes.SpaceShipParts;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Windowing.GraphicsLibraryFramework;
using CavingSimulator2.Render.Meshes;

namespace CavingSimulator2.GameLogic.Objects.SpaceShipParts
{
    public class Truster : Part
    {
        public Vector3 localForceDirection;
        public Keys key;
        public Truster(Transform transform, Vector3 localForceDirection, Keys key = Keys.Unknown) : base()
        {
            this.transform = transform;
            this.localForceDirection = localForceDirection;
            this.key = key;

            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "gimbal"));
        }
        public Truster(Vector3 localForceDirection, Keys key = Keys.Unknown) : base()
        {
            this.transform = new Transform(Vector3.Zero);
            this.localForceDirection = localForceDirection;
            this.key = key;

            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "gimbal"));
        }

        public override void Update()
        {
            transform.Position = new Vector3(new Vector4(this.parentTransform.Position) + new Vector4(localPosition) * Matrix4.CreateFromQuaternion(new Quaternion(this.parentTransform.Rotation)));
            transform.Rotation = this.parentTransform.Rotation;

            if (key == Keys.Unknown || Game.input.IsKeyDown(key))
            {
                Vector3 forceDirection = new Vector3(new Vector4(localForceDirection) * Matrix4.CreateFromQuaternion(new Quaternion(this.transform.Rotation)));
                parentRigbody.AddForce(transform.Position - parentTransform.Position, forceDirection * Game.deltaTime);
            }

            transform.Rotation += this.localRotation;
            
        }
    }
}
