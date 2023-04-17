
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.Render
{
    public static class Camera
    {
        public static Vector3 position { get { return relative_position  - lookToPoint * lenght; } }
        public static Vector3 relative_position = Vector3.One;
        public static float lenght = 5;


        public static Vector3 lookToPoint { get { return _lookToPoint; } set { _lookToPoint = Vector3.Normalize(value); } }
        private static Vector3 _lookToPoint = new Vector3(0, 1, 0);

        // [FIX IT] do correct stuff up Z axis
        public static readonly Vector3 up = new Vector3(0.0f, 0.0f, 1.0f);
        public static float yaw = 0;
        public static float pitch = 0;


        public static void SetYawPitch(float yaw, float pitch)
        {
            Camera.yaw = yaw;
            Camera.pitch = pitch;
            if (Camera.pitch > MathHelper.DegreesToRadians(89.0f)) { Camera.pitch = MathHelper.DegreesToRadians(89.0f); }
            else if (Camera.pitch < MathHelper.DegreesToRadians(-89.0f)) { Camera.pitch = MathHelper.DegreesToRadians(-89.0f); }

            _lookToPoint.X = (float)Math.Cos(Camera.pitch) * (float)Math.Cos(Camera.yaw);
            _lookToPoint.Z = (float)Math.Sin(Camera.pitch);
            _lookToPoint.Y = (float)Math.Cos(Camera.pitch) * (float)Math.Sin(Camera.yaw);

            _lookToPoint = Vector3.Normalize(_lookToPoint);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaYaw">Degree</param>
        /// <param name="deltaPitch">Degree</param>
        public static void SetDeltaYawPitch(float deltaYaw, float deltaPitch)
        {
            yaw -= deltaYaw;
            pitch -= deltaPitch;
            //Console.WriteLine(
            //    "yaw  : " + Camera.yaw + "\n" +
            //    "pitch: " + Camera.pitch);
            //Console.WriteLine("H:" + position.Z);
            if (pitch > MathHelper.DegreesToRadians(89.0f)) { pitch = MathHelper.DegreesToRadians(89.0f); }
            else if (pitch < MathHelper.DegreesToRadians(-89.0f)) { pitch = MathHelper.DegreesToRadians(-89.0f); }

            _lookToPoint.X = (float)Math.Cos(pitch) * (float)Math.Cos(yaw);
            _lookToPoint.Z = (float)Math.Sin(pitch);
            _lookToPoint.Y = (float)Math.Cos(pitch) * (float)Math.Sin(yaw);

            _lookToPoint = Vector3.Normalize(_lookToPoint);
        }

        public static void Update()
        {
            Matrix4 view = Matrix4.LookAt(position, position + _lookToPoint, up);
            Game.view = view;
            //Game.objectShader.SetUniform("View", ref view);
            //Game.blockShader.SetUniform("View", ref view);
        }


    }
}
