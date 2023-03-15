using BepuPhysics;
using CavingSimulator2.GameLogic.Components.Physics;
using CavingSimulator2.GameLogic.Objects;
using CavingSimulator2.Helpers;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator.GameLogic.Components
{
    public class Transform
    {
        public BaseObject baseObject;


        private Vector3 position = Vector3.Zero;
        private Quaternion rotation = Quaternion.Identity;
        private Vector3 scale = Vector3.One;
        public List<Transform> childs = new List<Transform>();

        public Transform(Vector3 position, Quaternion rotation, Vector3 scale, Transform parent)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }
        public Transform(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }
        public Transform(Vector3 position, Quaternion rotation, Transform parent)
        {
            this.position = position;
            this.rotation = rotation;
        }
        public Transform(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
        public Transform(Vector3 position, Transform parent)
        {
            this.position = position;
        }
        public Transform(Vector3 position)
        {
            this.position = position;
        }


        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }
        public Quaternion Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
            }
        }
        public Vector3 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

    }
}
