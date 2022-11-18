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
using OpenTK.Windowing.GraphicsLibraryFramework;
using CavingSimulator2.Render.Meshes;

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


            this.rigBody = new RigBody(this.transform, 1, new Vector3i(3, 3, 3));

            // auto redirection
            //AddPart(new Vector3i( 0,  0, 2), new Gimbal(new Vector3(0f, 0f, +30f)));
            //AddPart(new Vector3i( 0,  0,-2), new Gimbal(new Vector3(0f, 0f, -30f)));

            // Up Down
            AddPart(new Vector3i(0, 0, 1), new Gimbal(new Vector3(0f, 0f,+60f), Keys.Space));
            AddPart(new Vector3i(0, 0,-1), new Gimbal(new Vector3(0f, 0f,-30f), Keys.LeftShift));

            // rotate right left
            AddPart(new Vector3i(+1, -1, 0), new Truster(new Vector3(-30f, 0f, 0f), Keys.D));
            AddPart(new Vector3i(-1, +1, 0), new Truster(new Vector3(+30f, 0f, 0f), Keys.D));

            AddPart(new Vector3i(-1, -1, 0), new Truster(new Vector3(+30f, 0f, 0f), Keys.A));
            AddPart(new Vector3i(+1, +1, 0), new Truster(new Vector3(-30f, 0f, 0f), Keys.A));

            // foward back
            AddPart(new Vector3i(0, 2, 0), new Truster(new Vector3(0f, +30f, 0f), Keys.W));

            AddPart(new Vector3i(0, -1, 0), new Gimbal(new Vector3(0f, -30f, 0f), Keys.S));


            this.player = new Player(this.transform, this.rigBody);
            this.chunkGenerator = new ChunkGenerator(this.transform);

            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "frame"));
        }
        public int AddPart(Vector3i localPosition, Part part)
        {
            // set object to exat pos
            part.parentTransform = transform;
            part.localPosition = localPosition;
            part.parentRigbody = rigBody;
            if(part.RigBody is not null)
            {
                part.RigBody.Position = new Vector3(new Vector4(this.rigBody.Position) + new Vector4(localPosition) * Matrix4.CreateFromQuaternion(new Quaternion(this.rigBody.Rotation)));
                part.RigBody.Rotation = this.rigBody.Rotation;
                part.RigBody.LinearVelocity = Vector3.Zero;
                rigBody.Weld(localPosition, part.RigBody, Vector3.One, 1);
            }
            else
            {
                rigBody.StaticWeld(localPosition, Vector3.One, 1);
            }
            parts.Add(localPosition, part);
            return part.id;
        }
        public int AddPart(Vector3i localPosition, Vector3 localRotation, Part part)
        {
            // set object to exat pos
            part.parentTransform = transform;
            part.localPosition = localPosition;
            part.localRotation = localRotation;
            part.parentRigbody = rigBody;
            if (part.RigBody is not null)
            {
                part.RigBody.Position = new Vector3(new Vector4(this.rigBody.Position) + new Vector4(localPosition) * Matrix4.CreateFromQuaternion(new Quaternion(this.rigBody.Rotation)));
                part.RigBody.Rotation = this.rigBody.Rotation;
                part.RigBody.LinearVelocity = Vector3.Zero;
                rigBody.Weld(localPosition, part.RigBody, Vector3.One, 1);
            }
            else
            {
                rigBody.StaticWeld(localPosition, Vector3.One, 1);
            }
            parts.Add(localPosition, part);
            return part.id;
        }
        public void RemovePart(Vector3i localPosition)
        {
            if (parts[localPosition].RigBody is null) 
            {
                RigBody.StaticUnweld(localPosition);
            }
            else
            {
                RigBody.UnWeld(localPosition);
            }
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
