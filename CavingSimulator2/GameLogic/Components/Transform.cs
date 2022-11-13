using CavingSimulator2.GameLogic.Objects;
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
        public BaseObject baseObject;


        private Vector3 localPosition = Vector3.Zero;
        private Vector3 localRotation = Vector3.Zero;
        private Vector3 localScale = Vector3.One;
        public List<Transform> childs = new List<Transform>();

        public Transform(Vector3 position, Vector3 rotation, Vector3 scale, Transform parent)
        {
            this.parent = parent;
            this.LocalPosition = position;
            this.LocalRotation = rotation;
            this.LocalScale = scale;
        }
        public Transform(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            this.LocalPosition = position;
            this.LocalRotation = rotation;
            this.LocalScale = scale;
        }
        public Transform(Vector3 position, Vector3 rotation, Transform parent)
        {
            this.parent = parent;
            this.LocalPosition = position;
            this.LocalRotation = rotation;
        }
        public Transform(Vector3 position, Vector3 rotation)
        {
            this.LocalPosition = position;
            this.LocalRotation = rotation;
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
                Vector3 parentGlobalRotation = parent.GlobalRotation;
                return new Vector3( 
                    new Vector4(parent.GlobalPosition, 1) + 
                    new Vector4(localPosition,1) * 
                    Matrix4.CreateFromQuaternion(
                        new Quaternion(
                            MathHelper.DegreesToRadians(parentGlobalRotation.X),
                            MathHelper.DegreesToRadians(parentGlobalRotation.Y),
                            MathHelper.DegreesToRadians(parentGlobalRotation.Z),
                            1)
                        )
                    ); 
            }
            set
            {
                if (parent is null) localPosition = value;
                else localPosition = value - parent.GlobalPosition;
            }
        }
        public Vector3 GlobalRotation
        {
            get 
            {
                if (parent == null) return localRotation;
                return  parent.GlobalRotation + localRotation;
            }
            set 
            {
                if (parent is null) localPosition = value;
                else localRotation = value - parent.localRotation;
            }
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
        public Vector3 LocalRotation
        {
            get { return localRotation; }
            set { localRotation = value; }
        }

    }
}
