using CavingSimulator2.GameLogic.Objects;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.Components.Colliders.ColliderStrategy
{
    public interface IColliderStrategy
    {
        public abstract void Execute(Vector3i blockPosition);
        public abstract bool DetectCollision(Vector3i blockPosition);
        public abstract void RespondToCollision(Vector3i blockPosition);
    }
}
