namespace Space
{
    /// <summary>
    /// Represents a triangle in three dimensional space.
    /// Provides methods for creating triangles.
    /// </summary>
    public struct Triangle3D
    {
        #region Variables

        /// <summary>
        /// Coordinates of the triangle's corners.
        /// </summary>
        public readonly Point3D[] coordinates;

        #endregion

        #region Constructors         

        /// <summary>
        /// Creates a three dimensional triangle using three points as input. 
        /// </summary>
        /// <param name="p1">First corner.</param>
        /// <param name="p2">Second corner.</param>
        /// <param name="p3">Third corner.</param>
        public Triangle3D(Point3D p1, Point3D p2, Point3D p3)
        {
            coordinates = new Point3D[3];
            coordinates[0] = p1;
            coordinates[1] = p2;
            coordinates[2] = p3;
        }

        #endregion
    }
}
