using Matrices;
using Space;
using System;
using System.Collections.Generic;
using System.Windows;

namespace SpaceObjectVisualization.Tools
{
    /// <summary>
    /// 
    /// </summary>
    class Camera
    {
        #region Variables

        /// <summary>
        /// The camera's Position.
        /// </summary>
        public Point3D CameraPosition { get; set; }

        #region Vectors

        /// <summary>
        /// The cameras's view direction.
        /// Used for forward/backward movement and right/left camera roll.
        /// </summary>
        private Vect3D cameraDirectionUnitVector;

        /// <summary>
        /// A camera vector used for right/left rotation and up/down movement.
        /// </summary>
        private Vect3D cameraVerticalUnitVector;

        /// <summary>
        /// A camera vector used for up/down rotation and right/left movement.
        /// </summary>
        private Vect3D cameraHorizontalUnitVector;

        /// <summary>
        /// Normal vector used for angle calculation between a vector and the left edge of the camera view.
        /// </summary>
        private Vect3D cameraLeftPlaneNormalVector;

        /// <summary>
        /// Normal vector used for angle calculation between a vector and the right edge of the camera view.
        /// </summary>
        private Vect3D cameraRightPlaneNormalVector;

        /// <summary>
        /// Normal vector used for angle calculation between a vector and the top edge of the camera view.
        /// </summary>
        private Vect3D cameraTopPlaneNormalVector;

        /// <summary>
        /// Normal vector used for angle calculation between a vector and the bottom edge of the camera view.
        /// </summary>
        private Vect3D cameraBottomPlaneNormalVector;

        #endregion

        #region Doubles

        /// <summary>
        /// The field of wiev of the camera in radians.
        /// </summary>
        public double CameraFieldOfView { get; }

        /// <summary>
        /// The aspect ration of the screen.
        /// </summary>
        public double AspectRatio { get; }

        /// <summary>
        /// Used with trigonometric arithmetic to determine width pixel position of cornerpoints.
        /// </summary>
        private readonly double widthScalar;

        /// <summary>
        /// Used with trigonometric arithmetic to determine height pixel position of cornerpoints.
        /// </summary>
        private readonly double heightScalar;

        #endregion

        /// <summary>
        /// List containing all triangles.
        /// TODO delete when working with objects
        /// </summary>
        public Triangle3D[] triangles;

        /// <summary>
        /// List containing all objects.
        /// </summary>
        public List<Object3D> objects = new List<Object3D>();

        /// <summary>
        /// Array containing the indexes of the triangles within the camera's view.
        /// </summary>
        public int[] trianglesOnScreenIndex;

        /// <summary>
        /// Screen used for two dimensional projection of the triangles.
        /// </summary>
        public Screen screen;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a camera and its screen.
        /// </summary>
        /// <param name="screenWidth">Width of the screen.</param>
        /// <param name="screenHeight">Height of the screen.</param>
        public Camera(int screenWidth, int screenHeight)
        {
            CameraFieldOfView = 90 / Relation3D.radianToDegreeConst;           
            AspectRatio = (screenWidth / (double)screenHeight);
            widthScalar = Math.Sin((Math.PI - CameraFieldOfView) / 2) / Math.Sin(CameraFieldOfView);
            heightScalar = Math.Sin((Math.PI - CameraFieldOfView / AspectRatio) / 2) / Math.Sin(CameraFieldOfView / AspectRatio);        
            screen = new Screen(screenWidth, screenHeight);

            CreateTestTriangles();
            Reset();
        }

        #endregion

        #region Funcions

        /// <summary>
        /// Calculate the normal vectors used for angle calculation.
        /// </summary>
        private void CreatePlaneNormalVectors()
        {
            cameraLeftPlaneNormalVector = cameraDirectionUnitVector.RotateVector(cameraVerticalUnitVector, CameraFieldOfView / 2 - Math.PI / 2, true);
            cameraRightPlaneNormalVector = cameraDirectionUnitVector.RotateVector(cameraVerticalUnitVector, -(CameraFieldOfView / 2 - Math.PI / 2), true);
            cameraTopPlaneNormalVector = cameraDirectionUnitVector.RotateVector(cameraHorizontalUnitVector, (CameraFieldOfView / AspectRatio) / 2 - Math.PI / 2, true);
            cameraBottomPlaneNormalVector = cameraDirectionUnitVector.RotateVector(cameraHorizontalUnitVector, -((CameraFieldOfView / AspectRatio) / 2 - Math.PI / 2), true);
        }

