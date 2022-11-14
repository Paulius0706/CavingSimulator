using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepuPhysics;
using BepuPhysics.Collidables;
using CavingSimulator.GameLogic.Components;
using CavingSimulator2.Helpers;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using static System.Formats.Asn1.AsnWriter;

namespace CavingSimulator2.GameLogic.Components.Physics
{
    public class DynamicBody
    {
        public readonly Transform transform;
        public readonly BodyHandle bodyHandle;
        private BodyReference bodyReference;

        private ShapeType shapeType;
        

        public DynamicBody(Transform transform,float radius, float mass)
        {
            shapeType = ShapeType.sphere;
            this.transform = transform;
            if (!ShapesDir.sphereShapes.ContainsKey(radius)) ShapesDir.sphereShapes.Add(radius, Game.physicsSpace.Shapes.Add(new Sphere(radius)));
            BodyInertia bodyInertia = new Sphere(radius).ComputeInertia(1);

            this.bodyReference = Game.physicsSpace.Bodies[
                Game.physicsSpace.Bodies.Add(
                    BodyDescription.CreateDynamic(
                    (Adapter.Convert(transform.GlobalPosition), Adapter.Convert(new Quaternion(transform.GlobalRotation))),
                    bodyInertia,
                    ShapesDir.sphereShapes[radius],
                    0.01f))
                ];
            this.bodyHandle = bodyReference.Handle;
        }
        public DynamicBody(Transform transform, Vector3 size, float mass)
        {
            shapeType = ShapeType.box;
            this.transform = transform;
            if (!ShapesDir.boxShapes.ContainsKey(size)) ShapesDir.boxShapes.Add(size, Game.physicsSpace.Shapes.Add(new Box(size.X, size.Y, size.Z)));
            BodyInertia bodyInertia = new Box(size.X, size.Y, size.Z).ComputeInertia(mass);

            this.bodyReference = Game.physicsSpace.Bodies[
                Game.physicsSpace.Bodies.Add(
                    BodyDescription.CreateDynamic(
                    (Adapter.Convert(transform.GlobalPosition), Adapter.Convert(new Quaternion(transform.GlobalRotation))),
                    bodyInertia,
                    ShapesDir.boxShapes[size],
                    0.01f))
                ];
            this.bodyHandle = bodyReference.Handle;
        }
        public Vector3 GetVelocity()  { return Adapter.Convert(Game.physicsSpace.Bodies[bodyHandle].Velocity.Linear); }
        public Vector3 GetAVelocity() { return Adapter.Convert(Game.physicsSpace.Bodies[bodyHandle].Velocity.Angular); }
        public void AddVelocity(Vector3 velocity) { bodyReference.Velocity.Linear += Adapter.Convert(velocity); bodyReference.Awake = true; }
        public void AddAVelocity(Vector3 velocity) { bodyReference.Velocity.Linear += Adapter.Convert(velocity); bodyReference.Awake = true; }
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
