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
    public class StaticBody : IDisposable
    {
        
        public readonly Transform transform;
        private StaticHandle staticHandle;
        private bool disposed = false;

        private ShapeType shapeType;

        public StaticBody(Transform transform, float radius)
        {
            shapeType = ShapeType.sphere;
            this.transform = transform;
            if (!ShapesDir.sphereShapes.ContainsKey(radius)) ShapesDir.sphereShapes.Add(radius, Game.physicsSpace.Shapes.Add(new Sphere(radius)));

            this.staticHandle = Game.physicsSpace.Statics.Add(new StaticDescription(Adapter.Convert(transform.Position), ShapesDir.sphereShapes[radius]));
        }
        public StaticBody(Transform transform, Vector3 size)
        {
            shapeType = ShapeType.box;
            this.transform = transform;

            if (!ShapesDir.boxShapes.ContainsKey(size)) ShapesDir.boxShapes.Add(size, Game.physicsSpace.Shapes.Add(new Box(size.X,size.Y,size.Z)));
            this.staticHandle =  Game.physicsSpace.Statics.Add(new StaticDescription(Adapter.Convert(transform.Position), ShapesDir.boxShapes[size]));

            //this.staticHandle = Game.physicsSpace.Statics.Add(new StaticDescription(Adapter.Convert(transform.Position), Game.physicsSpace.Shapes.Add(new Box(size.X, size.Y, size.Z))));

        }
        ~StaticBody()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;
            Game.physicsSpace.Statics.Remove(staticHandle);
            
        }

        public Vector3 Position
        {
            get { return Adapter.Convert(Game.physicsSpace.Statics.GetStaticReference(this.staticHandle).Pose.Position); }
            set { Game.physicsSpace.Statics.GetStaticReference(this.staticHandle).Pose.Position = Adapter.Convert(value); }
        }
        public Vector3 Rotation
        {
            get { return Adapter.Convert(Game.physicsSpace.Statics.GetStaticReference(this.staticHandle).Pose.Orientation).ToEulerAngles(); }
            set { Game.physicsSpace.Statics.GetStaticReference(this.staticHandle).Pose.Orientation = Adapter.Convert(new Quaternion(value)); }
        }
    }
}
