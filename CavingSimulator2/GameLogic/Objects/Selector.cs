using CavingSimulator.GameLogic.Components;
using CavingSimulator2.Debugger;
using CavingSimulator2.GameLogic.Components.Physics;
using CavingSimulator2.Helpers;
using CavingSimulator2.Render.Meshes;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.Objects
{
    public class Selector : IDisposable
    {
        public const float scale = 1.2f;
        public Transform parentTransform;
        public Vector3i localPosition = Vector3i.Zero;
        public Vector3 frozenPos = Vector3.Zero;
        public Vector3 frozenRotation = Vector3.Zero;
        public Renderer renderer;
        public Transform transform;

        public Selector(Transform parentTransform)
        {
            this.parentTransform = parentTransform;
            this.transform = new Transform(Vector3.Zero);
            this.transform.Scale = scale * Vector3.One;

            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "selector"));
        }
        public void Update()
        {
            transform.Position = new Vector3(
                new Vector4(this.parentTransform.Position) + 
                new Vector4(new Vector3(localPosition.X, localPosition.Y, localPosition.Z)) * Matrix4.CreateFromQuaternion(new Quaternion(this.parentTransform.Rotation)));
            transform.Rotation = this.parentTransform.Rotation;

            if (Inputs.Up) localPosition += Vector3i.UnitZ;
            if (Inputs.Down) localPosition -= Vector3i.UnitZ;

            if (Inputs.Right) localPosition += Vector3i.UnitX;
            if (Inputs.Left) localPosition -= Vector3i.UnitX;

            if (Inputs.Forward) localPosition += Vector3i.UnitY;
            if (Inputs.Back) localPosition -= Vector3i.UnitY;
        }
        public void Render()
        {
            if (Game.shaderPrograms.Use == "object") renderer.Render();
        }

        public void Dispose()
        {
            renderer.Dispose();
        }
    }
}
