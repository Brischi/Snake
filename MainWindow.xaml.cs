using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SnakeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Rectangle food;
        private Ellipse snakeHead;
        private int SnakeCurrentRow;
        private int SnakeCurrentColumn;
        private int FoodCurrentRow;
        private int FoodCurrentColumn;

        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
            InitializeGameGrid();
            InitializeGameElements();
            this.KeyDown += SnakeHeadMovement;
        }

        private void InitializeGameGrid()
        {
            //Define the number of rows and columns
            //Grid-Gitter
            int rows = 22; //Zeilen 
            int columns = 40; //Spalten

            //Add columns to the grid
            for (int i = 0; i < columns; i++)
            {
                GameArea.ColumnDefinitions.Add(new ColumnDefinition());
            }

            //Add rows to the grid
            for (int i = 0;i < rows; i++)
            {
                GameArea.RowDefinitions.Add(new RowDefinition());
            }
        }

        private void InitializeGameElements()
        {
            //Initialize the snake head

            snakeHead = new Ellipse
            {
                Width = 20,
                Height = 20,
                Fill = Brushes.Green
            };
            PositionElementInGrid(snakeHead, 10, 20);   //Start at row 10, column 20
            UpdateSnakePosition();
            GameArea.Children.Add(snakeHead);       //Add the snake head to the grid

            //Initialize the food 
            food = new Rectangle
            {
                Width = 20,
                Height = 20,
                Fill = Brushes.Red
            };
            PositionElementInGrid(food, 5, 15);        //Start at row 5, column 15
            GameArea.Children.Add(food);        //Add the food to the grid
            UpdateFoodPosition();
        }

        private void PositionElementInGrid(UIElement element,int  row, int column)
        {
            Grid.SetRow(element, row);
            row = SnakeCurrentRow;
            row = FoodCurrentRow;
            Grid.SetColumn(element, column);
            column = SnakeCurrentColumn;
            column = FoodCurrentColumn;
        }

        private void UpdateSnakePosition()
        {
          PositionElementInGrid(snakeHead, SnakeCurrentRow, SnakeCurrentColumn);

        }
        private void UpdateFoodPosition()
        { 
            
            PositionElementInGrid(food, FoodCurrentRow, FoodCurrentColumn);
        }
        private void SnakeHeadMovement(object sender, KeyEventArgs e)
        {

            switch (e.Key)
            {
                case Key.Up:
                    SnakeCurrentRow -= 1;
                    UpdateSnakePosition();
                    
                    break;
                case Key.Down:
                    SnakeCurrentRow += 1;
                    UpdateSnakePosition();
                   
                    break;
                case Key.Left:
                    SnakeCurrentColumn -= 1;
                    UpdateSnakePosition();
                    
                    break;
                case Key.Right:
                    SnakeCurrentColumn += 1;
                    UpdateSnakePosition();
                   
                    break;
            }
        }
        /*
        private void BorderCollisionDetection()
        {
            int rows = 22;
            int columns = 40;
            

            if (SnakeCurrentColumn < 0 ||  SnakeCurrentColumn > columns) //Checkt nach horizontaler Kollision
            {
                return;
            }
            else if (SnakeCurrentRow < 0 || SnakeCurrentRow >= rows)    //Checkt nach vertikaler Kollision
            {
                Close();
            }
            else
            {
                return;
            }
            */
            

        }
    }
}