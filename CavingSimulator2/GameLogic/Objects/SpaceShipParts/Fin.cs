using CavingSimulator.GameLogic.Components;
using CavingSimulator2.Render.Meshes;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.Objects.SpaceShipParts
{
    public class Fin : Wing
    {
        public Fin(Transform transform, Quaternion localRotation) : base()
        {
            ImageName = "finImage";
            this.transform = transform;
            this.localRotation = localRotation;
            this.key = key;
            this.mass = 0.1f;
            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "fin"));
        }
        public Fin(Quaternion localRotation) : base()
        {
            ImageName = "finImage";
            this.transform = new Transform(Vector3.Zero);
            this.localRotation = localRotation;
            this.key = key;
            this.mass = 0.1f;
            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "fin"));
        }
        public override Part Create()
        {
            return new Fin(localRotation);
        }
    }
}
