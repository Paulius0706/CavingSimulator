﻿using CavingSimulator.GameLogic.Components;
using CavingSimulator2.GameLogic.Components.Physics;
using CavingSimulator2.GameLogic.Components;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CavingSimulator2.Render.Meshes;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CavingSimulator2.GameLogic.Objects.SpaceShipParts
{
    public class Frame : Part
    {

        public Frame(Transform transform) : base()
        {
            ImageName = "frameImage";
            this.transform = transform;

            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "frame"));
        }
        public Frame() : base()
        {
            ImageName = "frameImage";
            this.transform = new Transform(Vector3.Zero);

            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "frame"));
        }

        public override void Update()
        {
            transform.Position = new Vector3(new Vector4(this.parentTransform.Position) + new Vector4(localPosition) * Matrix4.CreateFromQuaternion(this.parentTransform.Rotation));
            transform.Rotation = this.parentTransform.Rotation;

        }

        public override Part Create()
        {
            return new Frame();
        }
    }
}