        /// <summary>
        /// Calculates which triangles are within the camera's field of view.
        /// </summary>
        public void DetermineTrianglesInRange()
        {
            //TODO can be heavly optimized by reading directly from the objects verticies. CREATE BACKUP WHEN EDITING.
            
            Vect3D toPointVector;
            Stack<int> trianglesIndex = new Stack<int>();
            foreach(Triangle3D triangle in triangles)
            {
                foreach(Point3D point in triangle.coordinates)
                {
                    toPointVector = new Vect3D(CameraPosition, point);
                    if (Relation3D.AngleBetween(toPointVector, cameraLeftPlaneNormalVector) > Math.PI / 2) continue;
                    if (Relation3D.AngleBetween(toPointVector, cameraRightPlaneNormalVector) > Math.PI / 2) continue;
                    if (Relation3D.AngleBetween(toPointVector, cameraTopPlaneNormalVector) > Math.PI / 2) continue;
                    if (Relation3D.AngleBetween(toPointVector, cameraBottomPlaneNormalVector) > Math.PI / 2) continue;
                    trianglesIndex.Push(Array.IndexOf(triangles, triangle));
                    break;
                }
            }
            trianglesOnScreenIndex = trianglesIndex.ToArray();            
        }

        public void BackFaceCulling()
        {
            Stack<int> trianglesIndex = new Stack<int>();
            Vect3D camToCorner;
            Vect3D cornerVector1;
            Vect3D cornerVector2;
            foreach (int index in trianglesOnScreenIndex)
            {
                cornerVector1 = new Vect3D(triangles[index].coordinates[0], triangles[index].coordinates[1]);
                cornerVector2 = new Vect3D(triangles[index].coordinates[0], triangles[index].coordinates[2]);
                camToCorner = new Vect3D(CameraPosition, triangles[index].coordinates[0]);
                if (Vect3D.VectorProduct(cornerVector1, cornerVector2) * camToCorner >= 0.0)
                {
                    trianglesIndex.Push(index);
                }                
            }
            trianglesOnScreenIndex = trianglesIndex.ToArray();
        }

        /// <summary>
        /// Calculates the triangles position on the screen and adds them to the screens collection of triangles.
        /// </summary>
        public void ProjectToScreen()
        {
            screen.ClearTriangles();

            //Used for angle calculation.
            Vect3D toPointVector;
            Vect3D toPointVerticalPlaneNormalVector;
            Vect3D toPointHorizontalPlaneNormalVector;

            //Angles.
            double angleLeft;
            double angleTop;

            //Position on screen: 0 = left/top, 1 = right/bottom.
            double screenScalar;

            Point[] triangleCoordinatesOnScreen;
            BackFaceCulling();
            foreach (int triangleIndex in trianglesOnScreenIndex)
            {
                triangleCoordinatesOnScreen = new Point[3];
                for (int i = 0; i < 3; i++)
                {
                    toPointVector = new Vect3D(CameraPosition, triangles[triangleIndex].coordinates[i]);

                    toPointVerticalPlaneNormalVector = Vect3D.VectorProduct(toPointVector, cameraVerticalUnitVector);
                    toPointHorizontalPlaneNormalVector = Vect3D.VectorProduct(toPointVector, cameraHorizontalUnitVector);
                    angleLeft = Relation3D.AngleBetween(toPointVerticalPlaneNormalVector, cameraLeftPlaneNormalVector);
                    angleTop = Relation3D.AngleBetween(toPointHorizontalPlaneNormalVector, cameraTopPlaneNormalVector);

                    toPointVerticalPlaneNormalVector.SwitchOrientation();
                    toPointHorizontalPlaneNormalVector.SwitchOrientation();                    

                    screenScalar = widthScalar * Math.Sin(angleLeft) / Math.Sin(Math.PI - angleLeft - (Math.PI - CameraFieldOfView) / 2);
                    if (Relation3D.AngleBetween(toPointVerticalPlaneNormalVector, cameraRightPlaneNormalVector) > CameraFieldOfView)
                        screenScalar *= -1;
                    triangleCoordinatesOnScreen[i].X = Math.Round(screenScalar * screen.Width, 0);

                    screenScalar = heightScalar * Math.Sin(angleTop) / Math.Sin(Math.PI - angleTop - (Math.PI - (CameraFieldOfView / AspectRatio)) / 2);
                    if (Relation3D.AngleBetween(toPointHorizontalPlaneNormalVector, cameraBottomPlaneNormalVector) > CameraFieldOfView / AspectRatio)
                        screenScalar *= -1;
                    triangleCoordinatesOnScreen[i].Y = Math.Round(screenScalar * screen.Height, 0);
                }                
                screen.AddTriangleToScreen(triangleCoordinatesOnScreen);
            }
        }

        #region Temporary functions

