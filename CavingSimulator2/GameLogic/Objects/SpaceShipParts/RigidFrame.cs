﻿using CavingSimulator.GameLogic.Components;
using CavingSimulator2.GameLogic.Components.Physics;
using CavingSimulator2.Render.Meshes;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.Objects.SpaceShipParts
{
    public class RigidFrame : Part
    {

        public RigidFrame(Transform transform) : base()
        {
            this.transform = transform;
            //this.rigBody = new RigBodyLegacy(this.transform, Vector3.One, 1, new Vector3i(2, 2, 2));

            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "frame"));
        }
        public RigidFrame()
        {
            this.transform = new Transform(Vector3.Zero);
            //this.rigBody = new RigBodyLegacy(this.transform, Vector3.One, 1, Vector3i.Zero);

            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "frame"));
        }

        public override void Render()
        {
            if (Game.shaderPrograms.Use == "object") renderer.Render();
        }

        public override void Update()
        {
            if (Game.UI.Use == "meniu") return;
            rigBody.Update();
        }


        public RigBody RigBody
        {
            get { return rigBody; }
            set { rigBody = value; }
        }
    }
}
