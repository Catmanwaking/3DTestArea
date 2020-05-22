namespace Space
{
    /// <summary>
    /// Represents a plane in three dimensional space.
    /// Provides methods for creating planes.
    /// </summary>
    public class Plane3D
    {
        #region Variables

        /// <summary>
        /// The vector pointing to a point on the plane
        /// </summary>
        public Vect3D SupportVector { get; set; }

        /// <summary>
        /// The vectors that togeter define the tilt of the plane.
        /// </summary>
        public Vect3D[] DirectionVectors { get; set; } = new Vect3D[2];

        /// <summary>
        /// The normal vector of the plane.
        /// </summary>
        public Vect3D NormalVector { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates the a plane in three dimensional space using three points in three dimensional space.
        /// </summary>
        /// <param name="point1">First point.</param>
        /// <param name="point2">Second point.</param>
        /// <param name="point3">Third point.</param>
        public Plane3D(Point3D point1, Point3D point2, Point3D point3)
        {
            SupportVector = new Vect3D(point1);
            DirectionVectors[0] = new Vect3D(point1, point2);
            DirectionVectors[1] = new Vect3D(point1, point3);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="triangle"></param>
        public Plane3D(Triangle3D triangle)
        {
            SupportVector = new Vect3D(triangle.coordinates[0]);
            DirectionVectors[0] = new Vect3D(triangle.coordinates[0], triangle.coordinates[1]);
            DirectionVectors[1] = new Vect3D(triangle.coordinates[0], triangle.coordinates[2]);
        }

        #endregion

        #region Functions

        /// <summary>
        /// Returns the point that the functions of the plane points to.
        /// </summary>
        /// <param name="vectorScalar1">The scalar for the first direction vector.</param>
        /// <param name="vectorScalar2">The scalar for the second directions vector.</param>
        /// <returns></returns>
        public Point3D GetPointAt(double vectorScalar1, double vectorScalar2)
        {
            return (SupportVector + DirectionVectors[0] * vectorScalar1 
                + DirectionVectors[1] * vectorScalar2).ToPoint();
        }

        #endregion
    }
}