        /// <summary>
        /// Temporary function to test the camera.
        /// </summary>
        private void CreateTestTriangles()
        {
            objects.Add(new Object3D("3DObjects\\Cube.obv"));

            int triangleCount = 0;
            foreach (Object3D obj in objects)
            {
                triangleCount += obj.verticiesFaceIndex.Length;
            }
            triangles = new Triangle3D[triangleCount];

            foreach (Object3D obj in objects)
            {
                for (int i = 0; i < obj.verticiesFaceIndex.Length; i++)
                {
                    triangles[i] = new Triangle3D(
                        obj.vertices[obj.verticiesFaceIndex[i][0]],
                        obj.vertices[obj.verticiesFaceIndex[i][1]],
                        obj.vertices[obj.verticiesFaceIndex[i][2]]
                    );
                }
            }

        }

        /// <summary>
        /// Temporary function to make the direction vector readable to the mainwindow.
        /// </summary>
        /// <returns></returns>
        public double[] ConvertCameraDirectionVector()
        {
            double[] coordinates = new double[3];
            coordinates[0] = Math.Round(cameraDirectionUnitVector.X, 3);
            coordinates[1] = Math.Round(cameraDirectionUnitVector.Y, 3);
            coordinates[2] = Math.Round(cameraDirectionUnitVector.Z, 3);
            return coordinates;
        }

        #endregion

        #region Camera movement

        /// <summary>
        /// Moves the camera along an axis.
        /// </summary>
        /// <param name="direction">Direction of movement.</param>
        /// <param name="distance">Distance of movement.</param>
        public void MoveOnAxis(char direction, double distance = 0.1)
        {
            Vect3D CurrentPosition = new Vect3D(CameraPosition);
            switch (direction)
            {
                case 'r':
                    CameraPosition = (CurrentPosition + cameraHorizontalUnitVector * distance).ToPoint();
                    break;
                case 'l':
                    CameraPosition = (CurrentPosition - cameraHorizontalUnitVector * distance).ToPoint();
                    break;
                case 'u':
                    CameraPosition = (CurrentPosition + cameraVerticalUnitVector * distance).ToPoint();
                    break;
                case 'd':
                    CameraPosition = (CurrentPosition - cameraVerticalUnitVector * distance).ToPoint();
                    break;
                case 'f':
                    CameraPosition = (CurrentPosition + cameraDirectionUnitVector * distance).ToPoint();
                    break;
                case 'b':
                    CameraPosition = (CurrentPosition - cameraDirectionUnitVector * distance).ToPoint();
                    break;
            }

            DetermineTrianglesInRange();
            ProjectToScreen();
        }

        /// <summary>
        /// Rotates the camera around an axis.
        /// </summary>
        /// <param name="axis">Rotationaxis</param>
        /// <param name="angle">Angle of rotation in degrees.</param>
        public void RotateAroundAxis(char axis, double angle = 1.5)
        {
            angle /= Relation3D.radianToDegreeConst;
            Vect3D rotationAxis = new Vect3D();
            switch (axis)
            {
                case 'x':
                    rotationAxis = cameraHorizontalUnitVector;
                    break;
                case 'y':
                    rotationAxis = cameraDirectionUnitVector;
                    break;
                case 'z':
                    rotationAxis = cameraVerticalUnitVector;
                    break;
            }
            Matrix rotationMatrix = Matrix.CreateRotationMatrix(rotationAxis, angle);

            cameraDirectionUnitVector = (rotationMatrix * new Matrix(cameraDirectionUnitVector)).ToVector();
            cameraVerticalUnitVector = (rotationMatrix * new Matrix(cameraVerticalUnitVector)).ToVector();
            cameraHorizontalUnitVector = (rotationMatrix * new Matrix(cameraHorizontalUnitVector)).ToVector();
            cameraLeftPlaneNormalVector = (rotationMatrix * new Matrix(cameraLeftPlaneNormalVector)).ToVector();
            cameraRightPlaneNormalVector = (rotationMatrix * new Matrix(cameraRightPlaneNormalVector)).ToVector();
            cameraTopPlaneNormalVector = (rotationMatrix * new Matrix(cameraTopPlaneNormalVector)).ToVector();
            cameraBottomPlaneNormalVector = (rotationMatrix * new Matrix(cameraBottomPlaneNormalVector)).ToVector();

            DetermineTrianglesInRange();
            ProjectToScreen();
        }

        /// <summary>
        /// Resets the camera's position and direction.
        /// </summary>
        public void Reset()
        {
            CameraPosition = new Point3D(1, -10, 1);
            cameraDirectionUnitVector = new Vect3D(0, 1, 0);
            cameraVerticalUnitVector = new Vect3D(0, 0, 1);
            cameraHorizontalUnitVector = new Vect3D(1, 0, 0);
            CreatePlaneNormalVectors();
            DetermineTrianglesInRange();
            ProjectToScreen();
        }

        #endregion

        #endregion
    }
}
