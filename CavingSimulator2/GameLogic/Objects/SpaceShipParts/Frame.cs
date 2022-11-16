using CavingSimulator.GameLogic.Components;
using CavingSimulator2.GameLogic.Components.Physics;
using CavingSimulator2.Render.Meshes.SpaceShipParts;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.Objects.SpaceShipParts
{
    public class Frame : Part
    {

        public Frame(Transform transform)
        {
            this.transform = transform;
            this.rigBody = new RigBody(this.transform, Vector3.One, 1, new Vector3i(2, 2, 2));

            this.renderer = new Renderer();
            this.renderer.AddMesh(new BoxMesh(this.transform, "container"));
        }
        public Frame()
        {
            this.transform = new Transform(Vector3.Zero);
            this.rigBody = new RigBody(this.transform, Vector3.One, 1, Vector3i.Zero);

            this.renderer = new Renderer();
            this.renderer.AddMesh(new BoxMesh(this.transform, "container"));
        }

        public override void Render()
        {
            if (Game.shaderPrograms.Use == "object") renderer.Render();
        }

        public override void Update()
        {
            rigBody.Update();
        }


        public RigBody RigBody
        {
            get { return rigBody; }
            set { rigBody = value; }
        }
    }
}
