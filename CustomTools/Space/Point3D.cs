namespace Space
{
    /// <summary>
    /// Represent a coordinate in three dimensional space.
    /// </summary>
    public struct Point3D
    {
        #region Variables

        /// <summary>
        /// X coordinate of the point.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y coordinate of the point.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Z coordinate of the point.
        /// </summary>
        public double Z { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a point in 3D space by assigning a X, Y and Z coordinate.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="z">Z coordinate.</param>
        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        #endregion
    }
}
