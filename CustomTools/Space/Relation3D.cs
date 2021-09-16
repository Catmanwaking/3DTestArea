using Matrices;
using Space;
using System;

namespace Space
{
    /// <summary>
    /// Provides constants and methods for calculating the relation between objects in three dimensional space.
    /// </summary>
    public static class Relation3D
    {
        #region Constants

        /// <summary>
        /// Represents one radian in degrees.
        /// </summary>
        public const double radianToDegreeConst = 180 / Math.PI;

        /// <summary>
        /// Represents one degree in radians.
        /// </summary>
        public const double degreeToRadianConst = Math.PI / 180;

        #endregion

        #region Static functions

        #region Intersections

        /// <summary>
        /// Calculates the intersection point of two given lines in 3D space.
        /// </summary>
        /// <param name="line1">Fist line.</param>
        /// <param name="line2">Second line.</param>
        /// <param name="intersection">Out parameter, true if intersection exists.</param>
        /// <returns>The point of the intersection.</returns>
        public static Point3D Intersection(Line3D line1, Line3D line2, out bool intersection)
        {
            Point3D point = new Point3D();
            Matrix linearEquation = new Matrix(line1, line2);
            linearEquation.ToRREF();
            int type = linearEquation.LinearEquationSolutionType();
            intersection = false;
            if (type == 1) //exactly one solution
            {
                if (line1.SupportVector.Z + linearEquation.Mtrx[0, 2] * line1.DirectionVector.Z
                    == line2.SupportVector.Z + linearEquation.Mtrx[1, 2] * line2.DirectionVector.Z)
                {
                    intersection = true;
                    point = line1.GetPointAt(linearEquation.Mtrx[0, 2]);
                }
            }
            else if (type == -1) //infinite soulutions
            {
                intersection = true;
                if (!IsParallel(line1, line2)) //actually not infinite solutions
                {
                    linearEquation = new Matrix(line1, line2, true);
                    linearEquation.ToRREF();
                    point = line1.GetPointAt(linearEquation.Mtrx[0, 2]);
                }
                else
                {
                    point.X = double.NaN;
                    point.Y = double.NaN;
                    point.Z = double.NaN;
                }
            }
            return point;
        }

        /// <summary>
        /// Calculates the intersection point of two given lines in 3D space.
        /// </summary>
        /// <param name="plane">Fist line.</param>
        /// <param name="line">Second line.</param>
        /// <param name="intersection">Out parameter, true if intersection exists.</param>
        /// <returns>The point of the intersection.</returns>
        public static Point3D Intersection(Plane3D plane, Line3D line, out bool intersection)
        {
            Point3D point = new Point3D();
            Matrix linearEquation = new Matrix(plane, line);
            linearEquation.ToRREF();
            int type = linearEquation.LinearEquationSolutionType();
            intersection = false;
            if (type == 1) //exactly one solution
            {
                if (plane.SupportVector.Z + linearEquation.Mtrx[0, 3] * plane.DirectionVectors[0].Z + linearEquation.Mtrx[1, 3] * plane.DirectionVectors[1].Z
                    == line.SupportVector.Z + linearEquation.Mtrx[2, 3] * line.DirectionVector.Z)
                {
                    intersection = true;
                    point = plane.GetPointAt(linearEquation.Mtrx[0, 3], linearEquation.Mtrx[1, 3]);
                }
            }
            else if (type == -1) //infinite soulutions
            {
                intersection = true;
                point.X = double.NaN;
                point.Y = double.NaN;
                point.Z = double.NaN;
            }
            return point;
        }

        #endregion

        #region Angles

        /// <summary>
        /// Calculates the angle between two vectors in radians.
        /// </summary>
        /// <param name="v1">First vector.</param>
        /// <param name="v2">Second vector.</param>
        /// <returns>Angle of the vectors in degrees.</returns>
        public static double AngleBetween(Vect3D v1, Vect3D v2)
        {
            double angle = Math.Round(v1 * v2 / (v1.GetLength() * v2.GetLength()), 10);
            angle = Math.Acos(angle);
            return Math.Round(angle, 3);
        }

        /// <summary>
        /// Calculates the angle between a line and a vector in radians.
        /// </summary>
        /// <param name="line">A line.</param>
        /// <param name="vector">A vector.</param>
        /// <returns></returns>
        public static double AngleBetween(Line3D line, Vect3D vector)
        {
            return AngleBetween(line.DirectionVector, vector);
        }

        /// <summary>
        /// Calculates the angle between two lines in radians.
        /// </summary>
        /// <param name="line1">First line.</param>
        /// <param name="line2">Second line.</param>
        /// <returns></returns>
        public static double AngleBetween(Line3D line1, Line3D line2)
        {
            return AngleBetween(line1.DirectionVector, line2.DirectionVector);
        }

        /// <summary>
        /// Calculates the angle between a plane and a vector in radians.
        /// </summary>
        /// <param name="plane">First line.</param>
        /// <param name="vector">Second line.</param>
        /// <returns></returns>
        public static double AngleBetween(Plane3D plane, Vect3D vector)
        {
            return Math.PI / 2 - AngleBetween(plane.NormalVector, vector);
        }

        /// <summary>
        /// Calculates the angle between a plane and a line in radians.
        /// </summary>
        /// <param name="plane">A plane.</param>
        /// <param name="line">A line</param>
        /// <returns></returns>
        public static double AngleBetween(Plane3D plane, Line3D line)
        {
            return Math.PI / 2 - AngleBetween(plane.NormalVector, line.DirectionVector);
        }

        /// <summary>
        /// Calculates the angle between two planes in radians.
        /// </summary>
        /// <param name="plane1">First plane.</param>
        /// <param name="plane2">Second plane.</param>
        /// <returns></returns>
        public static double AngleBetween(Plane3D plane1, Plane3D plane2)
        {
            return AngleBetween(plane1.NormalVector, plane2.NormalVector);
        }

        #endregion

        #region Parallels

        /// <summary>
        /// Checks if two vectors are parallel to each other.
        /// </summary>
        /// <param name="vector1">First vector.</param>
        /// <param name="vector2">Second vector.</param>
        /// <returns></returns>
        public static bool IsParallel(Vect3D vector1, Vect3D vector2)
        {
            //UNCHECKED may need to be rounded
            double scalar = vector1.X / vector2.X;
            if (vector1.Y == vector2.Y * scalar && vector1.Z == vector2.Z * scalar) return true;
            return false;
        }

        /// <summary>
        /// Checks if two lines are parallel to each other.
        /// </summary>
        /// <param name="line1">First line.</param>
        /// <param name="line2">Second line.</param>
        /// <returns></returns>
        public static bool IsParallel(Line3D line1, Line3D line2)
        {
            return IsParallel(line1.DirectionVector, line2.DirectionVector);
        }

        /// <summary>
        /// Checks if two planes are parallel to each other.
        /// </summary>
        /// <param name="plane1">First plane.</param>
        /// <param name="plane2">Second plane.</param>
        /// <returns></returns>
        public static bool IsParallel(Plane3D plane1, Plane3D plane2)
        {
            return IsParallel(plane1.NormalVector, plane2.NormalVector);
        }

        #endregion

        #endregion
    }
}
