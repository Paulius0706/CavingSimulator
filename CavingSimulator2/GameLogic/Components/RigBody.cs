using CavingSimulator2;
using CavingSimulator2.GameLogic.Components;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator.GameLogic.Components
{
    public class RigBody : Component
    {
        public readonly Transform transform;
        public Vector3 velocity = Vector3.Zero;
        public Vector3 angularVelocity = Vector3.Zero;

        public float drag = 1f;
        public float angularDrag = 1f;
        public bool enableGravity = false;
        public float gravity = 1f;

        public RigBody(Transform transform)
        {
            this.transform = transform;
            this.velocity = Vector3.Zero;
            this.angularVelocity = Vector3.Zero;
            this.drag = 1f;
            this.angularDrag = 1f;
            this.enableGravity = false;
            this.gravity = 1f;
        }

        public void Update()
        {
            bool isStatic = true;
            if (enableGravity) { velocity.Z -= gravity; }
            if (velocity.LengthSquared < 0.05f && angularVelocity.LengthSquared < 0.05f) return;
            //Console.WriteLine("Rig moving P:" + transform.GlobalPosition);
            
            transform.GlobalPosition += velocity * Game.deltaTime;
            //gameObject.Transform.GlobalRotation = new Quaternion(gameObject.Transform.GlobalRotation.ToEulerAngles() + angularVelocity * Game.deltaTime);

            velocity = GoTowards(velocity, Vector3.Zero, drag * Game.deltaTime);
            //angularVelocity -= GoTowards(angularVelocity, Vector3.Zero, angularDrag * Game.deltaTime);

            //gameObject.GetComponent<Collider>().CheckCollisions();
        }
        public void AddVelocity(Vector3 velocity)
        {
            this.velocity += velocity;
        }
        public void AddVelocity(Vector3 velocity, float maxSpeed)
        {
            this.velocity += velocity;
            if(this.velocity.LengthFast > maxSpeed)
            {
                this.velocity = this.velocity.Normalized() * maxSpeed;
            }
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
