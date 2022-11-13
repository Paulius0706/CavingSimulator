using CavingSimulator.GameLogic.Components;
using CavingSimulator2.GameLogic.Components;
using CavingSimulator2.GameLogic.Components.Colliders;
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
    public class CameraObject : BaseObject
    {
        public Transform transform;
        Renderer renderer;
        Collider collider;
        RigBody rigBody;
        Player player;
        public ChunkGenerator chunkGenerator;

        public CameraObject(Transform transform) : base()
        {
            this.transform = transform;
            this.transform.baseObject = this;

            this.renderer = new Renderer();
            this.renderer.AddMesh(new BoxMesh(transform, "container"));
            this.collider = new Collider( 
                transform,
                new Vector2(-0.5f, 0.5f),
                new Vector2(-0.5f, 0.5f),
                new Vector2(-0.5f, 0.5f),
                Vector3.Zero,
                Vector3i.One * 2);

            this.rigBody = new RigBody(transform,collider);
            this.collider.rigBody = this.rigBody;

            this.player = new Player(transform, rigBody);

            this.chunkGenerator = new ChunkGenerator(transform);
        }

        public override void Render()
        {
            base.Render();
            if (Game.shaderPrograms.Use == "object") renderer.Render();
            if (Game.shaderPrograms.Use == "block") chunkGenerator.Render();
        }
        public override void Update()
        {
            base.Update();
            rigBody.Update();
            player.Update();
            chunkGenerator.Update();
        }

    }
}
