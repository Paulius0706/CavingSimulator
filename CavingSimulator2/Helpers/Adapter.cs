using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.Helpers
{
    public static class Adapter
    {

        public static System.Numerics.Vector3 Convert(OpenTK.Mathematics.Vector3 vector)
        {
            return new System.Numerics.Vector3(vector.X, vector.Y, vector.Z);
        }
        public static OpenTK.Mathematics.Vector3 Convert(System.Numerics.Vector3 vector)
        {
            return new OpenTK.Mathematics.Vector3(vector.X, vector.Y, vector.Z);
        }
        public static System.Numerics.Quaternion Convert(OpenTK.Mathematics.Quaternion quaternion)
        {
            OpenTK.Mathematics.Vector3 vector = quaternion.ToEulerAngles();
            return new System.Numerics.Quaternion(vector.X, vector.Y, vector.Z, 1);
        }
        public static OpenTK.Mathematics.Quaternion Convert(System.Numerics.Quaternion quaternion)
        {
            return new OpenTK.Mathematics.Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
        }

        
    }
}
