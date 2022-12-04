using BepuPhysics;
using BepuPhysics.Collidables;
using BepuUtilities;
using BepuUtilities.Memory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CavingSimulator2.Physics.Shapes
{
    public struct Slope : IShape, IConvexShape
    {
        public float HalfWidth;
        public float HalfHeight;
        public float HalfLength;
        public int TypeId => 8;


        private static Mesh meshBase;

        public Mesh mesh;

        public Slope(float width, float height, float length)
        {
            HalfWidth = width * 0.5f; // X
            HalfHeight = height * 0.5f; // Y
            HalfLength = length * 0.5f; // Z

            mesh = new Mesh(meshBase.Triangles, new Vector3(width,height,length), Game.bufferPool);
        }
        
        public static void ImportMeshCollider(string path)
        {
            List<string> lines = File.ReadLines(path).ToList();
            List<Vector3> vertices = new List<Vector3>();
            List<int> indices = new List<int>();
            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                if (line.Length > 0 && line.StartsWith("v "))
                {
                    string[] sectors = line.Split(' ');
                    vertices.Add(new Vector3(
                        float.Parse(sectors[1], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(sectors[2], CultureInfo.InvariantCulture.NumberFormat),
                        float.Parse(sectors[3], CultureInfo.InvariantCulture.NumberFormat)
                        ));
                }
                if (line.Length > 0 && line.StartsWith("f "))
                {
                    string[] sectors = line.Split(' ').ToArray();
                    indices.Add(sectors[1].Split('/').Select(x => int.Parse(x) - 1).First());
                    indices.Add(sectors[2].Split('/').Select(x => int.Parse(x) - 1).First());
                    indices.Add(sectors[3].Split('/').Select(x => int.Parse(x) - 1).First());
                }
            }

            List<Vector3> verticesList = new List<Vector3>();
            
            for (int i = 0; i < indices.Count; i++)
            {
                verticesList.Add(vertices[indices[i]]);
            }
            Game.bufferPool.Take<Triangle>(verticesList.Count/3, out var triangles);
            for (int i = 0; i < verticesList.Count; i+=3)
            {
                triangles[i] = new Triangle(verticesList[i + 0], verticesList[i + 1], verticesList[i + 2]);
            }
            meshBase = new Mesh(triangles, Vector3.One, Game.bufferPool);
        }

        public void ComputeAngularExpansionData(out float maximumRadius, out float maximumAngularExpansion)
        {
            maximumRadius = (float)Math.Sqrt(HalfWidth * HalfWidth + HalfHeight * HalfHeight + HalfLength * HalfLength);
            maximumAngularExpansion = maximumRadius - Vector4.Min(new Vector4(HalfLength), Vector4.Min(new Vector4(HalfHeight), new Vector4(HalfLength))).X;
        }

        public void ComputeBounds(in Quaternion orientation, out Vector3 min, out Vector3 max)
        {
            Matrix3x3.CreateFromQuaternion(in orientation, out var result);
            Vector3 value = HalfWidth * result.X;
            Vector3 value2 = HalfHeight * result.Y;
            Vector3 value3 = HalfLength * result.Z;
            max = Vector3.Abs(value) + Vector3.Abs(value2) + Vector3.Abs(value3);
            min = -max;
        }

        public BodyInertia ComputeInertia(float mass)
        {
            return mesh.ComputeClosedInertia(mass);
        }

        public ShapeBatch CreateShapeBatch(BufferPool pool, int initialCapacity, BepuPhysics.Collidables.Shapes shapeBatches)
        {
            return mesh.CreateShapeBatch(pool, initialCapacity, shapeBatches);
        }

        public bool RayTest(in RigidPose pose, in Vector3 origin, in Vector3 direction, out float t, out Vector3 normal)
        {
            Vector3 v = origin - pose.Position;
            Matrix3x3.CreateFromQuaternion(in pose.Orientation, out var result);
            Matrix3x3.TransformTranspose(in v, in result, out var result2);
            Matrix3x3.TransformTranspose(in direction, in result, out var result3);
            Vector3 vector = new Vector3((result3.X < 0f) ? 1 : (-1), (result3.Y < 0f) ? 1 : (-1), (result3.Z < 0f) ? 1 : (-1)) / Vector3.Max(new Vector3(1E-15f), Vector3.Abs(result3));
            Vector3 vector2 = new Vector3(HalfWidth, HalfHeight, HalfLength);
            Vector3 value = (result2 - vector2) * vector;
            Vector3 value2 = (result2 + vector2) * vector;
            Vector3 vector3 = Vector3.Min(value, value2);
            Vector3 vector4 = Vector3.Max(value, value2);
            float num = ((vector4.X < vector4.Y) ? vector4.X : vector4.Y);
            if (vector4.Z < num)
            {
                num = vector4.Z;
            }

            if (num < 0f)
            {
                t = 0f;
                normal = default(Vector3);
                return false;
            }

            float num2;
            if (vector3.X > vector3.Y)
            {
                if (vector3.X > vector3.Z)
                {
                    num2 = vector3.X;
                    normal = result.X;
                }
                else
                {
                    num2 = vector3.Z;
                    normal = result.Z;
                }
            }
            else if (vector3.Y > vector3.Z)
            {
                num2 = vector3.Y;
                normal = result.Y;
            }
            else
            {
                num2 = vector3.Z;
                normal = result.Z;
            }

            if (num < num2)
            {
                t = 0f;
                normal = default(Vector3);
                return false;
            }

            t = ((num2 < 0f) ? 0f : num2);
            if (Vector3.Dot(normal, v) < 0f)
            {
                normal = -normal;
            }

            return true;
        }
    }
}
