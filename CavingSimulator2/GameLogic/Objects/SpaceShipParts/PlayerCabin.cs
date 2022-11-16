using CavingSimulator.GameLogic.Components;
using CavingSimulator2.GameLogic.Components.Physics;
using CavingSimulator2.GameLogic.Components;
using CavingSimulator2.Render.Meshes.SpaceShipParts;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.Objects.SpaceShipParts
{
    public class PlayerCabin : Part
    {
        public Player player;
        public ChunkGenerator chunkGenerator;

        public Dictionary<Vector3i, Part> parts = new Dictionary<Vector3i, Part>();

        public PlayerCabin(Transform transform)
        {
            this.transform = transform;


            this.rigBody = new RigBody(this.transform, Vector3.One, 1, new Vector3i(2, 2, 2));

            AddPart(new Vector3i(1, 1, 1), new Frame());
            RemovePart(new Vector3i(1, 1, 1));
            AddPart(   new Vector3i(-1, -1, 1), new Frame());
            RemovePart(new Vector3i(-1, -1, 1));
            AddPart(   new Vector3i( 1, -1, 1), new Frame());
            RemovePart(new Vector3i( 1, -1, 1));
            AddPart(   new Vector3i(-1,  1, 1), new Frame());
            RemovePart(new Vector3i(-1,  1, 1));

            //AddPart(   new Vector3i(1, 1, -1), new Frame());
            //RemovePart(new Vector3i(1, 1, -1));
            //AddPart(   new Vector3i(-1, -1, -1), new Frame());
            //RemovePart(new Vector3i(-1, -1, -1));
            //AddPart(   new Vector3i(1, -1, -1), new Frame());
            //RemovePart(new Vector3i(1, -1, -1));
            //AddPart(   new Vector3i(-1, 1, -1), new Frame());
            //RemovePart(new Vector3i(-1, 1, -1));

            this.player = new Player(this.transform, this.rigBody);
            this.chunkGenerator = new ChunkGenerator(this.transform);

            this.renderer = new Renderer();
            this.renderer.AddMesh(new BoxMesh(transform, "container"));
        }
        public void AddPart(Vector3i localPosition, Part part)
        {
            // set object to exat pos
            part.transform.Position = new Vector3(new Vector4(this.transform.Position) * Matrix4.CreateTranslation(localPosition) * Matrix4.CreateFromQuaternion(new Quaternion(this.transform.Rotation)));
            part.transform.Rotation = this.transform.Rotation + part.transform.Rotation;

            part.RigBody.Rotation = this.transform.Rotation;
            part.RigBody.Position = this.transform.Position;
            part.RigBody.LinearVelocity = Vector3.Zero;

            rigBody.AddChildren(localPosition, part.transform, Vector3.One, 1);
            parts.Add(localPosition, part);
        }
        public void RemovePart(Vector3i localPosition)
        {
            parts[localPosition].Dispose();
            parts.Remove(localPosition);
        }

        public override void Render()
        {
            if (Game.shaderPrograms.Use == "object") 
            {
                renderer.Render();
                foreach (var part in parts.Values) { part.Render(); }
            } 
            if (Game.shaderPrograms.Use == "block") chunkGenerator.Render();
            
        }
        public override void Update()
        {
            rigBody.Update();
            player.Update();
            chunkGenerator.Update();
            foreach(var part in parts.Values) { part.Update(); }
        }

    }
}
