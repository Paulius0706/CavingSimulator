﻿using CavingSimulator.GameLogic.Components;
using CavingSimulator2.Debugger;
using CavingSimulator2.GameLogic.Components.Physics;
using CavingSimulator2.GameLogic.Objects.SpaceShipParts;
using CavingSimulator2.GameLogic.UI.Views.Components;
using CavingSimulator2.Helpers;
using CavingSimulator2.Render;
using CavingSimulator2.Render.Meshes;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CavingSimulator2.GameLogic.Components.Inventory;

namespace CavingSimulator2.GameLogic.Objects
{
    public class Selector : IDisposable
    {
        public const float scale = 1.2f;
        public const float lockAngleSin = 0.7071067811865475f;
        public Transform parentTransform;
        public Vector3i localPosition = Vector3i.Zero;
        public Vector3 frozenPos = Vector3.Zero;
        public Quaternion frozenRotation = Quaternion.Identity;

        public Vector3i Xaxis;
        public Vector3i Yaxis;
        public Vector3i Zaxis;
        public Vector3 lookAxis;
        public Vector3 lookRotation;

        public Renderer renderer;
        public Transform transform;
        public KeyBind keyBind = KeyBind.no;
        public PlayerCabin playerCabin;

        public enum KeyBind
        {
            no,
            key
        }

