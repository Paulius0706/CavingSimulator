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
using CavingSimulator2.GameLogic.UI.Views.Components;
using CavingSimulator2.GameLogic.Objects.SpaceShips;

namespace CavingSimulator2.GameLogic.Objects.SpaceShipParts
{
    public class PlayerCabin : Part
    {
        public Player player;
        public Inventory inventory;
        public ChunkGenerator chunkGenerator;
        public Selector selector;
        public Vector3 curerentVelocity;
        public Vector3 currentAngularVelocity;

        public Dictionary<Vector3i, Part> parts = new Dictionary<Vector3i, Part>();

        public PlayerCabin(Transform transform) : base()
        {
            this.transform = transform;


            this.rigBody = new RigBody(this.transform, 1, new Vector3i(5, 5, 5));
            this.inventory = new Inventory(this);

            //Plane.Create(this);

            this.player = new Player(this.transform, this.rigBody, this);
            this.chunkGenerator = new ChunkGenerator(this.transform);

            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "frame"));
        }
        public void RemoveAllParts() { var keys = parts.Keys.ToList(); foreach (Vector3i key in keys) { RemovePart(key);} }
        public void LoadPlane() { RemoveAllParts(); Plane.Create(this); }
        public void LoadEmpty() { RemoveAllParts(); }
        public void LoadDrone() { RemoveAllParts(); Drone.Create(this); }
        public int AddPart(Vector3i localPosition, Part part)
        {
            if (localPosition == Vector3i.Zero) return -1;
            // set object to exat pos
            part.parentTransform = transform;
            part.playerCabin = this;
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
            if (localPosition == Vector3i.Zero) return -1;
            // set object to exat pos
            part.parentTransform = transform;
            part.playerCabin = this;
            part.localPosition = localPosition;
            part.localRotation = localRotation;
            part.parentRigbody = rigBody;
            if (part.RigBody is not null)
            {
                part.RigBody.Position = new Vector3(new Vector4(this.rigBody.Position) + new Vector4(localPosition) * Matrix4.CreateFromQuaternion(this.rigBody.Rotation));
                part.RigBody.Rotation = this.rigBody.Rotation;
                part.RigBody.LinearVelocity = Vector3.Zero;
                rigBody.Weld(localPosition, part.RigBody, Vector3.One, part.mass);
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
            if (localPosition == Vector3i.Zero) return;
            if (!parts.ContainsKey(localPosition)) return;
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
            #region Remove this later

            #endregion

            inventory.Update();
            rigBody.Update();
            player.Update();
            chunkGenerator.Update();
            curerentVelocity = rigBody.LinearVelocity;
            currentAngularVelocity = rigBody.AngularVelocity;
            foreach(var part in parts.Values) { part.Update(); }
            BuilderUpdate();            
            
        }
        public void AddSelector()
        {
            if(selector == null)
            {
                selector = new Selector(this.transform, this);
                selector.frozenPos = this.transform.Position;
                selector.frozenRotation = this.transform.Rotation;
                rigBody.AngularVelocity = Vector3.Zero;
                rigBody.LinearVelocity = Vector3.Zero;
                Game.UI.UseView("builder");
                HintInfo hintInfo = Game.UI.GetView<HintInfo>("HintInfo");
                if (hintInfo is not null && Game.UI.Use == "builder")
                {
                    hintInfo.Update(1, "   Bind Key - L   ");
                }
            }
        }
        public void RemoveSelector()
        {
            if(selector != null)
            {
                selector.Dispose(); selector = null;
                Game.UI.UseView("game");
            }
        }
        private void BuilderUpdate()
        {
            if (Inputs.BuildMode)
            {
                if (selector != null) 
                {
                    RemoveSelector();
                }
                else
                {
                    AddSelector();
                }
            }
            if (selector != null)
            {
                if (Inputs.InputBindMode)
                {
                    selector.keyBind = selector.keyBind == Selector.KeyBind.no ? Selector.KeyBind.key : Selector.KeyBind.no;

                    HintInfo hintInfo = Game.UI.GetView<HintInfo>("HintInfo");
                    if(hintInfo is not null && Game.UI.Use == "builder")
                    {
                        if (selector.keyBind == Selector.KeyBind.key)
                        {
                            hintInfo.Update(1, " UnBind Key - L   ");
                        }
                        if (selector.keyBind == Selector.KeyBind.no)
                        {
                            hintInfo.Update(1, "   Bind Key - L   ");
                        }
                    }
                    
                    Debug.WriteLine(nameof(selector.keyBind) + " => " + selector.keyBind);
                }
                selector.Update();

                rigBody.Position = selector.frozenPos;
                //rigBody.Rotation = selector.frozenRotation;
                rigBody.AngularVelocity *= 0.1f;
                rigBody.LinearVelocity = Vector3.Zero;
            }

        }
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
