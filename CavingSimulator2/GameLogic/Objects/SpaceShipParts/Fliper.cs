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
    public class Fliper : Part
    {
        public Fliper() { }
        public Fliper(Transform transform, Quaternion localRotation) : base()
        {
            ImageName = "filperImage";
            this.transform = transform;
            this.localRotation = localRotation;
            this.key = key;
            this.mass = 0.1f;
            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "filper"));
        }
        public Fliper(Quaternion localRotation) : base()
        {
            ImageName = "filperImage";
            this.transform = new Transform(Vector3.Zero);
            this.localRotation = localRotation;
            this.key = key;
            this.mass = 0.1f;
            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "filper"));
        }
        public override void Update()
        {
            Vector3 localPos = new Vector3(new Vector4(localPosition) * Matrix4.CreateFromQuaternion(parentTransform.Rotation));
            transform.Position = this.parentTransform.Position + localPos;
            var qRotation = (this.parentTransform.Rotation * this.localRotation);
            transform.Rotation = qRotation;
            if (Game.UI.Use == "meniu") return;
            if (playerCabin == null) return;
            Vector3 upVec = (new Vector3(Vector4.UnitZ * Matrix4.CreateFromQuaternion(this.transform.Rotation))).Normalized();

            Vector3 velocity = playerCabin.curerentVelocity - playerCabin.currentAngularVelocity * localPosition.EuclideanLength;
            float currentVelosity = velocity.Length;

            if (currentVelosity != 0f)
            {
                float angle = Vector3.CalculateAngle(velocity / currentVelosity, upVec);
                float multiplier = -MathF.Cos(angle) * 5f * currentVelosity * Game.deltaTime;
                parentRigbody.AddForce(localPos, upVec * multiplier);
            }
        }
        public override Part Create()
        {
            return new Fliper(this.localRotation);
        }
    }
}
