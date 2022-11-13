using CavingSimulator2.GameLogic.Objects;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CavingSimulator2.GameLogic.Components.Colliders.ColliderStrategy;
using CavingSimulator.GameLogic.Components;

namespace CavingSimulator2.GameLogic.Components.Colliders
{
    public class Collider
    {
        public readonly Transform transform;
        public RigBody rigBody;
        private Vector2 _X;
        private Vector2 _Y;
        private Vector2 _Z;
        private Vector3 _offset;
        private Vector3i blockDetectionDistance;
        private float _R;
        public IColliderStrategy colliderStrategy;
        public Collider(Transform transform, Vector2 x, Vector2 y, Vector2 z, Vector3 offset, Vector3i blockDetectionDistance)
        {
            this.blockDetectionDistance = blockDetectionDistance;
            this.transform = transform;
            this._X = x;
            this._Y = y;
            this._Z = z;
            this._offset = offset;
            this.colliderStrategy = new BoxColliderStrategy(this);

        }
        public Collider(Vector2 x, Vector2 y, Vector2 z)
        {
            this._X = x;
            this._Y = y;
            this._Z = z;
            this._offset = Vector3.Zero;
            this.colliderStrategy = new BoxColliderStrategy(this);
        }
        public Collider(float r, Vector3 offset)
        {
            this._R = r;
            this._offset = offset;
            this.colliderStrategy = new CircleColliderStrategy(this);
        }
        public Collider(float r)
        {
            this._R = r;
            this.colliderStrategy = new CircleColliderStrategy(this);
        }
        public void CheckCollisions()
        {

            Vector3i targetChunk = ChunkGenerator.getTargetChunk(transform.GlobalPosition);
            Vector3i blockPos = new Vector3i((int)MathF.Round(transform.GlobalPosition.X), (int)MathF.Round(transform.GlobalPosition.Y), (int)MathF.Round(transform.GlobalPosition.Z));
            // for blocks
            if (ChunkGenerator.chunks.ContainsKey(targetChunk))
            {
                for (int x = -blockDetectionDistance.X + blockPos.X; x <= blockDetectionDistance.X + blockPos.X; x++)
                {
                    for (int y = -blockDetectionDistance.Y + blockPos.Y; y <= blockDetectionDistance.Y + blockPos.Y; y++)
                    {
                        for (int z = -blockDetectionDistance.Z + blockPos.Z; z <= blockDetectionDistance.Z + blockPos.Z; z++)
                        {
                            if (ChunkGenerator.chunks[targetChunk].FullBlockExist(new Vector3i(x, y, z)))
                            colliderStrategy.Execute(new Vector3i(x, y, z));
                        }

                    }
                }
            }
            

            // for enities


            // old
            //foreach(GameObject gameObject in Game.gameObjects.Values)
            //{
            //    if(gameObject != this.gameObject)
            //    colliderStrategy.Execute(gameObject);
            //}
        }
        public Vector2 X
        {
            get
            {
                return new Vector2(
                _X.X * transform.GlobalScale.X + transform.GlobalPosition.X + _offset.X,
                    _X.Y * transform.GlobalScale.X + transform.GlobalPosition.X + _offset.X);
            }
        }
        public Vector2 Y
        {
            get
            {
                return new Vector2(
                _Y.X * transform.GlobalScale.Y + transform.GlobalPosition.Y + _offset.Y,
                    _Y.Y * transform.GlobalScale.Y + transform.GlobalPosition.Y + _offset.Y);
            }
        }
        public Vector2 Z
        {
            get
            {
                return new Vector2(
                _Z.X * transform.GlobalScale.Z + transform.GlobalPosition.Z + _offset.Z,
                    _Z.Y * transform.GlobalScale.Z + transform.GlobalPosition.Z + _offset.Z);
            }
        }
    }
}
