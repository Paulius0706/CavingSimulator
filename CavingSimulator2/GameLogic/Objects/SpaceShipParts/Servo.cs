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
    public class Servo : Part
    {
        public float force;
        public bool active;
        //public Keys key;
        public Vector3 localUp;
        public Servo(Transform transform, Quaternion localRotation, float force, Quaternion targetRotation, Keys key = Keys.Unknown, Keys nkey = Keys.Unknown) : base()
        {
            ImageName = "servo";
            this.active = true;
            this.transform = transform;
            this.localRotation = localRotation;
            this.force = force;
            this.key = key;

            //this.localUp = new Vector3(Vector4.UnitZ * Matrix4.CreateFromQuaternion(this.localRotation));

            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "servo"));
        }
        public Servo(Quaternion localRotation, float force, Quaternion targetRotation, Keys key = Keys.Unknown, Keys nkey = Keys.Unknown ) : base()
        {
            ImageName = "servo";
            this.active = true;
            this.transform = new Transform(Vector3.Zero);
            this.localRotation = localRotation;
            this.force = force;
            this.key = key;



            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "servo"));
        }

        public override void Update()
        {
            Vector3 globalPos = new Vector3(new Vector4(localPosition) * Matrix4.CreateFromQuaternion(this.parentTransform.Rotation));
            transform.Position = this.parentTransform.Position + globalPos;
            transform.Rotation = (this.parentTransform.Rotation * this.localRotation);

            if (Game.UI.Use == "meniu") return;

            if (key != Keys.Unknown && Game.input.IsKeyDown(key))
            {
                this.localUp = new Vector3(Vector4.UnitZ * Matrix4.CreateFromQuaternion(this.localRotation));
                Vector3 upVec = (new Vector3(Vector4.UnitZ * Matrix4.CreateFromQuaternion(this.transform.Rotation))).Normalized();
                Vector3 rightVec = (new Vector3(Vector4.UnitX * Matrix4.CreateFromQuaternion(this.transform.Rotation))).Normalized();
                Vector3 leftVec = -rightVec;
                Vector3 rightPos = globalPos + rightVec;
                Vector3 leftPos = globalPos + leftVec;

                float multiply = 0.25f;
                parentRigbody.AddForce(rightPos,  upVec * Game.deltaTime * force * multiply);
                parentRigbody.AddForce(leftPos,  -upVec * Game.deltaTime * force * multiply);
            }
            if (nkey != Keys.Unknown && Game.input.IsKeyDown(nkey))
            {
                this.localUp = new Vector3(Vector4.UnitZ * Matrix4.CreateFromQuaternion(this.localRotation));
                Vector3 upVec = (new Vector3(Vector4.UnitZ * Matrix4.CreateFromQuaternion(this.transform.Rotation))).Normalized();
                Vector3 rightVec = (new Vector3(Vector4.UnitX * Matrix4.CreateFromQuaternion(this.transform.Rotation))).Normalized();
                Vector3 leftVec = -rightVec;
                Vector3 rightPos = globalPos + rightVec;
                Vector3 leftPos = globalPos + leftVec;

                float multiply = 0.25f;
                parentRigbody.AddForce(rightPos, -upVec * Game.deltaTime * force * multiply);
                parentRigbody.AddForce(leftPos,   upVec * Game.deltaTime * force * multiply);
            }
            
        }
        public override Servo Create()
        {
            return new Servo(localRotation, force, Quaternion.Identity, key);
        }
    }
}
