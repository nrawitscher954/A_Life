

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SharpMatter.SharpMath;



namespace SharpMatter.Geometry
{
    //[Serializable]
    public class Vec3 : MonoBehaviour , IEquatable<Vec3>
    {


        #region Operator Override

        /// <summary>
        /// declare an implicit user-defined type conversion. Convert from Vec3 to String
        /// </summary>
        /// <param name="vector"></param>
        public static implicit operator string(Vec3 vector)
        {
            return vector.ToString();
        }

   







        /// <summary>
        /// adds second vector to the first one
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vec3 operator +(Vec3 a, Vec3 b)
        {

            a.x += b.x;
            a.y += b.y;
            a.z += b.z;

            return a;
        }

        public static Vec3 operator +(Vec3 a, Vector3 b)
        {

            a.x += b.x;
            a.y += b.y;
            a.z += b.z;

            return a;
        }

        /// <summary>
        /// Substract second vector from the first one
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>

        public static Vec3 operator -(Vec3 a, Vec3 b)
        {
            a.x -= b.x;
            a.y -= b.y;
            a.z -= b.z;

            return a;
        }


        public static Vec3 operator -(Vec3 a, Vector3 b)
        {
            a.x -= b.x;
            a.y -= b.y;
            a.z -= b.z;

            return a;
        }


