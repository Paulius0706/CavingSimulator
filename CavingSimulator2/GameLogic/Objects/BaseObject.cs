
using CavingSimulator2.GameLogic.Components.Physics;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.GameLogic.Objects
{
    public abstract class BaseObject : IDisposable
    {
        public readonly int id;
        public static int incremeter;
        private bool disposed;
        
        public BaseObject()
        {
            id = incremeter++;
        }
        public void Dispose()
        {
            
            //Console.WriteLine("gameObject is disposed");
            if (disposed) return;
            disposed = true;
            AbstractDispose();


        }
        protected virtual void AbstractDispose() { }

        public virtual void CollisionTrigger(Vector3i blockPosition) { }

        public virtual void Render() { }
        public virtual void Update() { }

        public virtual bool TryGetRigBody(out RigBody rigBody) { rigBody = null; return false; }
    }
}
