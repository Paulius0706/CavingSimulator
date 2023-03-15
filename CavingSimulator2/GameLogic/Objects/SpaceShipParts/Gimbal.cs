using BepuPhysics;
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
        public Vector3 forceDirection;
        //public Keys key;
        public Gimbal(Transform transform, Vector3 forceDirection, Keys key = Keys.Unknown) : base()
        {
            ImageName = "gimbalImage";
            this.transform = transform;
            this.forceDirection = forceDirection;
            this.key = key;

            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "gimbal"));
        }
        public Gimbal(Vector3 forceDirection, Keys key = Keys.Unknown) : base()
        {
            ImageName = "gimbalImage";
            this.transform = new Transform(Vector3.Zero);
            this.forceDirection = forceDirection;
            this.key = key;

            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "gimbal"));
        }

        public override void Update()
        {
            Vector3 localPos = new Vector3(new Vector4(localPosition) * Matrix4.CreateFromQuaternion(this.parentTransform.Rotation));
            transform.Position = this.parentTransform.Position + localPos;
            transform.Rotation = this.parentTransform.Rotation;
            
            if(key == Keys.Unknown || Game.input.IsKeyDown(key))
            {
                parentRigbody.AddForce(localPos, forceDirection * Game.deltaTime);
                //parentRigbody.AddForce(transform.Position - parentTransform.Position, forceDirection * Game.deltaTime);
            }
        }
        public override Part Create()
        {
            return new Gimbal(forceDirection, key);
        }
    }
}
