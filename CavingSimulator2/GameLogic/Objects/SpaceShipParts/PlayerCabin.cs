using CavingSimulator.GameLogic.Components;
using CavingSimulator2.GameLogic.Components.Physics;
using CavingSimulator2.GameLogic.Components;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Windowing.GraphicsLibraryFramework;
using CavingSimulator2.Render.Meshes;
using CavingSimulator2.Helpers;
using CavingSimulator2.Debugger;

namespace CavingSimulator2.GameLogic.Objects.SpaceShipParts
{
    public class PlayerCabin : Part
    {
        public Player player;
        public Inventory inventory;
        public ChunkGenerator chunkGenerator;
        public Selector selector;

        public Dictionary<Vector3i, Part> parts = new Dictionary<Vector3i, Part>();

        public PlayerCabin(Transform transform) : base()
        {
            this.transform = transform;


            this.rigBody = new RigBody(this.transform, 1, new Vector3i(5, 5, 5));

            // Up Down
            AddPart(new Vector3i(0, 0, 1), new Gimbal(new Vector3(0f, 0f,+120f), Keys.Space));
            AddPart(new Vector3i(0, 0,-1), new Gimbal(new Vector3(0f, 0f,-30f), Keys.LeftShift));

            // rotate right left
            AddPart(new Vector3i(+1, -1, 0), new Thruster(new Quaternion(0f,0f, MathHelper.DegreesToRadians(+90f)), 60f, Keys.D));
            AddPart(new Vector3i(-1, +1, 0), new Thruster(new Quaternion(0f,0f, MathHelper.DegreesToRadians(-90f)), 60f, Keys.D));

            AddPart(new Vector3i(-1, -1, 0), new Thruster(new Quaternion(0f, 0f, MathHelper.DegreesToRadians(-90f)), 60f, Keys.A));
            AddPart(new Vector3i(+1, +1, 0), new Thruster(new Quaternion(0f, 0f, MathHelper.DegreesToRadians(+90f)), 60f, Keys.A));

            // foward back
            AddPart(new Vector3i(0, -1, 0), new Thruster(new Quaternion(0f, 0f, 0f), 30f, Keys.W));
            AddPart(new Vector3i(0, +2, 0), new Thruster(new Quaternion(0f, 0f, MathHelper.DegreesToRadians(+180f)), 60f, Keys.S));

            //AddPart(new Vector3i(0, +1, 0), new GyroScope(Vector3.Zero, 150f, Vector3.Zero, Keys.G));

            this.player = new Player(this.transform, this.rigBody, this);
            this.inventory = new Inventory(this);
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
                part.RigBody.Position = new Vector3(new Vector4(this.rigBody.Position) + new Vector4(localPosition) * Matrix4.CreateFromQuaternion(this.rigBody.Rotation));
                part.RigBody.Rotation = this.rigBody.Rotation;
                part.RigBody.LinearVelocity = Vector3.Zero;
                rigBody.Weld(localPosition, part.RigBody, Vector3.One, 1);
            }
            else
            {
                rigBody.StaticWeld(part.colliderShape,localPosition, part.localRotation, 1);
            }
            parts.Add(localPosition, part);
            return part.id;
        }
        public int AddPart(Vector3i localPosition, Quaternion localRotation, Part part)
        {
            // set object to exat pos
            part.parentTransform = transform;
            part.localPosition = localPosition;
            part.localRotation = localRotation;
            part.parentRigbody = rigBody;
            if (part.RigBody is not null)
            {
                part.RigBody.Position = new Vector3(new Vector4(this.rigBody.Position) + new Vector4(localPosition) * Matrix4.CreateFromQuaternion(this.rigBody.Rotation));
                part.RigBody.Rotation = this.rigBody.Rotation;
                part.RigBody.LinearVelocity = Vector3.Zero;
                rigBody.Weld(localPosition, part.RigBody, Vector3.One, 1);
            }
            else
            {
                rigBody.StaticWeld(part.colliderShape,localPosition, part.localRotation, 1f);
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
            if (Game.shaderPrograms.Use == "object" && selector != null) selector.Render();

        }
        public override void Update()
        {
            inventory.Update();
            rigBody.Update();
            player.Update();
            chunkGenerator.Update();
            foreach(var part in parts.Values) { part.Update(); }
            BuilderUpdate();            
            
        }
        private void BuilderUpdate()
        {
            if (Inputs.BuildMode)
            {
                if (selector != null) { selector.Dispose(); selector = null; }
                else
                {
                    selector = new Selector(this.transform, this);
                    selector.frozenPos = this.transform.Position;
                    selector.frozenRotation = this.transform.Rotation;
                    rigBody.AngularVelocity = Vector3.Zero;
                    rigBody.LinearVelocity = Vector3.Zero;
                }
            }
            if (selector != null)
            {
                if (Inputs.InputBindMode)
                {
                    selector.KeyBind = !selector.KeyBind;
                    Debug.WriteLine(nameof(selector.KeyBind) + " => " + selector.KeyBind);
                }
                selector.Update();

                rigBody.Position = selector.frozenPos;
                //rigBody.Rotation = selector.frozenRotation;
                rigBody.AngularVelocity *= 0.1f;
                rigBody.LinearVelocity = Vector3.Zero;
            }

        }
        //public Player player;
        //public Inventory inventory;
        //public ChunkGenerator chunkGenerator;
        //public Selector selector;

        //public Dictionary<Vector3i, Part> parts = new Dictionary<Vector3i, Part>();
        protected override void AbstractDispose()
        {
            if (player is not null) player.Dispose();
            if (inventory is not null) inventory.Dispose();
            if (chunkGenerator is not null) chunkGenerator.Dispose();
            if (selector is not null) selector.Dispose();
            foreach(Part part in parts.Values)
            {
                part.Dispose();
            }
        }

    }
}
