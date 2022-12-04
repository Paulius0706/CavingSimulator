using CavingSimulator2.GameLogic.Components.Physics;
using CavingSimulator2.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.Physics.Shapes
{
    public struct ShapeInfo
    {
        public readonly ShapeType type;
        public readonly Vector3 position;
        public readonly Quaternion rotation;
        public readonly float mass;

        public ShapeInfo(ShapeType shapeType, OpenTK.Mathematics.Vector3 position, OpenTK.Mathematics.Vector3 rotation, float mass)
        {
            this.type = shapeType;
            this.position = Adapter.Convert(position);
            this.rotation = Adapter.Convert(new OpenTK.Mathematics.Quaternion(rotation));
            this.mass = mass;
        }
        public static implicit operator ShapeInfo((ShapeType shapeType, OpenTK.Mathematics.Vector3 position, OpenTK.Mathematics.Vector3 rotation, float mass) info)
        {
            return new ShapeInfo(info.shapeType,info.position,info.rotation,info.mass);
        }
    }
}
