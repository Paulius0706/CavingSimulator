using CavingSimulator.GameLogic.Components;
using CavingSimulator2.GameLogic.Objects;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.Components.Physics
{
    public static class BlocksDir
    {
        public static Dictionary<Vector3i, StaticBody> colliderBlocks = new Dictionary<Vector3i, StaticBody>();
        public static Dictionary<Vector3i, (Transform, Vector3)> register = new Dictionary<Vector3i, (Transform, Vector3)>();
        public static void Update()
        {
            /*
            var keys2 = register.Keys;
            foreach (Vector3i key in keys2)
            {
                if (!colliderBlocks.ContainsKey(key))
                {
                    colliderBlocks.Add(key, new StaticBody(register[key].Item1, register[key].Item2));
                }
            }
            var keys1 = colliderBlocks.Keys;
            foreach (Vector3i key in keys1)
            {
                if (!register.ContainsKey(key)) { colliderBlocks[key].Remove(); colliderBlocks.Remove(key); }
            }
            
            register.Clear();
            */
            
            var keys = colliderBlocks.Keys;
            foreach (Vector3i key in keys)
            {
                bool delete = true;
                foreach (BaseObject baseObject in Game.objects.Values)
                {
                    if (baseObject.TryGetRigBody(out RigBody rigBody))
                    {
                        Vector3i blockPos = new Vector3i((int)MathF.Round(rigBody.transform.Position.X), (int)MathF.Round(rigBody.transform.Position.Y), (int)MathF.Round(rigBody.transform.Position.Z));
                        if (Math.Abs(key.X - blockPos.X) <= rigBody.blockDetectionDistance.X + 2 &&
                            Math.Abs(key.Y - blockPos.Y) <= rigBody.blockDetectionDistance.Y + 2 &&
                            Math.Abs(key.Z - blockPos.Z) <= rigBody.blockDetectionDistance.Z + 2)
                        {
                            delete = false;
                            break;
                        }
                    }
                }
                if (delete) { colliderBlocks[key].Remove(); colliderBlocks.Remove(key); }
            }
        }

        public static void Add(Transform transform, Vector3i pos)
        {
            if (!register.ContainsKey(pos)) { register.Add(pos, (transform, Vector3.One)); }
        }
    }
}
