using SpaceObjectVisualization.Tools;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace SpaceObjectVisualization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables

        /// <summary>
        /// The camera which contains all data and logic for the interaction with the three dimensional space.
        /// </summary>
        readonly Camera camera;

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            ScreenCanvas.ClipToBounds = true;
            camera = new Camera((int)ScreenCanvas.Width, (int)ScreenCanvas.Height);
            DrawOnCanvas();
            UpdateInformation();
        }

        #endregion

        #region Button functions

        /// <summary>
        /// Asynchronously moves the camera into a direction specified by the tag of the sending button.
        /// The action is performed in 10 seperate steps to visualize the camera's movement.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MoveBTN_Click(object sender, RoutedEventArgs e)
        {
            char tag = ((Button)sender).Tag.ToString()[0];
            for (int i = 0; i < 10; i++)
            {
                camera.MoveOnAxis(tag);
                UpdateInformation();
                DrawOnCanvas();
                await Task.Delay(200);
            }
        }

        /// <summary>
        /// Asynchronously rotates the camera around an axis specified by the tag of the sending button.
        /// The action is performed in 10 seperate steps to visualize the camera's movement.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void RotateBTN_Click(object sender, RoutedEventArgs e)
        {
            string tag = ((Button)sender).Tag.ToString();
            double rotationDirection = tag[1] == 'r' ? -1.5 : 1.5;
            for (int i = 0; i < 10; i++)
            {
                camera.RotateAroundAxis(tag[0], rotationDirection);
                UpdateInformation();
                DrawOnCanvas();
                await Task.Delay(200);
            }
        }

        /// <summary>
        /// Resets the camera's position and rotation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetBTN_Click(object sender, RoutedEventArgs e)
        {
            camera.Reset();
            UpdateInformation();
            DrawOnCanvas();
        }

        #endregion

        #region Functions

        /// <summary>
        /// Updates the information of the camera's current position and rotation visible on the screen.
        /// </summary>
        private void UpdateInformation()
        {
            double[] dirVect = camera.ConvertCameraDirectionVector();
            dirXTB.Text = dirVect[0].ToString();
            dirYTB.Text = dirVect[1].ToString();
            dirZTB.Text = dirVect[2].ToString();
            posXTB.Text = camera.CameraPosition.X.ToString();
            posYTB.Text = camera.CameraPosition.Y.ToString();
            posZTB.Text = camera.CameraPosition.Z.ToString();
            tCount.Text = camera.trianglesOnScreenIndex.Length.ToString();
        }

        /// <summary>
        /// Draws all trianlgles from the camera onto the canvas.
        /// </summary>
        private void DrawOnCanvas()
        {
            ScreenCanvas.Children.Clear();
            foreach (Polygon poly in camera.screen.Draw())
            {
                ScreenCanvas.Children.Add(poly);
            }
        }

        #endregion
    }
}