        public static Vector3 operator -(Vector3 a, Vec3 b)
        {
           a.x -= (float) b.x;
            a.y -= (float) b.y;
            a.z -= (float) b.z;

            return  a;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vec3 operator /(Vec3 a, Vec3 b)
        {
            a.x /= b.x;
            a.y /= b.y;
            a.z /= b.z;

            return a;
        }


        /// <summary>
        /// divides a vector by a number, this produces the result of scaling the vector
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Vec3 operator /(Vec3 a, double b)
        {
            a.x /= b;
            a.y /= b;
            a.z /= b;

            return a;
        }


        /// <summary>
        /// Multiple a vector by a scalar value, this produces the effect of scaling  a vector
        /// </summary>
        /// <param name="a"></param>
        /// <param name="t"></param>
        /// <returns></returns> a scaled vector

        public static Vec3 operator *(Vec3 a, double t)
        {
            a.x *= t;
            a.y *= t;
            a.z *= t;
            return a;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="scalar"></param>
        /// <returns></returns>
        public static Vec3 operator *(double scalar, Vec3 vector)
        {
            vector.x *= scalar;
            vector.y *= scalar;
            vector.z *= scalar;
            return vector;
        }





        /// <summary>
        /// 
        /// This method multiplies two vectors together
        /// This value equals a.Length * b.Length * cos(alpha), where alpha is the angle between vectors.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="t"></param>
        /// <returns></returns> returns a number
        public static double operator *(Vec3 a, Vec3 b)
        {

            return (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Vec3 a, Vec3 b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Vec3 a, Vec3 b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>

        public static bool operator <(Vec3 a, Vec3 b)
        {
            return a.x < b.x || a.x == b.x && a.y < b.y || a.x == b.x && a.y == b.y && a.z < b.z;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >(Vec3 a, Vec3 b)
        {
            return a.x > b.x || a.x == b.x && a.y > b.y || a.x == b.x && a.y == b.y && a.z > b.z;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator <=(Vec3 a, Vec3 b)
        {
            return a.x < b.x || a.x == b.x && a.z < b.z || a.x == b.x && a.z == b.z && a.z <= b.z;


        }

        /// <summary>
        /// return true if vector a is larger than or equal to vector b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >=(Vec3 a, Vec3 b)
        {
            return a.x > b.x || a.x == b.x && a.y > b.y || a.x == b.x && a.y == b.y && a.z >= b.z;

        }





        #endregion



        public double x;
        public double y;
        public double z;

        public float X;
        public float Y;
        public float Z;




        /// <summary>
        /// construct vector from double values
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vec3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }


        public Vec3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        /// <summary>
        /// Construct a vector by coopying coordinates of another vector
        /// </summary>
        /// <param name="other"></param>
        public Vec3(Vec3 other)
        {
            this.x = other.x;
            this.y = other.y;
            this.z = other.z;
        }

       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>The length of the line between this and the other point; or 0 if any of the points is not valid.
        public double DistanceTo(Vec3 other)
        {
            if (other == null) return 0;

            this.x -= other.x;
            this.y -= other.y;
            this.z -= other.z;
            double dist = new Vec3(this.x, this.y, this.z).Magnitude;
            return dist;
        }


        /// <summary>
        /// Equal steps in the parameter t give rise to equal steps in the ineterpolated values
        /// In interpolations, all terms must sum up to 1
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Vec3 LinearInterpolation(Vec3 a, Vec3 b, double t)
        {
            t = SharpMath.SharpMath.Clamp(t, 0, 1);
            double deltaX = a.x * (1 - t) + (b.x * t);
            double deltaY = a.y * (1 - t) + (b.y * t);
            double deltaZ = a.x * (1 - t) + (b.z * t);

            return new Vec3(deltaX, deltaY, deltaZ);
        }


        public double Magnitude
        {
            get { return Math.Sqrt(SquareLength); }

        }

        private void Multiply(double n)
        {
            this.x *= n;
            this.y *= n;
            this.z *= n;
        }



        /// <summary>
        /// Distribute randomly a set of vectors within a rectangular boundary
        /// </summary>
        /// <param name="minX"></param> 
        /// <param name="maxX"></param>
        /// <param name="minY"></param>
        /// <param name="maxY"></param>
        /// <param name="minZ"></param>
        /// <param name="maxZ"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        public static Vec3 GetRandomPoint(double minX, double maxX, double minY, double maxY, double minZ, double maxZ, System.Random random)
        {
            double x = minX + (maxX - minX) * random.NextDouble();
            double y = minY + (maxY - minY) * random.NextDouble();
            double z = minZ + (maxZ - minZ) * random.NextDouble();



            return new Vec3(x, y, z);
        }


        /// <summary>
        /// Create unit vectors with Random 3D directions
        /// </summary>
        /// <param name="random"></param>
        /// <returns></returns>
        /// 

        public static Vec3 GetRandomVector3D(System.Random random)
        {

            double phi = 2.0 * Math.PI * random.NextDouble();
            double theta = Math.Acos(2.0 * random.NextDouble() - 1.0);

            double x = Math.Sin(theta) * Math.Cos(phi);
            double y = Math.Sin(theta) * Math.Sin(phi);
            double z = Math.Cos(theta);

            return new Vec3(x, y, z);

        }


        /// <summary>
        /// Create unit vectors with Random 2D directions
        /// </summary>
        /// <param name="random"></param>
        /// <returns></returns>

        //public static Vec3 GetRandomVector2D(Random random)
        //{



        //        double angle = 2.0 * Math.PI * random.NextDouble();

        //        double x = Math.Cos(angle);
        //        double y = Math.Sin(angle);

        //        return new Vec3(x, y, 0.0);

        //}

        /// <summary>
        /// 
        /// </summary>

        private double SquareLength
        {
            get { return this.x * this.x + this.y * this.y + this.z * this.z; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static Vec3 Zero
        {
            get { return new Vec3(0, 0, 0); }
        }



        /// <summary>
        /// Limits the magnitude of a vector if its larger than max value
        /// </summary>
        /// <param name="max"></param>
        public void Limit(double max)
        {
            if (Magnitude > max)
            {
                Normalize();
                Multiply(max);

            }
        }





        /// <summary>
        /// Unitizes the vector in place. A unit vector has length 1 unit.
        ///An invalid or zero length vector cannot be unitized.
        /// </summary>
        /// <returns></returns> true if vector was unitized and whose length is not 0.0
        public bool Normalize()
        {
            double magnitude = SquareLength;
            if (magnitude > 0.0)
            {
                magnitude = 1.0 / Math.Sqrt(magnitude);
                this.x *= magnitude;
                this.y *= magnitude;
                this.z *= magnitude;

                return true;
            }

            else
                return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public override string ToString()
        {
            double a = this.x;
            double b = this.y;
            double c = this.z;

            return "{" + a.ToString() + "," + " " + b.ToString() + "," + " " + c.ToString() + "}";

        }



        #region Equality and Hashcode Override

        public bool Equals(Vec3 obj)
        {
            if (obj == null)
                return false;

            if (this.x == obj.x && this.y == obj.y)
                return true;
            else
                return false;
        }

        public override bool Equals(object obj)
        {
            /////  https://msdn.microsoft.com/en-us/library/system.object.gethashcode(v=vs.110).aspx.
            if (!(obj is Vec3)) return false;

            Vec3 vec = (Vec3)obj;
            return this.x == vec.x && this.y == vec.y;



        }


        public override int GetHashCode()
        {
            ///  https://msdn.microsoft.com/en-us/library/system.object.gethashcode(v=vs.110).aspx.
            return ShiftAndWrap(this.x.GetHashCode(), 2) ^ this.y.GetHashCode();
        }


        private int ShiftAndWrap(int value, int positions)
        {
            /// https://msdn.microsoft.com/en-us/library/system.object.gethashcode(v=vs.110).aspx.
            positions = positions & 0x1F;

            // Save the existing bit pattern, but interpret it as an unsigned integer.
            uint number = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
            // Preserve the bits to be discarded.
            uint wrapped = number >> (32 - positions);
            // Shift and wrap the discarded bits.
            return BitConverter.ToInt32(BitConverter.GetBytes((number << positions) | wrapped), 0);
        }


        #endregion





    } ///  end class


}
