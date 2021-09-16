using Matrices;
using System;

namespace Space
{
    /// <summary>
    /// Represents a vector in three dimensional space.
    /// Provides methods for creating and manipulating vectors.
    /// </summary>
    public struct Vect3D
    {
        #region Variables

        /// <summary>
        /// X coordiante of the vector.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y coordiante of the vector.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Z coordiante of the vector.
        /// </summary>
        public double Z { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates the local vector to the given point.
        /// </summary>
        /// <param name="end">End point of the vector.</param>
        public Vect3D(Point3D end)
        {
            X = end.X;
            Y = end.Y;
            Z = end.Z;
        }

        /// <summary>
        /// Creates a vector from two points in 3D space.
        /// </summary>
        /// <param name="start">Starting point of the vector.</param>
        /// <param name="end">End point of the vector.</param>
        public Vect3D(Point3D start, Point3D end)
        {
            X = end.X - start.X;
            Y = end.Y - start.Y;
            Z = end.Z - start.Z;
        }

        /// <summary>
        /// Creates a vector by assigning its coordinates directly.
        /// </summary>
        /// <param name="x">X coordinate of the vector.</param>
        /// <param name="y">Y coordinate of the vector.</param>
        /// <param name="z">Z coordinate of the vector.</param>
        public Vect3D(double x = 0.0, double y = 0.0, double z = 0.0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Calculates the length of the vector.
        /// </summary>
        /// <returns>Length of the vector.</returns>
        public double GetLength()
        {
            return Math.Round(Math.Sqrt(X * X + Y * Y + Z * Z), 10);
        }

        /// <summary>
        /// Turns the vector into a point in three dimensional space.
        /// </summary>
        /// <returns></returns>
        public Point3D ToPoint()
        {
            return new Point3D(X, Y, Z);
        }

        /// <summary>
        /// Turns the vector into a unit vector of lenght 1.
        /// </summary>
        /// <returns></returns>
        public Vect3D ToUnitVector()
        {
            Vect3D unitVector = this;
            unitVector *= (1 / GetLength());
            return unitVector;
        }

        /// <summary>
        /// Rotates a vector clockwise around a specified axis by the given angle.
        /// </summary>
        /// <param name="axis">Rotation axis.</param>
        /// <param name="angle">Rotation angle in radians.</param>
        /// <param name="copy">Creates a copy of the vector. False by default.</param>
        /// <returns></returns>
        public Vect3D RotateVector(Vect3D axis, double angle, bool copy = false)
        {
            axis *= (1 / axis.GetLength());
            Matrix rotationMatrix = Matrix.CreateRotationMatrix(axis, angle);
            Vect3D rotated = rotationMatrix * this;
            if (!copy)
                this = rotated;
            return rotated;
        }

        /// <summary>
        /// Switches the orientation of the vector.
        /// </summary>
        public void SwitchOrientation()
        {
            X = -X;
            Y = -Y;
            Z = -Z;
        }

        #endregion

        #region Static functions

        /// <summary>
        /// Creates the cross product/vector product of two vectors.
        /// </summary>
        /// <param name="v1">First vector.</param>
        /// <param name="v2">Second vector.</param>
        /// <returns>Resulting vector.</returns>
        public static Vect3D VectorProduct(Vect3D v1, Vect3D v2)
        {
            Vect3D v3 = new Vect3D();
            v3.X = v1.Y * v2.Z - v1.Z * v2.Y;
            v3.Y = v1.Z * v2.X - v1.X * v2.Z;
            v3.Z = v1.X * v2.Y - v1.Y * v2.X;
            return v3;
        }

        #endregion

        #region Overloaded operators

        //Vector addition
        public static Vect3D operator +(Vect3D v1, Vect3D v2)
        {
            v1.X += v2.X;
            v1.Y += v2.Y;
            v1.Z += v2.Z;
            return v1;
        }

        //Vector subtraction
        public static Vect3D operator -(Vect3D v1, Vect3D v2)
        {
            v1.X -= v2.X;
            v1.Y -= v2.Y;
            v1.Z -= v2.Z;
            return v1;
        }

        //Scalar product
        public static double operator *(Vect3D v1, Vect3D v2)
        {
            return (v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z);
        }

        //Scalar multiplication
        public static Vect3D operator *(Vect3D v, double i)
        {
            return new Vect3D(v.X * i, v.Y * i, v.Z * i);
        }

        #endregion
    }
}