        public Selector(Transform parentTransform , PlayerCabin playerCabin)
        {
            this.playerCabin = playerCabin;
            this.parentTransform = parentTransform;
            this.transform = new Transform(Vector3.Zero);
            this.transform.Scale = scale * Vector3.One;

            this.renderer = new Renderer();
            this.renderer.AddMesh(new Mesh(this.transform, "selector"));
        }
        public void Update()
        {
            transform.Position = new Vector3(
                new Vector4(this.parentTransform.Position) + 
                new Vector4(new Vector3(localPosition.X, localPosition.Y, localPosition.Z)) * Matrix4.CreateFromQuaternion(this.parentTransform.Rotation));
            transform.Rotation = this.parentTransform.Rotation;

            Vector3 FaxisY = Vector3.Normalize(Camera.lookToPoint);
            Vector3 FaxisX = Vector3.Normalize(Vector3.Cross(Camera.lookToPoint, Camera.up));
            Vector3 FaxisZ = Vector3.Normalize(-Vector3.Cross(Camera.lookToPoint, FaxisX));

            Camera.relative_position = this.transform.Position;

            FaxisY = new Vector3(new Vector4(FaxisY) * Matrix4.CreateFromQuaternion(this.parentTransform.Rotation.Inverted()));
            FaxisZ = new Vector3(new Vector4(FaxisZ) * Matrix4.CreateFromQuaternion(this.parentTransform.Rotation.Inverted()));
            FaxisX = new Vector3(new Vector4(FaxisX) * Matrix4.CreateFromQuaternion(this.parentTransform.Rotation.Inverted()));

            Yaxis = GetAxis(FaxisY);
            Zaxis = GetAxis(FaxisZ);
            Xaxis = GetAxis(FaxisX);

            // do axis
            Vector3 look = new Vector3(new Vector4(Camera.lookToPoint) * Matrix4.CreateFromQuaternion(this.parentTransform.Rotation.Inverted()));


            lookAxis = Vector3i.UnitY;
            if (MathF.Abs(look.X) > MathF.Abs(look.Y) && MathF.Abs(look.X) > MathF.Abs(look.Z)) lookAxis = look.X > 0 ? Vector3i.UnitX : -Vector3i.UnitX;
            if (MathF.Abs(look.Z) > MathF.Abs(look.Y) && MathF.Abs(look.Z) > MathF.Abs(look.X)) lookAxis = look.Z > 0 ? Vector3i.UnitZ : -Vector3i.UnitZ;
            if (MathF.Abs(look.Y) > MathF.Abs(look.Z) && MathF.Abs(look.Y) > MathF.Abs(look.X)) lookAxis = look.Y > 0 ? Vector3i.UnitY : -Vector3i.UnitY;
            
            lookRotation = LookAxisToLooRotation(lookAxis);

            if (keyBind == KeyBind.no)
            {
                if (Inputs.Up)
                {
                    localPosition += Zaxis;
                    UpdateUI();
                }
                if (Inputs.Down)
                {
                    localPosition -= Zaxis;
                    UpdateUI();
                }

                if (Inputs.Right)
                {
                    localPosition += Xaxis;
                    UpdateUI();
                }
                if (Inputs.Left)
                {
                    localPosition -= Xaxis;
                    UpdateUI();
                }

                if (Inputs.Forward)
                {
                    localPosition += Yaxis;
                    UpdateUI();
                }
                if (Inputs.Back)
                {
                    localPosition -= Yaxis;
                    UpdateUI();
                }
                if (playerCabin.parts.ContainsKey(localPosition))
                {
                    if (Inputs.RorateRight)
                    {
                        playerCabin.parts[localPosition].localRotation = Quaternion.FromAxisAngle(lookAxis, MathHelper.DegreesToRadians(90f)) * playerCabin.parts[localPosition].localRotation;
                    }
                    if (Inputs.RorateLeft)
                    {
                        playerCabin.parts[localPosition].localRotation = Quaternion.FromAxisAngle(lookAxis, MathHelper.DegreesToRadians(-90f)) * playerCabin.parts[localPosition].localRotation;
                    }
                }
            }
        }
        public enum RightLeft
        {
            right,
            left
        }
        public static void LoadRotate(PlayerCabin playerCabin, Vector3i localPosition, Vector3 lookAxis, RightLeft rightLeft)
        {
            if (playerCabin.parts.ContainsKey(localPosition))
            {
                if (rightLeft == RightLeft.right)
                {
                    playerCabin.parts[localPosition].localRotation = Quaternion.FromAxisAngle(lookAxis, MathHelper.DegreesToRadians(90f)) * playerCabin.parts[localPosition].localRotation;
                }
                if (rightLeft == RightLeft.left)
                {
                    playerCabin.parts[localPosition].localRotation = Quaternion.FromAxisAngle(lookAxis, MathHelper.DegreesToRadians(-90f)) * playerCabin.parts[localPosition].localRotation;
                }
            }
        }
        public static Vector3 LookAxisToLooRotation(Vector3 lookAxis)
        {
            Vector3 lookRotation = Vector3i.Zero;
            if (lookAxis.Y == 1) { lookRotation = Vector3.Zero; }
            else if (lookAxis.Y == -1) { lookRotation.Z = MathHelper.DegreesToRadians(180f); }
            else if (lookAxis.X == 1) { lookRotation.Z = MathHelper.DegreesToRadians(-90f); }
            else if (lookAxis.X == -1) { lookRotation.Z = MathHelper.DegreesToRadians(90f); }
            else if (lookAxis.Z == 1) { lookRotation.X = MathHelper.DegreesToRadians(90f); }
            else if (lookAxis.Z == -1) { lookRotation.X = MathHelper.DegreesToRadians(-90f); }
            return lookRotation;
        }
        public void UpdateUI()
        {
            if (Game.UI.Current is not null)
            {
                ItemInfo itemInfo = Game.UI.GetView<ItemInfo>("ItemInfo");
                if (!playerCabin.parts.ContainsKey(localPosition) && itemInfo != null)
                {
                    itemInfo.empty = true;
                }
                if (playerCabin.parts.ContainsKey(localPosition) && itemInfo != null)
                {
                    itemInfo.empty = false;
                    itemInfo.UpdateValue(ItemInfo.Label.name, playerCabin.parts[localPosition].GetType().Name);
                    itemInfo.UpdateValue(ItemInfo.Label.key, playerCabin.parts[localPosition].key.ToString());
                    itemInfo.UpdateValue(ItemInfo.Label.nKey, playerCabin.parts[localPosition].nkey.ToString());
                    itemInfo.UpdateValue(ItemInfo.Label.toggle, "");
                    itemInfo.UpdateValue(ItemInfo.Label.position, "X:" + localPosition.X + " Y:" + localPosition.Y + " Z:" + localPosition.Z);

                }

            }
        }
        private Vector3i GetAxis(Vector3 vector)
        {
            Vector3i axis = Vector3i.Zero;
            axis.X = 
                Math.Abs(vector.X) >= Math.Abs(vector.X) && 
                Math.Abs(vector.X) >= Math.Abs(vector.Y) && 
                Math.Abs(vector.X) >= Math.Abs(vector.Z) 
                ? (vector.X > 0 ? 1 : -1) : 0;
            axis.Y =
                Math.Abs(vector.Y) >= Math.Abs(vector.X) &&
                Math.Abs(vector.Y) >= Math.Abs(vector.Y) &&
                Math.Abs(vector.Y) >= Math.Abs(vector.Z) 
                ? (vector.Y > 0 ? 1 : -1) : 0;
            axis.Z =
                Math.Abs(vector.Z) >= Math.Abs(vector.X) &&
                Math.Abs(vector.Z) >= Math.Abs(vector.Y) &&
                Math.Abs(vector.Z) >= Math.Abs(vector.Z) 
                ? (vector.Z > 0 ? 1 : -1) : 0;
            return axis;
        }
        private int GetAxis(float len)
        {
            return Math.Abs(Math.Round(len)) > lockAngleSin ? (len > 0f ? 1 : -1) : 0;
        }
        public void Render()
        {
            if (Game.shaderPrograms.Use == "object") renderer.Render();
        }

        public void Dispose()
        {
            renderer.Dispose();
        }
    }
}
