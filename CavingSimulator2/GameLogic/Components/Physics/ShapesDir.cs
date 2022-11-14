using BepuPhysics.Collidables;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.Components.Physics
{
    public class ShapesDir
    {
        public static Dictionary<Vector3, TypedIndex> boxShapes = new Dictionary<Vector3, TypedIndex>();
        public static Dictionary<float, TypedIndex> sphereShapes = new Dictionary<float, TypedIndex>();
    }
}
