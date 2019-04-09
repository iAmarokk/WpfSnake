using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfSnake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int size = 16;
        DispatcherTimer timer;
        Random random;
        Point food;
        Point snake;
        int width;
        int height;
        int stepX;
        int stepY;
        int totals = 100;
        int points = 0;
        int speed = 1;
        int length = 1;
        int countEat = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            random = new Random();
            width = (int)CanvasMap.ActualWidth;
            height = (int)CanvasMap.ActualHeight;
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(200000);
            timer.IsEnabled = true;

            KeyDown += MainWindow_KeyDown;
            AddFood();
            snake = new Point(width / 2, height / 2);
            stepX = 0;
            stepY = 0;
            MoveSnake();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Down: stepX = 0; stepY = +speed; break;
                case Key.Up: stepX = 0; stepY = -speed; break;
                case Key.Left: stepX = -speed; stepY = 0; break;
                case Key.Right: stepX = +speed; stepY = 0; break;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            MoveSnake();
            if (OutOfScreen(snake))
                GameOver();
            if (IsCrossedSelf())
                GameOver();
            if (IsCrossed(snake, food))
                if (++points == totals)
                    GameWin();
            else
                {
                    AddFood();
                    countEat++;
                    Title = "Шариков до победы " + (totals - countEat).ToString();
                    //speed++;
                    //size = size + size / 2;
                }
        }

        bool IsCrossedSelf()
        {
            for(int j = 10; j < CanvasMap.Children.Count; j++)
            {
                Ellipse ellipse = (Ellipse)CanvasMap.Children[j];
                Point point = (Point)ellipse.Tag;
                if (IsCrossed(snake, point))
                    return true;
            }
            return false;
        }

        void MoveSnake()
        {
            snake.X += stepX * size / 3;
            snake.Y += stepY * size / 3;
            Ellipse ellipse = CreateEllipse(snake, Brushes.Violet);
            ellipse.Tag = snake;
            if (CanvasMap.Children.Count > length)
                CanvasMap.Children.RemoveAt(length);
            CanvasMap.Children.Insert(1, ellipse);
        }

        void AddFood()
        {
            food = new Point(random.Next(width - size * 2), random.Next(height - size * 2));
            Ellipse ellipse = CreateEllipse(food, Brushes.Salmon);
            if (CanvasMap.Children.Count > 0)
                CanvasMap.Children.RemoveAt(0);
            CanvasMap.Children.Insert(0, ellipse);
            length++;
        }

        private Ellipse CreateEllipse(Point point, Brush brush)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Width = size;
            ellipse.Height = size;
            ellipse.Fill = brush;
            Canvas.SetLeft(ellipse, point.X);
            Canvas.SetTop(ellipse, point.Y);
            return ellipse;
        }

        bool IsCrossed(Point A, Point B)
        {
            return Math.Abs(A.X - B.X) < size &&
                   Math.Abs(A.Y - B.Y) < size;
        }

        bool OutOfScreen (Point A)
        {
            //return false;
            return A.X <= 0 || A.X >= width - size ||
                   A.Y <= 0 || A.Y >= height - size;
        }

        void GameOver()
        {
            MessageBox.Show("WASTED");
            Close();
        }

        void GameWin()
        {
            MessageBox.Show("YOU WIN!");
            Close();
        }
    }
}
