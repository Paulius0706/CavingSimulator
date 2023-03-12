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
    public class GyroScope : Part
    {
        public Vector3 targetRotation;
        public float force;
        public bool active;
        public Keys key;
        public GyroScope(Transform transform, Vector3 localRotation, float force, Vector3 targetRotation, Keys key = Keys.Unknown) : base()
        {
            ImageName = "gyroImage";
            this.active = true;
            this.transform = transform;
            this.localRotation = localRotation;
            this.targetRotation = targetRotation;
            this.force = force;
            this.key = key;

            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "gyroscope"));
        }
        public GyroScope(Vector3 localRotation, float force, Vector3 targetRotation, Keys key = Keys.Unknown) : base()
        {
            ImageName = "gyroImage";
            this.active = true;
            this.transform = new Transform(Vector3.Zero);
            this.localRotation = localRotation;
            this.targetRotation = targetRotation;
            this.force = force;
            this.key = key;

            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "gyroscope"));
        }

        public override void Update()
        {
            Vector3 localPos = new Vector3(new Vector4(localPosition) * Matrix4.CreateFromQuaternion(new Quaternion(this.parentTransform.Rotation)));
            transform.Position = this.parentTransform.Position + localPos;
            var qRotation = (Quaternion.FromEulerAngles(this.parentTransform.Rotation) * Quaternion.FromEulerAngles(this.localRotation));
            transform.Rotation = qRotation.ToEulerAngles();

            if (key != Keys.Unknown && Game.input.IsKeyDown(key)) { active = !active; }
            if (active)
            {
                Vector3 rotationTargetDelta = (targetRotation - parentRigbody.Rotation);
                rotationTargetDelta.Z = 0;
                if (rotationTargetDelta == Vector3.Zero) return;
                Vector3 rotationForce = rotationTargetDelta.Normalized() * force * Game.deltaTime;
                parentRigbody.AddAngularVelocity(rotationForce);
            }
        }
        public override Part Create()
        {
            return new GyroScope(localRotation, force,targetRotation, key);
        }
    }
}
