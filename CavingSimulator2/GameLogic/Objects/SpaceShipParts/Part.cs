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
            if (rigBody is not null) { Console.WriteLine("\n\n\n\n\n\n " + "disposed rig"); rigBody.Dispose(); }
            if (renderer is not null) renderer.Dispose();
        }

        public RigBody RigBody
        {
            get { return rigBody; }
            set { rigBody = value; }
        }
    }
}
