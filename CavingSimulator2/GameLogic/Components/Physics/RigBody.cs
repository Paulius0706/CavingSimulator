using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.Constraints;
using CavingSimulator.GameLogic.Components;
using CavingSimulator2.Helpers;
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
    public class RigBody : IDisposable
    {
        //public static Dictionary<Vector3i, StaticBody> colliderBlocks = new Dictionary<Vector3i, StaticBody>();
        public readonly Transform transform;
        private DynamicBody dynamicBody;
        public readonly Vector3i blockDetectionDistance;
        private bool disposed;
        public const float gravity = 10f;

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
            //LinearVelocity += -Vector3.UnitZ * gravity * Game.deltaTime;
            UpdateBlocks();
            UpdateTransform();
        }
        private void UpdateTransform()
        {
            transform.Position = dynamicBody.Position;
            transform.Rotation = dynamicBody.Rotation;
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
        public void Weld(Vector3 offset, RigBody rigBody, Vector3 size, float mass)
        {
            dynamicBody.Weld(offset, rigBody, size, mass);
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
            public readonly BodyHandle bodyHandle;
            private BodyReference bodyReference;

            private ShapeType shapeType;
            private bool disposed;


            public DynamicBody(Transform transform, float radius, float mass)
            {
                shapeType = ShapeType.sphere;
                this.transform = transform;
                if (!ShapesDir.sphereShapes.ContainsKey(radius)) ShapesDir.sphereShapes.Add(radius, Game.physicsSpace.Shapes.Add(new Sphere(radius)));
                BodyInertia bodyInertia = new Sphere(radius).ComputeInertia(1f);
                
                this.bodyHandle = Game.physicsSpace.Bodies.Add(
                        BodyDescription.CreateDynamic(
                            new RigidPose(Adapter.Convert(transform.Position), Adapter.Convert(new Quaternion(transform.Rotation))),
                            bodyInertia,
                            new CollidableDescription( ShapesDir.sphereShapes[radius], radius),
                            new BodyActivityDescription(0.01f)
                        )
                    );
                Awake = true;
            }
            public DynamicBody(Transform transform, Vector3 size, float mass)
            {
                shapeType = ShapeType.box;
                this.transform = transform;
                if (!ShapesDir.boxShapes.ContainsKey(size)) ShapesDir.boxShapes.Add(size, Game.physicsSpace.Shapes.Add(new Box(size.X, size.Y, size.Z)));
                BodyInertia bodyInertia = new Box(size.X, size.Y, size.Z).ComputeInertia(1f);
                Console.WriteLine("rot:" +this.transform.Rotation);
                Console.WriteLine("pos:" + this.transform.Position);
                this.bodyHandle = Game.physicsSpace.Bodies.Add(
                        BodyDescription.CreateDynamic(
                        new RigidPose(Adapter.Convert(this.transform.Position), Adapter.Convert(new Quaternion(this.transform.Rotation))),
                        bodyInertia,
                        new CollidableDescription(ShapesDir.boxShapes[size],size.Length + 0.1f),
                        new BodyActivityDescription(0.01f)));

                Awake = true;
                
            }
            public ref BodyReference GetBodyReference() { return ref this.bodyReference; }

            public int Weld(Vector3 offset, RigBody rigBody, Vector3 size, float mass)
            {
                if (!ShapesDir.boxShapes.ContainsKey(size)) ShapesDir.boxShapes.Add(size, Game.physicsSpace.Shapes.Add(new Box(size.X, size.Y, size.Z)));
                BodyInertia bodyInertia = new Box(size.X, size.Y, size.Z).ComputeInertia(1f);

                Game.physicsSpace.Solver.Add(this.bodyHandle, rigBody.dynamicBody.bodyHandle, new Weld()
                {
                    LocalOffset = Adapter.Convert(offset),
                    LocalOrientation = Adapter.Convert(new Quaternion(transform.Rotation)),
                    SpringSettings = new SpringSettings(10f, 1f)
                });

                return bodyHandle.Value;
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
