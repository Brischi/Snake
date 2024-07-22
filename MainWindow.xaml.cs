using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SnakeGame
{
    public partial class MainWindow : Window
    {
        private Rectangle food;
        private Ellipse snakeHead;
        private List<Ellipse> snakeBody;
        private int SnakeCurrentRow;
        private int SnakeCurrentColumn;
        private int FoodCurrentRow;
        private int FoodCurrentColumn;
        private Key currentDirection = Key.Right; // Initial direction
        private DispatcherTimer gameTimer;
        private DispatcherTimer scoreTimer;
        private int score;
        private int secondsElapsed;
        private int lives;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGameGrid();
            InitializeGameElements();
            this.KeyDown += SnakeHeadMovement;

            // Initialize the game timer
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(200); // Adjusted interval to slow down the snake
            gameTimer.Tick += GameLoop;

            // Initialize the score timer
            scoreTimer = new DispatcherTimer();
            scoreTimer.Interval = TimeSpan.FromSeconds(1); // Update timer every second
            scoreTimer.Tick += ScoreTimer_Tick;

            StartGame();
        }

        private void StartGame()
        {
            score = 0;
            secondsElapsed = 0;
            lives = 3;
            UpdateScore();
            UpdateTimer();
            UpdateHeartsDisplay();
            gameTimer.Start();
            scoreTimer.Start();
        }

        private void ScoreTimer_Tick(object sender, EventArgs e)
        {
            secondsElapsed++;
            UpdateTimer();
        }

        private void UpdateTimer()
        {
            int minutes = secondsElapsed / 60;
            int seconds = secondsElapsed % 60;
            TimerTextBlock.Text = $"Timer: {minutes:D2}:{seconds:D2}";
        }

        private void UpdateScore()
        {
            PointsTextBlock.Text = $"Points: {score}";
        }

        private void UpdateHeartsDisplay()
        {
            switch (lives)
            {
                case 3:
                    HeartsImage.Source = new BitmapImage(new Uri("pack://application:,,,/HeartStatus_3.png"));
                    break;
                case 2:
                    HeartsImage.Source = new BitmapImage(new Uri("pack://application:,,,/HeartStatus_2.png"));
                    break;
                case 1:
                    HeartsImage.Source = new BitmapImage(new Uri("pack://application:,,,/HeartStatus_1.png"));
                    break;
                case 0:
                    HeartsImage.Source = new BitmapImage(new Uri("pack://application:,,,/HeartStatus_0.png"));
                    break;
            }
        }

        private void ShowOuchMessage()
        {
            OuchTextBlock.Visibility = Visibility.Visible;
            OuchTextBlock.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            OuchTextBlock.Arrange(new Rect(OuchTextBlock.DesiredSize));

            double left = (GameCanvas.ActualWidth - OuchTextBlock.ActualWidth) / 2;
            double top = (GameCanvas.ActualHeight - OuchTextBlock.ActualHeight) / 2;

            Canvas.SetLeft(OuchTextBlock, left);
            Canvas.SetTop(OuchTextBlock, top);

            // Hide the "Ouch!" message after a short delay
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += (sender, args) =>
            {
                OuchTextBlock.Visibility = Visibility.Collapsed;
                timer.Stop();
            };
            timer.Start();
        }




        private void InitializeGameGrid()
        {
            int rows = 22;
            int columns = 40;

            GameArea.ColumnDefinitions.Clear();
            GameArea.RowDefinitions.Clear();

            for (int i = 0; i < columns; i++)
            {
                GameArea.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < rows; i++)
            {
                GameArea.RowDefinitions.Add(new RowDefinition());
            }
        }

        private void InitializeGameElements()
        {
            snakeHead = new Ellipse
            {
                Width = 20,
                Height = 20,
                Fill = Brushes.Green
            };
            PositionElementInGrid(snakeHead, 10, 20);
            SnakeCurrentRow = 10;
            SnakeCurrentColumn = 20;
            GameArea.Children.Add(snakeHead);

            food = new Rectangle
            {
                Width = 20,
                Height = 20,
                Fill = Brushes.Red
            };
            PositionElementInGrid(food, 5, 15);
            FoodCurrentRow = 5;
            FoodCurrentColumn = 15;
            GameArea.Children.Add(food);

            snakeBody = new List<Ellipse>();

            Ellipse initialBodyPart = new Ellipse
            {
                Width = 20,
                Height = 20,
                Fill = Brushes.Green
            };
            snakeBody.Add(initialBodyPart);
            GameArea.Children.Add(initialBodyPart);
            PositionElementInGrid(initialBodyPart, SnakeCurrentRow + 1, SnakeCurrentColumn); // Position below the head
        }

        private void AddSnakeBodyPart()
        {
            Ellipse bodyPart = new Ellipse
            {
                Width = 20,
                Height = 20,
                Fill = Brushes.Green
            };

            int tailRow = Grid.GetRow(snakeBody[snakeBody.Count - 1]);
            int tailColumn = Grid.GetColumn(snakeBody[snakeBody.Count - 1]);

            snakeBody.Add(bodyPart);
            GameArea.Children.Add(bodyPart);
            PositionElementInGrid(bodyPart, tailRow, tailColumn); // Start new body part at the current position of the tail
        }

        private void PositionElementInGrid(UIElement element, int row, int column)
        {
            Grid.SetRow(element, row);
            Grid.SetColumn(element, column);
        }

        private void UpdateSnakePosition()
        {
            if (SnakeCurrentRow < 0 || SnakeCurrentRow >= GameArea.RowDefinitions.Count ||
                SnakeCurrentColumn < 0 || SnakeCurrentColumn >= GameArea.ColumnDefinitions.Count)
            {
                HandleCollision(true); // Pass true if it's a border collision
                return;
            }

            foreach (Ellipse bodyPart in snakeBody)
            {
                if (SnakeCurrentRow == Grid.GetRow(bodyPart) && SnakeCurrentColumn == Grid.GetColumn(bodyPart))
                {
                    HandleCollision(false); // Pass false if it's a body collision
                    return;
                }
            }

            if (SnakeCurrentRow == FoodCurrentRow && SnakeCurrentColumn == FoodCurrentColumn)
            {
                ConsumeFood();
            }

            MoveSnakeBody();
            PositionElementInGrid(snakeHead, SnakeCurrentRow, SnakeCurrentColumn);
        }

        private void HandleCollision(bool isBorderCollision)
        {
            lives--;
            UpdateHeartsDisplay();
            ShowOuchMessage();

            if (lives <= 0)
            {
                GameOver();
            }
            else
            {
                if (isBorderCollision)
                {
                    SnakeCurrentRow = 10;
                    SnakeCurrentColumn = 20;
                }
                PositionElementInGrid(snakeHead, SnakeCurrentRow, SnakeCurrentColumn);
            }
        }

        private void MoveSnakeBody()
        {
            int previousRow = SnakeCurrentRow;
            int previousColumn = SnakeCurrentColumn;

            for (int i = 0; i < snakeBody.Count; i++)
            {
                int tempRow = Grid.GetRow(snakeBody[i]);
                int tempColumn = Grid.GetColumn(snakeBody[i]);
                PositionElementInGrid(snakeBody[i], previousRow, previousColumn);
                previousRow = tempRow;
                previousColumn = tempColumn;
            }
        }

        private void UpdateFoodPosition()
        {
            PositionElementInGrid(food, FoodCurrentRow, FoodCurrentColumn);
        }

        private void SnakeHeadMovement(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Up && currentDirection != Key.Down) ||
                (e.Key == Key.Down && currentDirection != Key.Up) ||
                (e.Key == Key.Left && currentDirection != Key.Right) ||
                (e.Key == Key.Right && currentDirection != Key.Left))
            {
                currentDirection = e.Key;
            }
        }

        private void GameLoop(object sender, EventArgs e)
        {
            switch (currentDirection)
            {
                case Key.Up:
                    SnakeCurrentRow -= 1;
                    break;
                case Key.Down:
                    SnakeCurrentRow += 1;
                    break;
                case Key.Left:
                    SnakeCurrentColumn -= 1;
                    break;
                case Key.Right:
                    SnakeCurrentColumn += 1;
                    break;
            }

            UpdateSnakePosition();
        }

        private void GameOver()
        {
            gameTimer.Stop();
            scoreTimer.Stop();

            MessageBoxResult result = MessageBox.Show("Game Over!", "Game Over", MessageBoxButton.OK);
            if (result == MessageBoxResult.OK)
            {
                PlayAgainButton.Visibility = Visibility.Visible; // Show the Play again button
            }
        }

        private void ResetGame()
        {
            gameTimer.Stop();
            scoreTimer.Stop();

            GameArea.Children.Clear();
            GameArea.ColumnDefinitions.Clear();
            GameArea.RowDefinitions.Clear();
            snakeBody.Clear();

            InitializeGameGrid();
            InitializeGameElements();

            score = 0;
            secondsElapsed = 0;
            lives = 3;
            UpdateScore();
            UpdateTimer();
            UpdateHeartsDisplay();

            OuchTextBlock.Visibility = Visibility.Collapsed;

            gameTimer.Start();
            scoreTimer.Start();
        }


        private void ConsumeFood()
        {
            AddSnakeBodyPart();

            score += 25;
            UpdateScore();

            Random rnd = new Random();
            FoodCurrentRow = rnd.Next(0, GameArea.RowDefinitions.Count);
            FoodCurrentColumn = rnd.Next(0, GameArea.ColumnDefinitions.Count);
            UpdateFoodPosition();
        }

        private void PlayAgainButton_Click(object sender, RoutedEventArgs e)
        {
            PlayAgainButton.Visibility = Visibility.Collapsed; // Hide the Play again button
            ResetGame();
        }
    }
}
