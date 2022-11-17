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

        public PlayerCabin(Transform transform) : base()
        {
            this.transform = transform;


            this.rigBody = new RigBody(this.transform, Vector3.One, 1, new Vector3i(2, 2, 2));

            AddPart(new Vector3i(1, 1, 1), new Frame());
            //RemovePart(new Vector3i(1, 1, 1));
            AddPart(   new Vector3i(-1, -1, 1), new Frame());
            //RemovePart(new Vector3i(-1, -1, 1));
            AddPart(   new Vector3i( 1, -1, 1), new Frame());
            //RemovePart(new Vector3i( 1, -1, 1));
            AddPart(   new Vector3i(-1,  1, 1), new Frame());
            //RemovePart(new Vector3i(-1,  1, 1));

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
            this.renderer.AddMesh(new BoxMesh(this.transform, "container"));
        }
        public int AddPart(Vector3i localPosition, Part part)
        {
            // set object to exat pos
            part.RigBody.Position = new Vector3(new Vector4(this.rigBody.Position) + new Vector4(localPosition) * Matrix4.CreateFromQuaternion(new Quaternion(this.rigBody.Rotation)));
            part.RigBody.Rotation = this.rigBody.Rotation;

            //part.RigBody.Rotation = this.transform.Rotation;
            //part.RigBody.Position = this.transform.Position;
            part.RigBody.LinearVelocity = Vector3.Zero;

            rigBody.Weld(localPosition, part.RigBody, Vector3.One, 1);
            parts.Add(localPosition, part);
            return part.id;
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
