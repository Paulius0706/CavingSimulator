using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities.Memory;
using CavingSimulator.GameLogic.Components;
using CavingSimulator2.Debugger;
using CavingSimulator2.Helpers;
using CavingSimulator2.Physics.Shapes;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.Components.Physics
{
    public class RigBody
    {
        public readonly Transform transform;
        public Vector3 centerOffset;
        public Vector3 worldCenterOffset;
        private BodyHandle bodyHandle;
        private BodyReference bodyReference;
        private TypedIndex shapeHandle;
        public Vector3i blockDetectionDistance;
        private bool disposed;
        private const float angularDrag = 0.5f;
        private const float linearDrag = 0.1f;
        private const int PartsLimit = 50;
        private Dictionary<Vector3, ShapeInfo> shapesInfo = new Dictionary<Vector3, ShapeInfo>();
        private Dictionary<Vector3, float> shapesMasses = new Dictionary<Vector3, float>();

        public RigBody(Transform transform, float mass, Vector3i blockDetectionDistance)
        {
            this.transform = transform;
            this.blockDetectionDistance = blockDetectionDistance;

            shapesInfo.Add(Vector3.Zero, (ShapeType.box, Vector3.Zero, Vector3.Zero, mass));
            CompoundBuilder compoundBuilder = new CompoundBuilder(Game.bufferPool, Game.physicsSpace.Shapes, PartsLimit);
            compoundBuilder.Add(new Box(1, 1, 1), System.Numerics.Vector3.Zero, mass);
            compoundBuilder.BuildDynamicCompound(out Buffer<CompoundChild> children, out BodyInertia bodyInertia, out System.Numerics.Vector3 center);

            this.shapeHandle = Game.physicsSpace.Shapes.Add(new BigCompound(children,Game.physicsSpace.Shapes,Game.bufferPool));

            bodyHandle = Game.physicsSpace.Bodies.Add(BodyDescription.CreateDynamic(
                new RigidPose(Adapter.Convert(this.transform.Position) + center, Adapter.Convert(this.transform.Rotation)),
                bodyInertia,
                new CollidableDescription(this.shapeHandle),
                new BodyActivityDescription(0.01f))
                );
            bodyReference = Game.physicsSpace.Bodies[this.bodyHandle];

            compoundBuilder.Dispose();
        }
        public void Update()
        {
            UpdateTransform();
            UpdateBlocks();

            Drag();
        }
        private void Drag()
        {
            Vector3 angularVelocity = AngularVelocity;
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
            AngularVelocity = angularVelocity;

            LinearVelocity -= LinearVelocity * linearDrag * Game.deltaTime;
        }

        private void UpdateTransform()
        {
            transform.Rotation = Rotation;
            worldCenterOffset = new Vector3(new Vector4(centerOffset) * Matrix4.CreateFromQuaternion(this.transform.Rotation));
            transform.Position = Position - worldCenterOffset;
            
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

            Game.physicsSpace.Bodies[bodyHandle].ApplyImpulse(Adapter.Convert(force), Adapter.Convert(worldPosition - worldCenterOffset));
            bodyReference.Awake = true;
        }
        public void AddAngularVelocity(Vector3 force)
        {

            Game.physicsSpace.Bodies[bodyHandle].ApplyAngularImpulse(Adapter.Convert(force));
            bodyReference.Awake = true;
        }
        public void Weld(Vector3 offset, RigBody rigBody, Vector3 size, float mass)
        {

        }
        public void UnWeld(Vector3 offset)
        {
            
        }
        public void StaticWeld(ShapeType shapeType, Vector3 localPosition, Quaternion localRotation, float mass)
        {
            // adds new shape to selected list
            if (shapesInfo.ContainsKey(localPosition)) return;
            shapesInfo.Add(localPosition, (shapeType, localPosition, localRotation.ToEulerAngles(), mass));
            blockDetectionDistance = (Vector3i)(Vector3.One * (shapesInfo.Keys.Max(o => o.Length) + 2f));
            UpdateShape();
            
        }
        public void StaticUnweld(Vector3 offset)
        {
            // adds new shape to selected list
            if (!shapesInfo.ContainsKey(offset)) return;
            shapesInfo.Remove(offset);
            UpdateShape();

        }
        private void UpdateShape()
        {
            CompoundBuilder compoundBuilder = new CompoundBuilder(Game.bufferPool,Game.physicsSpace.Shapes,PartsLimit);
            foreach (ShapeInfo info in this.shapesInfo.Values)
            {
                switch (info.type)
                {
                    case ShapeType.box: compoundBuilder.Add(new Box(1f, 1f, 1f), new RigidPose(info.position/*, info.rotation*/), info.mass); break;
                    case ShapeType.sphere: compoundBuilder.Add(new Sphere(0.5f), new RigidPose(info.position, info.rotation), info.mass); break;
                    case ShapeType.cylinder: compoundBuilder.Add(new Cylinder(0.5f, 1f), new RigidPose(info.position, info.rotation), info.mass); break;
                    case ShapeType.slope: compoundBuilder.Add(new Slope(1f, 1f, 1f), new RigidPose(info.position, info.rotation), info.mass); break;
                }
            }
            compoundBuilder.BuildDynamicCompound(out Buffer<CompoundChild> children, out BodyInertia bodyInertia, out System.Numerics.Vector3 center);
            
            TypedIndex newShapeHandle = Game.physicsSpace.Shapes.Add(new BigCompound(children, Game.physicsSpace.Shapes, Game.bufferPool));

            System.Numerics.Vector3 position = bodyReference.Pose.Position;
            System.Numerics.Quaternion orentation = bodyReference.Pose.Orientation;

            bodyReference.ApplyDescription(
                BodyDescription.CreateDynamic(
                    new RigidPose(),
                    bodyInertia,
                    new CollidableDescription(newShapeHandle),
                    new BodyActivityDescription(0.01f))
                );
            bodyReference.Pose.Position = position;
            bodyReference.Pose.Orientation = orentation;
            centerOffset = Adapter.Convert(center);




            Game.physicsSpace.Shapes.RecursivelyRemoveAndDispose(this.shapeHandle, Game.bufferPool);
            this.shapeHandle = newShapeHandle;
            compoundBuilder.Dispose();
        }
        

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            Game.physicsSpace.Bodies.Remove(bodyReference.Handle);
            Game.physicsSpace.Shapes.RemoveAndDispose(shapeHandle, Game.bufferPool);
            
        }

        public Vector3 LinearVelocity
        {
            get { return Adapter.Convert(bodyReference.Velocity.Linear); }
            set { bodyReference.Velocity.Linear = Adapter.Convert(value); bodyReference.Awake = true; }
        }
        public Vector3 AngularVelocity
        {
            get { return Adapter.Convert(bodyReference.Velocity.Angular) ; }
            set { bodyReference.Velocity.Angular = Adapter.Convert(value); bodyReference.Awake = true; }
        }
        public Vector3 Position
        {
            get { return Adapter.Convert(bodyReference.Pose.Position); }
            set { bodyReference.Pose.Position = Adapter.Convert( value); bodyReference.Awake = true; }
        }
        public Quaternion Rotation
        {
            get { return Adapter.Convert(bodyReference.Pose.Orientation); }
            set { bodyReference.Pose.Orientation = Adapter.Convert(value); bodyReference.Awake = true; }
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
