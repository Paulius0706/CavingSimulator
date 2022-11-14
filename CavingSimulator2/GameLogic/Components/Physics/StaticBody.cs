using BepuPhysics;
using BepuPhysics.Collidables;
using CavingSimulator.GameLogic.Components;
using CavingSimulator2.Helpers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.Components.Physics
{
    public class StaticBody
    {
        
        public readonly Transform transform;
        public readonly BodyHandle bodyHandle;

        private ShapeType shapeType;

        public StaticBody(Transform transform, float radius)
        {
            shapeType = ShapeType.sphere;
            this.transform = transform;
            if (!ShapesDir.sphereShapes.ContainsKey(radius)) ShapesDir.sphereShapes.Add(radius, Game.physicsSpace.Shapes.Add(new Sphere(radius)));

            Game.physicsSpace.Statics.Add(new StaticDescription(Adapter.Convert(transform.GlobalPosition), ShapesDir.sphereShapes[radius]));
        }
        public StaticBody(Transform transform, Vector3 size)
        {
            shapeType = ShapeType.box;
            this.transform = transform;
            if (!ShapesDir.boxShapes.ContainsKey(size)) ShapesDir.boxShapes.Add(size, Game.physicsSpace.Shapes.Add(new Box(size.X,size.Y,size.Z)));

            Game.physicsSpace.Statics.Add(new StaticDescription(Adapter.Convert(transform.GlobalPosition), ShapesDir.boxShapes[size]));
        }
        public Vector3 GetPosition()
        {
            return Adapter.Convert(Game.physicsSpace.Bodies[bodyHandle].Pose.Position);
        }
        public Vector3 GetRotation()
        {
            return Adapter.Convert(Game.physicsSpace.Bodies[bodyHandle].Pose.Orientation).ToEulerAngles();
        }

    }
}
