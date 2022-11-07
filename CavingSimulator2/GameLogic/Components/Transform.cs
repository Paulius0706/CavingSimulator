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
        public Transform parent;



        private Vector3 localPosition = Vector3.Zero;
        private Quaternion localRotation = Quaternion.Identity;
        private Vector3 localScale = Vector3.One;
        public List<Transform> childs = new List<Transform>();

        public Transform(Vector3 position, Quaternion rotation, Vector3 scale, Transform parent)
        {
            this.parent = parent;
            this.LocalPosition = position;
            this.LocalRotation = rotation;
            this.LocalScale = scale;
        }
        public Transform(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            this.LocalPosition = position;
            this.LocalRotation = rotation;
            this.LocalScale = scale;
        }
        public Transform(Vector3 position, Quaternion rotation, Transform parent)
        {
            this.parent = parent;
            this.LocalPosition = position;
            this.LocalRotation = rotation;
        }
        public Transform(Vector3 position, Vector3 scale, Transform parent)
        {
            this.parent = parent;
            this.LocalPosition = position;
            this.LocalScale = scale;
        }
        public Transform(Vector3 position, Quaternion rotation)
        {
            this.LocalPosition = position;
            this.LocalRotation = rotation;
        }
        public Transform(Vector3 position, Vector3 scale)
        {
            this.LocalPosition = position;
            this.LocalScale = scale;
        }
        public Transform(Vector3 position, Transform parent)
        {
            this.parent = parent;
            this.LocalPosition = position;
        }
        public Transform(Vector3 position)
        {
            this.LocalPosition = position;
        }


        public Vector3 GlobalPosition
        {
            get
            {
                if (parent == null) return localPosition;
                return localPosition + parent.GlobalPosition; 
            }
            set
            {
                if (parent is null) localPosition = value;
                else localPosition = value - parent.GlobalPosition;
            }
        }
        public Quaternion GlobalRotation
        {
            get { return LocalRotation; }
            set { LocalRotation = value; }
        }
        public Vector3 GlobalScale
        {
            get { return LocalScale; }
            set { LocalScale = value; }
        }



        public Vector3 LocalPosition
        {
            get { return localPosition;}
            set { localPosition = value;}
        }

        public Vector3 LocalScale
        {
            get { return localScale; }
            set { localScale = value; }
        }
        public Quaternion LocalRotation
        {
            get { return localRotation; }
            set { localRotation = value; }
        }

    }
}
