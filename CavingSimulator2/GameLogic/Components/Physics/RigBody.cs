using BepuPhysics;
using CavingSimulator.GameLogic.Components;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CavingSimulator2.GameLogic.Components.Physics
{
    public class RigBody
    {
        //public static Dictionary<Vector3i, StaticBody> colliderBlocks = new Dictionary<Vector3i, StaticBody>();
        public readonly Transform transform;
        public readonly DynamicBody dynamicBody;
        public readonly Vector3i blockDetectionDistance;

        public RigBody(Transform transform,float radius,float mass, Vector3i blockDetectionDistance)
        {
            this.transform = transform;
            this.blockDetectionDistance = blockDetectionDistance;
            dynamicBody = new DynamicBody(transform, radius, mass);
            
        }
        public RigBody(Transform transform, Vector3 size, float mass, Vector3i blockDetectionDistance)
        {
            this.transform = transform;
            this.blockDetectionDistance = blockDetectionDistance;
            dynamicBody = new DynamicBody(transform, size, mass);
        }
        public void Update()
        {
            UpdateBlocks();
            UpdateTransform();
        }
        private void UpdateTransform()
        {
            transform.GlobalPosition = dynamicBody.GetPosition();
            transform.GlobalRotation = dynamicBody.GetRotation();
        }
        private void UpdateBlocks()
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
                            if (ChunkGenerator.chunks[targetChunk].FullBlockExist(new Vector3i(x, y, z)) && !BlocksDir.colliderBlocks.ContainsKey(new Vector3i(x, y, z)))
                            {
                                BlocksDir.colliderBlocks.Add(new Vector3i(x, y, z), new StaticBody(new Transform(new Vector3i(x, y, z)), Vector3.One));
                            }
                        }
                    }
                }
            }
            

        }

        public static Vector3 GoTowards(Vector3 start, Vector3 target, float delta)
        {
            return start + Vector3.Normalize(target - start) * delta;
        }
        public static Vector3 GoTowordsDelta(Vector3 start, Vector3 target, float delta)
        {
            return Vector3.Normalize(target - start) * delta;
        }
    }
}
