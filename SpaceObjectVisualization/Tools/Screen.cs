using Space;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SpaceObjectVisualization.Tools
{
    /// <summary>
    /// 
    /// Provides functions to convert points of triangles into polygons for a canvas
    /// </summary>
    class Screen
    {    
        #region Variables

        /// <summary>
        /// Width of the screen in pixels.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Height of the screen in pixels.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// A list containing the trianlges on the screen as point arrays of length 3.
        /// </summary>
        private readonly List<Point[]> trianglesOnScreen = new List<Point[]>();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a screen by defining its width and its height directly.
        /// </summary>
        /// <param name="width">Width of the screen.</param>
        /// <param name="height">Height of the screen.</param>
        public Screen(int width, int height)
        {
            Width = width;
            Height = height;            
        }

        #endregion

        #region Functions

        /// <summary>
        /// Takes all triangles on the screen and turns them into polygons that can be drawn onto a canvas.
        /// </summary>
        /// <returns></returns>
        public Polygon[] Draw()
        {
            Polygon[] shapes = new Polygon[trianglesOnScreen.Count];
            SolidColorBrush SCB = new SolidColorBrush { Color = Color.FromArgb(255, 100, 100, 100) };
            SolidColorBrush outline = new SolidColorBrush { Color = Color.FromArgb(255, 0, 0, 0) };
            for (int i = 0; i < trianglesOnScreen.Count; i++)
            {
                shapes[i] = new Polygon();
                shapes[i].Points = new PointCollection(trianglesOnScreen[i]);
                shapes[i].Stroke = outline;
                shapes[i].Fill = SCB;
                shapes[i].StrokeThickness = 0.9;
            }
            return shapes;
        }

        /// <summary>
        /// Adds a two dimensional triangle to the screen using its coordinates.
        /// </summary>
        /// <param name="triangle">An array containing three coordinates.</param>
        public void AddTriangleToScreen(Point[] triangle)
        {
            if(triangle.Length == 3) trianglesOnScreen.Add(triangle);
        }

        /// <summary>
        /// Deletes all triangles that are currently on the screen.
        /// </summary>
        public void ClearTriangles()
        {
            trianglesOnScreen.Clear();
        }

        #endregion
    }
}
