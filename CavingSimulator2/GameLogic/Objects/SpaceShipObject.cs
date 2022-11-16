using CavingSimulator.GameLogic.Components;
using CavingSimulator2.GameLogic.Components;
using CavingSimulator2.GameLogic.Components.Physics;
using CavingSimulator2.Render.Meshes;
using CavingSimulator2.Render.Meshes.SpaceShipParts;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.Objects
{
    public class SpaceShipObject : BaseObject
    {
        public Transform transform;
        Renderer renderer;
        RigBody rigBody;
        Player player;
        public ChunkGenerator chunkGenerator;

        public SpaceShipObject(Transform transform) : base()
        {
            this.transform = transform;
            this.transform.baseObject = this;

            
            Transform childTransform = new Transform(new Vector3(1f, 1f, 1f) + transform.Position); // + transform.Position


            this.rigBody = new RigBody(this.transform, Vector3.One, 1, new Vector3i(2, 2, 2));
            //transform.Body = this.rigBody.dynamicBody.GetBodyReference();
            this.rigBody.AddChildren(new Vector3(1f,0f,0f), childTransform, Vector3.One, 1);
            

            this.player = new Player(this.transform, this.rigBody);
            this.chunkGenerator = new ChunkGenerator(this.transform);

            this.renderer = new Renderer();
            this.renderer.AddMesh(new BoxMesh(transform, "container"));
            this.renderer.AddMesh(new BoxMesh(childTransform, "container"));
        }

        public override void Render()
        {
            base.Render();
            if (Game.shaderPrograms.Use == "object") renderer.Render();
            if (Game.shaderPrograms.Use == "block") chunkGenerator.Render();
        }
        public void AddComponent(Vector3i offset)
        {
            
        }
        public override void Update()
        {
            base.Update();
            rigBody.Update();
            player.Update();
            chunkGenerator.Update();
        }

        public override bool TryGetRigBody(out RigBody rigBody) { rigBody = this.rigBody; return true; }

    }
}
