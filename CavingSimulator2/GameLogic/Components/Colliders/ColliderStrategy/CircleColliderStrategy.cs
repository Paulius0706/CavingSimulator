using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.Components.Colliders.ColliderStrategy
{
    public class CircleColliderStrategy : IColliderStrategy
    {
        public readonly Collider collider;

        public CircleColliderStrategy(Collider collider)
        {
            this.collider = collider;
        }

        public bool DetectCollision(Vector3i blockPosition)
        {
            throw new NotImplementedException();
        }

        public void Execute(Vector3i blockPosition)
        {
            throw new NotImplementedException();
        }

        public void RespondToCollision(Vector3i blockPosition)
        {
            throw new NotImplementedException();
        }
    }
}
