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

        public static Queue<Vector3i> creationQueue = new Queue<Vector3i>();
        public static Queue<Vector3i> creationList = new Queue<Vector3i>();

        public static void Update()
        {

        }
    }
}
