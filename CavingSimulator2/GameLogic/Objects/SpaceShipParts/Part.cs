using BepuPhysics;
using CavingSimulator.GameLogic.Components;
using CavingSimulator2.GameLogic.Components;
using CavingSimulator2.GameLogic.Components.Physics;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
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
        public string ImageName = "";
        public Transform transform;
        protected Renderer renderer;
        protected RigBody rigBody;
        public RigBody parentRigbody;
        public PlayerCabin playerCabin;
        public Transform parentTransform;
        public float mass = 1f;
        public Vector3i localPosition;
        public Quaternion localRotation = Quaternion.Identity;
        public ShapeType colliderShape = ShapeType.box;
        public Keys key;
        public Keys nkey;

        public Part() { }

        public override void Render()
        {
            if (Game.shaderPrograms.Use == "object") renderer.Render();
        }

        public override void Update() 
        {
            if(rigBody is not null) rigBody.Update();
        }

        public override bool TryGetRigBody(out RigBody rigBody) { rigBody = this.rigBody; return true; }

        public virtual Part Create()
        {
            return null;
        }

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
