using BepuPhysics;
using CavingSimulator.GameLogic.Components;
using CavingSimulator2.GameLogic.Components;
using CavingSimulator2.GameLogic.Components.Physics;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.Objects.SpaceShipParts
{
    public abstract class Part : BaseObject
    {
        public Transform transform;
        protected Renderer renderer;
        protected RigBody rigBody;
        public RigBody parentRigbody;
        public Transform parentTransform;
        public Vector3i localPosition;
        public Vector3 localRotation = Vector3.Zero;

        public Part() { }

        public override void Render()
        {
            if (Game.shaderPrograms.Use == "object") renderer.Render();
        }

        public override void Update() 
        {
            rigBody.Update();
        }

        public override bool TryGetRigBody(out RigBody rigBody) { rigBody = this.rigBody; return true; }

        protected override void AbstractDispose()
        {
            if (rigBody is not null) { rigBody.Dispose(); }
            if (renderer is not null) { renderer.Dispose(); }
        }

        public RigBody RigBody
        {
            get { return rigBody; }
            set { rigBody = value; }
        }
    }
}
