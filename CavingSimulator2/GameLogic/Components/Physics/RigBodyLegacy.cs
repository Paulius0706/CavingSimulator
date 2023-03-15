using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.Constraints;
using BepuUtilities;
using BepuUtilities.Collections;
using BepuUtilities.Memory;
using CavingSimulator.GameLogic.Components;
using CavingSimulator2.Debugger;
using CavingSimulator2.Helpers;
using CavingSimulator2.Physics.Shapes;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CavingSimulator2.GameLogic.Components.Physics
{
    public class RigBodyLegacy : IDisposable
    {
        //public static Dictionary<Vector3i, StaticBody> colliderBlocks = new Dictionary<Vector3i, StaticBody>();
        public readonly Transform transform;
        private DynamicBody dynamicBody;
        public readonly Vector3i blockDetectionDistance;
        private bool disposed;
        private const float angularDrag = 0.5f;

        public RigBodyLegacy(Transform transform,float radius,float mass, Vector3i blockDetectionDistance)
        {
            this.transform = transform;
            this.blockDetectionDistance = blockDetectionDistance;
            dynamicBody = new DynamicBody(transform, radius, mass);
            
        }
        public RigBodyLegacy(Transform transform, Vector3 size, float mass, Vector3i blockDetectionDistance)
        {
            this.transform = transform;
            this.blockDetectionDistance = blockDetectionDistance;
            dynamicBody = new DynamicBody(transform, size, mass);
        }
        public RigBodyLegacy(Transform transform, float mass, Vector3i blockDetectionDistance)
        {
            this.transform = transform;
            this.blockDetectionDistance = blockDetectionDistance;
            dynamicBody = new DynamicBody(transform, mass);
        }
        public void Update()
        {
            UpdateTransform();
            //LinearVelocity += -Vector3.UnitZ * gravity * Game.deltaTime;
            UpdateBlocks();
            
            Drag();
        }
        private void Drag()
        {
            Vector3 angularVelocity = dynamicBody.AngularVelocity;
            float drag = angularDrag * Game.deltaTime;
            angularVelocity.X = angularVelocity.X > 0 ? 
                (angularVelocity.X - drag >= 0 ? angularVelocity.X - drag : 0f) : 
                (angularVelocity.X + drag <= 0 ? angularVelocity.X + drag : 0f);
            angularVelocity.Y = angularVelocity.Y > 0 ?
                (angularVelocity.Y - drag >= 0 ? angularVelocity.Y - drag : 0f) :
                (angularVelocity.Y + drag <= 0 ? angularVelocity.Y + drag : 0f);
            angularVelocity.Z = angularVelocity.Z > 0 ?
                (angularVelocity.Z - drag >= 0 ? angularVelocity.Z - drag : 0f) :
                (angularVelocity.Z + drag <= 0 ? angularVelocity.Z + drag : 0f);
            dynamicBody.AngularVelocity = angularVelocity;

        }

        private void UpdateTransform()
        {
            transform.Position = dynamicBody.Position;
            transform.Rotation = new Quaternion(dynamicBody.Rotation);
        }
        private void UpdateBlocks()
        {
            Vector3i targetChunk = ChunkGenerator.getTargetChunk(transform.Position);
            Vector3i blockPos = new Vector3i((int)MathF.Round(transform.Position.X), (int)MathF.Round(transform.Position.Y), (int)MathF.Round(transform.Position.Z));
            // for blocks
            if (ChunkGenerator.chunks.ContainsKey(targetChunk) && blockDetectionDistance != Vector3i.Zero)
            {
                for (int x = -blockDetectionDistance.X + blockPos.X; x <= blockDetectionDistance.X + blockPos.X; x++)
                {
                    for (int y = -blockDetectionDistance.Y + blockPos.Y; y <= blockDetectionDistance.Y + blockPos.Y; y++)
                    {
                        for (int z = -blockDetectionDistance.Z + blockPos.Z; z <= blockDetectionDistance.Z + blockPos.Z; z++)
                        {
                            if (ChunkGenerator.chunks[targetChunk].FullBlockExist(new Vector3i(x, y, z)) && !BlocksDir.colliderBlocks.ContainsKey(new Vector3i(x, y, z)))
                            {
                                //BlocksDir.Add(new Transform(new Vector3i(x, y, z)), new Vector3i(x, y, z));
                                BlocksDir.colliderBlocks.Add(new Vector3i(x, y, z), new StaticBody(new Transform(new Vector3i(x, y, z)), Vector3.One));
                            }
                        }
                    }
                }
            }
        }
        public void AddForce(Vector3 worldPosition, Vector3 force)
        {
            dynamicBody.AddForce(worldPosition,force);
        }
        public void Weld(Vector3 offset, RigBodyLegacy rigBody, Vector3 size, float mass)
        {
            dynamicBody.Weld(offset, rigBody, size, mass);
        }
        public void UnWeld(Vector3 offset)
        {
            dynamicBody.UnWeld(offset);
        }
        public void StaticWeld(ShapeType shapeType, Vector3 localPosition, Vector3 localRotation, float mass)
        {
            dynamicBody.StaticWeld(shapeType, localPosition, localRotation, mass);
        }
        public void StaticUnweld(Vector3 offset)
        {
            dynamicBody.StaticUnweld(offset);
        }
        public static Vector3 GoTowards(Vector3 start, Vector3 target, float delta)
        {
            return start + Vector3.Normalize(target - start) * delta;
        }
        public static Vector3 GoTowordsDelta(Vector3 start, Vector3 target, float delta)
        {
            return Vector3.Normalize(target - start) * delta;
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            dynamicBody.Dispose();
        }

        public Vector3 LinearVelocity
        {
            get { return dynamicBody.LinearVelocity; }
            set { dynamicBody.LinearVelocity = value; dynamicBody.Awake = true; }
        }
        public Vector3 AngularVelocity
        {
            get { return dynamicBody.AngularVelocity; }
            set { dynamicBody.AngularVelocity = value; dynamicBody.Awake = true; }
        }
        public Vector3 Position
        {
            get { return dynamicBody.Position; }
            set { dynamicBody.Position = value; dynamicBody.Awake = true; }
        }
        public Vector3 Rotation
        {
            get { return dynamicBody.Rotation; }
            set { dynamicBody.Rotation = value; dynamicBody.Awake = true; }
        }


        sealed class DynamicBody : IDisposable
        {
            public readonly Transform transform;
            public BodyHandle bodyHandle;
            Dictionary<System.Numerics.Vector3, (BodyHandle, System.Numerics.Vector3, System.Numerics.Quaternion, ConstraintHandle)> welds = new Dictionary<System.Numerics.Vector3, (BodyHandle, System.Numerics.Vector3, System.Numerics.Quaternion, ConstraintHandle)>();

            Dictionary<System.Numerics.Vector3, (Box, RigidPose, float)> boxes = new Dictionary<System.Numerics.Vector3, (Box, RigidPose, float)>();
            Dictionary<System.Numerics.Vector3, (Sphere, RigidPose, float)> spheres = new Dictionary<System.Numerics.Vector3, (Sphere, RigidPose, float)>();
            Dictionary<System.Numerics.Vector3, (Slope, RigidPose, float)> slopes = new Dictionary<System.Numerics.Vector3, (Slope, RigidPose, float)>();
            Dictionary<System.Numerics.Vector3, (Cylinder, RigidPose, float)> cylinders = new Dictionary<System.Numerics.Vector3, (Cylinder, RigidPose, float)>();

            private const int maxPartLimit = 50;
            public TypedIndex compoundHandle;

            private bool disposed;
            

            public DynamicBody(Transform transform, float radius, float mass)
            {
                this.transform = transform;
                if (!ShapesDir.sphereShapes.ContainsKey(radius)) ShapesDir.sphereShapes.Add(radius, Game.physicsSpace.Shapes.Add(new Sphere(radius)));
                BodyInertia bodyInertia = new Sphere(radius).ComputeInertia(mass);
                
                this.bodyHandle = Game.physicsSpace.Bodies.Add(
                        BodyDescription.CreateDynamic(
                            new RigidPose(Adapter.Convert(transform.Position), Adapter.Convert(transform.Rotation)),
                            bodyInertia,
                            new CollidableDescription( ShapesDir.sphereShapes[radius], radius),
                            new BodyActivityDescription(0.01f)
                        )
                    );
                Awake = true;
            }
            public DynamicBody(Transform transform, Vector3 size, float mass)
            {
                this.transform = transform;
                if (!ShapesDir.boxShapes.ContainsKey(size)) ShapesDir.boxShapes.Add(size, Game.physicsSpace.Shapes.Add(new Box(size.X, size.Y, size.Z)));
                BodyInertia bodyInertia = new Box(size.X, size.Y, size.Z).ComputeInertia(mass);
                
                this.bodyHandle = Game.physicsSpace.Bodies.Add(
                        BodyDescription.CreateDynamic(
                        new RigidPose(Adapter.Convert(this.transform.Position), Adapter.Convert(this.transform.Rotation)),
                        bodyInertia,
                        new CollidableDescription(ShapesDir.boxShapes[size]),
                        new BodyActivityDescription(0.01f)));

                Awake = true;
            }
            public DynamicBody(Transform transform, float mass)
            {
                this.transform = transform;
                CompoundBuilder compoundBuilder = new CompoundBuilder(Game.bufferPool, Game.physicsSpace.Shapes, maxPartLimit);

                boxes.Add(System.Numerics.Vector3.Zero, (new Box(1, 1, 1), System.Numerics.Vector3.Zero, mass));
                compoundBuilder.Add(boxes[System.Numerics.Vector3.Zero].Item1, boxes[System.Numerics.Vector3.Zero].Item2, boxes[System.Numerics.Vector3.Zero].Item3);
                compoundBuilder.BuildDynamicCompound(out Buffer<CompoundChild> children, out BodyInertia bodyInertia, out System.Numerics.Vector3 center);
                compoundBuilder.Reset();
                compoundBuilder.Dispose();
                //Console.WriteLine(children.Length + " " + bodyInertia.InverseMass + " " + center);
                this.compoundHandle = Game.physicsSpace.Shapes.Add(new Compound(children));
                this.bodyHandle = Game.physicsSpace.Bodies.Add(
                        BodyDescription.CreateDynamic(
                        new RigidPose(Adapter.Convert(this.transform.Position) + center, Adapter.Convert(this.transform.Rotation)),
                        bodyInertia,
                        new CollidableDescription(this.compoundHandle),
                        new BodyActivityDescription(0.01f)));
                Awake = true;
            }
            public void AddForce(Vector3 worldPosition, Vector3 force)
            {
                Game.physicsSpace.Bodies[bodyHandle].ApplyImpulse(Adapter.Convert(force), Adapter.Convert(worldPosition));
                Awake = true;
            }
            public void Weld(Vector3 offset, RigBodyLegacy rigBody, Vector3 size, float mass)
            {
                if (!ShapesDir.boxShapes.ContainsKey(size)) ShapesDir.boxShapes.Add(size, Game.physicsSpace.Shapes.Add(new Box(size.X, size.Y, size.Z)));
                BodyInertia bodyInertia = new Box(size.X, size.Y, size.Z).ComputeInertia(1f);

                welds.Add(Adapter.Convert(offset),
                    (rigBody.dynamicBody.bodyHandle,
                    Adapter.Convert(offset),
                    Adapter.Convert(transform.Rotation),
                    Game.physicsSpace.Solver.Add(this.bodyHandle, rigBody.dynamicBody.bodyHandle, new Weld()
                        {
                            LocalOffset = Adapter.Convert(offset),
                            LocalOrientation = Adapter.Convert(transform.Rotation),
                            SpringSettings = new SpringSettings(10f, 1f)
                        })
                    )
                    );

            }
            public void UnWeld(Vector3 offset)
            {
                Game.physicsSpace.Solver.Remove(welds[Adapter.Convert(offset)].Item4);
                welds.Remove(Adapter.Convert(offset));
            }
            
            
            public void StaticWeld(ShapeType shapeType,Vector3 localPosition,Vector3 localRotation, float mass)
            {
                if (boxes.ContainsKey(Adapter.Convert(localPosition))) { Debug.WriteLine("is not empty"); return; }
                if (spheres.ContainsKey(Adapter.Convert(localPosition))) { Debug.WriteLine("is not empty"); return; }
                if (slopes.ContainsKey(Adapter.Convert(localPosition))) { Debug.WriteLine("is not empty"); return; }
                if (cylinders.ContainsKey(Adapter.Convert(localPosition))) { Debug.WriteLine("is not empty"); return; }
                switch (shapeType)
                {
                    case ShapeType.box:      boxes    .Add(Adapter.Convert(localPosition), (new Box(1f,1f,1f)    , new RigidPose(Adapter.Convert(localPosition), Adapter.Convert(new Quaternion(localRotation))), mass)); break; // There is a problem
                    case ShapeType.sphere:   spheres  .Add(Adapter.Convert(localPosition), (new Sphere(0.5f)     , new RigidPose(Adapter.Convert(localPosition), Adapter.Convert(new Quaternion(localRotation))), mass)); break; 
                    case ShapeType.slope:    slopes   .Add(Adapter.Convert(localPosition), (new Slope(1f, 1f, 1f), new RigidPose(Adapter.Convert(localPosition), Adapter.Convert(new Quaternion(localRotation))), mass)); break;
                    case ShapeType.cylinder: cylinders.Add(Adapter.Convert(localPosition), (new Cylinder(0.5f,1f), new RigidPose(Adapter.Convert(localPosition), Adapter.Convert(new Quaternion(localRotation))), mass)); break;
                }
                UpdateComposite();
            }


            public void StaticUnweld(Vector3 localPosition)
            {
                if (boxes.ContainsKey(Adapter.Convert(localPosition)))     { boxes.    Remove(Adapter.Convert(localPosition)); return; }
                if (spheres.ContainsKey(Adapter.Convert(localPosition)))   { spheres.  Remove(Adapter.Convert(localPosition)); return; }
                if (slopes.ContainsKey(Adapter.Convert(localPosition)))    { slopes.   Remove(Adapter.Convert(localPosition)); return; }
                if (cylinders.ContainsKey(Adapter.Convert(localPosition))) { cylinders.Remove(Adapter.Convert(localPosition)); return; }

                UpdateComposite();
            }

            

            private void UpdateComposite()
            {
                System.Numerics.Vector3 position = Game.physicsSpace.Bodies[bodyHandle].Pose.Position;
                System.Numerics.Quaternion rotation = Game.physicsSpace.Bodies[bodyHandle].Pose.Orientation;
                System.Numerics.Vector3 velocity = Game.physicsSpace.Bodies[bodyHandle].Velocity.Linear;
                System.Numerics.Vector3 angularVelocity = Game.physicsSpace.Bodies[bodyHandle].Velocity.Angular;

                Game.physicsSpace.Bodies.Remove(bodyHandle);
                Game.physicsSpace.Shapes.RemoveAndDispose(compoundHandle, Game.bufferPool);
                CompoundBuilder compoundBuilder = new CompoundBuilder(Game.bufferPool, Game.physicsSpace.Shapes, maxPartLimit);
                foreach (var shape in boxes.Values)     { compoundBuilder.Add(shape.Item1, shape.Item2, shape.Item3); }
                foreach (var shape in spheres.Values)   { compoundBuilder.Add(shape.Item1, shape.Item2, shape.Item3); }
                foreach (var shape in slopes.Values)    { compoundBuilder.Add(shape.Item1, shape.Item2, shape.Item3); }
                foreach (var shape in cylinders.Values) { compoundBuilder.Add(shape.Item1, shape.Item2, shape.Item3); }
                compoundBuilder.BuildDynamicCompound(out Buffer<CompoundChild> children, out BodyInertia bodyInertia);//, out System.Numerics.Vector3 center);
                compoundBuilder.Reset();
                compoundBuilder.Dispose();
                
                this.compoundHandle = Game.physicsSpace.Shapes.Add(new Compound(children));
                
                this.bodyHandle = Game.physicsSpace.Bodies.Add(
                        BodyDescription.CreateDynamic(
                        new RigidPose(position),
                        bodyInertia,
                        new CollidableDescription(this.compoundHandle),
                        new BodyActivityDescription(0.01f)));
                Game.physicsSpace.Bodies[bodyHandle].Pose.Orientation = rotation;
                Game.physicsSpace.Bodies[bodyHandle].Velocity.Linear = velocity;
                Game.physicsSpace.Bodies[bodyHandle].Velocity.Angular = angularVelocity;
                WeldRigidBodies();
                Awake = true;
            }

            private void WeldRigidBodies()
            {
                foreach(var constrain in welds.Values)
                {
                    Game.physicsSpace.Solver.Add(this.bodyHandle, constrain.Item1, new Weld()
                    {
                        LocalOffset = constrain.Item2,
                        LocalOrientation = constrain.Item3,
                        SpringSettings = new SpringSettings(10f, 1f)
                    });
                }
            }

            public void Dispose()
            {
                if (disposed) return;
                disposed = true;
                
                Game.physicsSpace.Bodies.Remove(bodyHandle);              
            }

            public Vector3 LinearVelocity
            {
                get { return Adapter.Convert(Game.physicsSpace.Bodies[bodyHandle].Velocity.Linear); }
                set { Game.physicsSpace.Bodies[bodyHandle].Velocity.Linear = Adapter.Convert(value); Awake = true; }
            }
            public Vector3 AngularVelocity
            {
                get { return Adapter.Convert(Game.physicsSpace.Bodies[bodyHandle].Velocity.Angular); }
                set { Game.physicsSpace.Bodies[bodyHandle].Velocity.Angular = Adapter.Convert(value); Awake = true; }
            }
            public Vector3 Position
            {
                get { return Adapter.Convert(Game.physicsSpace.Bodies[bodyHandle].Pose.Position); }
                set { Game.physicsSpace.Bodies[bodyHandle].Pose.Position = Adapter.Convert(value); Awake = true; }
            }
            public Vector3 Rotation
            {
                get { return Adapter.Convert(Game.physicsSpace.Bodies[bodyHandle].Pose.Orientation).ToEulerAngles(); }
                set { Game.physicsSpace.Bodies[bodyHandle].Pose.Orientation = Adapter.Convert(new Quaternion(value)); Awake = true; }
            }
            
            public bool Awake
            {
                get 
                {
                    return Game.physicsSpace.Bodies[bodyHandle].Awake; 
                }
                set 
                {
                    BodyReference bodyReference1 = Game.physicsSpace.Bodies[bodyHandle];
                    bodyReference1.Awake = value; 
                
                }
            }
        }
    }
}
