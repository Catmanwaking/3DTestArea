namespace Space
{
    /// <summary>
    /// Represents a Line in three dimensional space.
    /// Provides methods for creating lines.
    /// </summary>
    public class Line3D
    {
        #region Variables

        /// <summary>
        /// The vector pointing to a point on the line.
        /// </summary>
        public Vect3D SupportVector { get; set; }

        /// <summary>
        /// The vector pointing into the direction of the line.
        /// </summary>
        public Vect3D DirectionVector { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a line from two points in three dimensional space.
        /// </summary>
        /// <param name="p1">First point.</param>
        /// <param name="p2">Second point.</param>
        public Line3D(Point3D p1, Point3D p2)
        {
            SupportVector = new Vect3D(p1);
            DirectionVector = new Vect3D(p1, p2);
        }

        /// <summary>
        /// Creates a line from a point on the line and a direction vector.
        /// </summary>
        /// <param name="point">Point on the line.</param>
        /// <param name="directionVector">Direction of the line.</param>
        public Line3D(Point3D point, Vect3D directionVector)
        {
            SupportVector = new Vect3D(point);
            DirectionVector = directionVector;
        }

        /// <summary>
        /// Creates a line from a support vector and a direction vector.
        /// </summary>
        /// <param name="supportVector">The support vector.</param>
        /// <param name="directionVector">The direction vector.</param>
        public Line3D(Vect3D supportVector, Vect3D directionVector)
        {
            SupportVector = supportVector;
            DirectionVector = directionVector;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Retruns the point that the function of the line points to.
        /// </summary>
        /// <param name="vectorScalar">The scalar for the direction vector.</param>
        /// <returns></returns>
        public Point3D GetPointAt(double vectorScalar)
        {           
            return (SupportVector + DirectionVector * vectorScalar).ToPoint();
        }

        #endregion
    }
}
