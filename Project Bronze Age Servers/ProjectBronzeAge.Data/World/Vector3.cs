using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBronzeAge.Data
{
    public struct Vector3
    {
        public float x, y, z;
        public float length { get{ return (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2)); } }

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        
        public static float Distance(Vector3 v1, Vector3 v2)
        {
            var direction = v2 - v1;
            return direction.length;
        }
        
        // Operators
        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            if (v1.x == v2.x && v1.y == v2.y && v1.z == v2.z)
                return true;
            else return false;
        }
        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            if (v1.x != v2.x && v1.y != v2.y && v1.z != v2.z)
                return true;
            else return false;
        }
        public static Vector3 operator *(Vector3 v, float constant)
        {
            v.x *= constant;
            v.y *= constant;
            v.z *= constant;
            return v; 
        }
        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            v1.x -= v2.x;
            v1.y -= v2.y;
            v1.z -= v2.z;
            return v1;
        }
    }
}
