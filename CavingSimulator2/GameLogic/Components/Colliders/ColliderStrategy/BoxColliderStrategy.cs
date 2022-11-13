using CavingSimulator.GameLogic.Components;
using CavingSimulator2.GameLogic.Objects;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.Components.Colliders.ColliderStrategy
{
    public class BoxColliderStrategy : IColliderStrategy
    {
        public readonly Collider collider;

        public BoxColliderStrategy(Collider collider)
        {
            this.collider = collider;
        }

        public bool DetectCollision(Vector3i blockPosition)
        {
            //Console.WriteLine(collider.gameObject.Transform.GlobalPosition + "\n" + go.Transform.GlobalPosition);
            Collider col = ChunkGenerator.Chunk.Block.ConstructBlockCollider(blockPosition);
            //if (collider.Z.X > col.Z.Y ) return false;
            //return true;
            ////if (col1.X.X > col2.X.Y 
            ////    || col2.X.X > col1.X.Y 
            ////    || col1.Y.X > col2.Y.Y
            ////    || col2.Y.X > col1.Y.Y
            ////    || col1.Z.X > col2.Z.Y
            ////    || col2.Z.X > col1.Z.Y
            ////    ) return false;
            ////return true;
            return
                collider.X.Y > col.X.X && // col1 max is more then col2 min
                collider.X.X < col.X.Y && // col1 min is less then col2 max
                collider.Y.Y > col.Y.X && // col1 max is more then col2 min
                collider.Y.X < col.Y.Y && // col1 min is less then col2 max
                collider.Z.Y > col.Z.X && // col1 max is more then col2 min
                collider.Z.X < col.Z.Y;   // col1 min is less then col2 max
        }

        public void Execute(Vector3i blockPosition)
        {
            if (DetectCollision(blockPosition))
            {
                RespondToCollision(blockPosition);
                collider.transform.baseObject.CollisionTrigger(blockPosition);
            }
        }
        

        public void RespondToCollision(Vector3i blockPosition)
        {
            float x = 0f;
            float y = 0f;
            float z = 0f;
            Collider additionalCollider = ChunkGenerator.Chunk.Block.ConstructBlockCollider(blockPosition);
            RigBody rigBody = collider.rigBody;
            if (!(additionalCollider.X.X < collider.X.X && collider.X.Y < additionalCollider.X.Y) &&
                !(collider.X.X < additionalCollider.X.X && additionalCollider.X.Y < collider.X.Y))
            {
                if (additionalCollider.X.X < collider.X.X && additionalCollider.X.Y < collider.X.Y) x = additionalCollider.X.Y - collider.X.X;
                if (additionalCollider.X.X > collider.X.X && additionalCollider.X.Y > collider.X.Y) x = additionalCollider.X.X - collider.X.Y;
            }
            if (!(additionalCollider.Y.X < collider.Y.X && collider.Y.Y < additionalCollider.Y.Y) &&
                !(collider.Y.X < additionalCollider.Y.X && additionalCollider.Y.Y < collider.Y.Y))
            {
                if (additionalCollider.Y.X < collider.Y.X && additionalCollider.Y.Y < collider.Y.Y) y = additionalCollider.Y.Y - collider.Y.X;
                if (additionalCollider.Y.X > collider.Y.X && additionalCollider.Y.Y > collider.Y.Y) y = additionalCollider.Y.X - collider.Y.Y;
            }
            if (!(additionalCollider.Z.X < collider.Z.X && collider.Z.Y < additionalCollider.Z.Y) &&
                !(collider.Z.X < additionalCollider.Z.X && additionalCollider.Z.Y < collider.Z.Y))
            {
                if (additionalCollider.Z.X < collider.Z.X && additionalCollider.Z.Y < collider.Z.Y) z = additionalCollider.Z.Y - collider.Z.X;
                if (additionalCollider.Z.X > collider.Z.X && additionalCollider.Z.Y > collider.Z.Y) z = additionalCollider.Z.X - collider.Z.Y;
            }
            if (x != 0 &&
                (MathF.Abs(x) < MathF.Abs(y) || y == 0) &&
                (MathF.Abs(x) < MathF.Abs(z) || z == 0))
            {
                collider.transform.GlobalPosition += Vector3.UnitX * x * 1.05f;
                rigBody.velocity += rigBody.velocity.X * x < 0 ? Vector3.UnitX * -rigBody.velocity.X * 1.5f : Vector3.Zero;
            }
            if (y != 0 &&
                (MathF.Abs(y) < MathF.Abs(x) || x == 0) &&
                (MathF.Abs(y) < MathF.Abs(z) || z == 0))
            {
                collider.transform.GlobalPosition += Vector3.UnitY * y * 1.05f;
                rigBody.velocity += rigBody.velocity.Y * y < 0 ? Vector3.UnitY * -rigBody.velocity.Y * 1.5f : Vector3.Zero;
            }
            if (z != 0 &&
                (MathF.Abs(z) < MathF.Abs(x) || x == 0) &&
                (MathF.Abs(z) < MathF.Abs(y) || y == 0))
            {
                collider.transform.GlobalPosition += Vector3.UnitZ * z * 1.05f;
                rigBody.velocity += rigBody.velocity.Z * z < 0 ? Vector3.UnitZ * -rigBody.velocity.Z * 1.5f : Vector3.Zero;
            }
        }
    }
}
