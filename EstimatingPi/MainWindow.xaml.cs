using System;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Media;
using ThinkLib;

namespace EstimatingPi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Turtle mike;
        Random rng;

        double canvasHeight;
        double canvasWidth;

        double radius ;

        Point center;

        bool stop = false;

        public MainWindow()
        {
            InitializeComponent();

            mike = new Turtle(NinjaMutantCanvas,0,0);

            mike.Visible = false;

            rng = new Random();

            // Assuming canvas is a Aquare
            canvasHeight = NinjaMutantCanvas.Height;
            canvasWidth = NinjaMutantCanvas.Width;

            radius = canvasWidth / 2;

            // Center of radius
            center = new Point(radius, radius);

            // Draw circle in canvas
            // Uncomment to draw a circle on the canvas
            //DrawCircle(center, radius, 10);


        }

        /// <summary>
        /// Draw a circle that fits the square canvas
        /// </summary>
        /// <param name="center"> Center of the canvas</param>
        /// <param name="radius"> Distance from canvas center to edge</param>
        /// <param name="brushWidth"> How thick brush should be</param>
        void DrawCircle(Point center, double radius, double brushWidth = 1)
        {
            mike.WarpTo(center);
            mike.BatchSize = 10;
            mike.BrushWidth = brushWidth;
            for (int i = 0; i < 360; i++)
            {
                mike.BrushDown = false;
                mike.Forward(radius - 1);
                mike.BrushDown = true;
                mike.Forward(1);
                mike.WarpTo(center);
                mike.Left(1);

            }
            mike.BatchSize = 1;

        } // DrawCircle


        /// <summary>
        /// Draw Squares representing rain drops outside of the circle
        /// </summary>
        /// <param name="location"> Where to draw the sqaure</param>
        /// <param name="size"> Size of the square</param>
        public void DrawSquare(Point location, double size)
        {
            mike.LineBrush = Brushes.Yellow;
            mike.BrushWidth = 2;
            mike.WarpTo(location.X - size / 2, location.Y + size / 2);
            for(int i = 0; i < 4; i++)
            {
                mike.Forward(size);
                mike.Left(90);
            }
            mike.BrushWidth = 1;
        } // DrawSquare

        /// <summary>
        /// Draw Trianges representing rain drops inside of the circle
        /// </summary>
        /// <param name="location"> Where to draw the sqaure</param>
        /// <param name="size"> Size of the square</param>
        public void DrawTriangle(Point location, double size)
        {
            mike.LineBrush = Brushes.Green;
            mike.Heading = 0;
            mike.BrushWidth = 2;
            mike.WarpTo(location.X - size / 2, location.Y + size / 2);
            for (int i = 0; i < 3; i++)
            {
                mike.Forward(size);
                mike.Left(120);
            }
            mike.BrushWidth = 1;
            mike.Heading = 0;
        } // DrawTriangle

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            int totalDrops = 0;
            int dropsInCircle = 0;

            mike.BatchSize = 100;


            string raindrops = tbNumberOfRaindrops.Text.ToString().Trim();

            try
            {
                int numOfRaindrops = Convert.ToInt32(raindrops);

                while (totalDrops < numOfRaindrops)
                {
                    // Choose random point in canvas
                    double x = rng.NextDouble() * canvasWidth;
                    double y = rng.NextDouble() * canvasHeight;

                    // distance between point and center of circle
                    double distance = Math.Sqrt(Math.Pow(x - center.X, 2) + Math.Pow(y - center.Y, 2));


                    if (distance < radius)
                    // point inside of circle
                    {
                        DrawTriangle(new Point(x, y), 5);
                        dropsInCircle++;
                    }
                    else
                    // point outside of circle
                    {
                        DrawSquare(new Point(x, y), 5);
                    }
                    totalDrops++;

                    // pi = prob(rain drops in circle) * 4
                    double piEstimate = Math.Round(((double)dropsInCircle / (double)totalDrops) * 4, 5);

                    lblResult.Content = $"{totalDrops} drops: {dropsInCircle} landed in circle.\nPi estimate is {piEstimate}";

                    if (stop)
                    {
                        break;
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            stop = true;
        }
    }
}
