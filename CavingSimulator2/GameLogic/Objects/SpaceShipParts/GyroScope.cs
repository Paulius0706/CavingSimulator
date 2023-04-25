using CavingSimulator.GameLogic.Components;
using CavingSimulator2.Debugger;
using CavingSimulator2.Render.Meshes;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.Objects.SpaceShipParts
{
    public class GyroScope : Part
    {
        Transform coreTransform;
        Mesh core;
        public float force;
        public bool active;
        //public Keys key;
        public Vector3 localUp;
        public GyroScope(Transform transform, Quaternion localRotation, float force, Quaternion targetRotation, Keys key = Keys.Unknown) : base()
        {
            ImageName = "gyroImage";
            this.active = true;
            this.transform = transform;
            this.localRotation = localRotation;
            this.coreTransform = new Transform(transform.Position);
            this.force = force;
            this.key = key;

            //this.localUp = new Vector3(Vector4.UnitZ * Matrix4.CreateFromQuaternion(this.localRotation));

            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "gyroscope"));
            this.core = new Mesh(this.coreTransform, "gyroscopeCore");
        }
        public GyroScope(Quaternion localRotation, float force, Quaternion targetRotation, Keys key = Keys.Unknown) : base()
        {
            ImageName = "gyroImage";
            this.active = true;
            this.transform = new Transform(Vector3.Zero);
            this.localRotation = localRotation;
            this.coreTransform = new Transform(transform.Position);
            this.force = force;
            this.key = key;

            

            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "gyroscope"));
            this.core = new Mesh(this.coreTransform, "gyroscopeCore");
        }

        public override void Update()
        {
            Vector3 globalPos = new Vector3(new Vector4(localPosition) * Matrix4.CreateFromQuaternion(this.parentTransform.Rotation));
            transform.Position = this.parentTransform.Position + globalPos;
            transform.Rotation = (this.parentTransform.Rotation * this.localRotation);

            coreTransform.Position = transform.Position;
            coreTransform.Rotation = localRotation;
            //SetCoreRotation();

            if (Game.UI.Use == "meniu") return;
            


            if (key != Keys.Unknown && Game.input.IsKeyPressed(key)) { active = !active; }
            if (active)
            {

                this.localUp = new Vector3(Vector4.UnitZ * Matrix4.CreateFromQuaternion(this.localRotation));
                Vector3 upVec = (new Vector3(Vector4.UnitZ * Matrix4.CreateFromQuaternion(this.transform.Rotation))).Normalized();
                Vector3 rightVec = (new Vector3(Vector4.UnitX * Matrix4.CreateFromQuaternion(this.transform.Rotation))).Normalized();
                Vector3 leftVec = -rightVec;
                Vector3 rightPos = globalPos + rightVec;
                Vector3 leftPos = globalPos + leftVec;

                
                float rightDist = (localUp - rightVec).Length;
                float leftDist = (localUp - leftVec).Length;

                float multiply = 0.25f;
                if (rightDist > leftDist)
                {
                    parentRigbody.AddForce(rightPos, upVec * Game.deltaTime * force * multiply);
                    parentRigbody.AddForce(leftPos, -upVec * Game.deltaTime * force * multiply);
                }
                if (rightDist < leftDist)
                {
                    parentRigbody.AddForce(rightPos, -upVec * Game.deltaTime * force * multiply);
                    parentRigbody.AddForce(leftPos,  upVec * Game.deltaTime * force * multiply);
                }


            }
        }
        private void SetCoreRotation()
        {
            Vector3 localFoward = new Vector3(new Vector4(Vector3.UnitY) * Matrix4.CreateFromQuaternion(this.localRotation));
            Vector3 localUP = new Vector3(new Vector4(Vector3.UnitZ) * Matrix4.CreateFromQuaternion(this.localRotation));
            Vector3 globalFoward = new Vector3(new Vector4(Vector3.UnitY) * Matrix4.CreateFromQuaternion(this.transform.Rotation));
            Vector3 globalUP = new Vector3(new Vector4(Vector3.UnitZ) * Matrix4.CreateFromQuaternion(this.transform.Rotation));
            
            float angletoSurface = MathHelper.DegreesToRadians(90f) - Vector3.CalculateAngle(localUP, globalFoward);
            Vector3 axisToNormalVec = Vector3.Cross(localUP, globalFoward);
            Quaternion rotationToPlane = Quaternion.FromAxisAngle(axisToNormalVec, -angletoSurface);
            localUP = new Vector3(new Vector4(localUP) * Matrix4.CreateFromQuaternion(rotationToPlane));

            float angle = Vector3.CalculateAngle(globalUP, localUP);

            localRotation.ToEulerAngles(out Vector3 angles);
            transform.Rotation.ToEulerAngles(out Vector3 globalAngles);
            coreTransform.Rotation = Quaternion.FromEulerAngles(angles.X,globalAngles.Y,globalAngles.Z);
        }
        public override void Render()
        {
            base.Render();
            if (core is not null) core.Render();
        }
        public override Part Create()
        {
            return new GyroScope(localRotation, force,Quaternion.Identity, key);
        }
    }
}
