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
        public MainWindow()
        {
            InitializeComponent();
            InitializeGameGrid();
            InitializeGameElements();
            
        }

        private void InitializeGameGrid()
        {
            //Define the number of rows and columns
            //Grid-Gitter
            int rows = 22; //Zeilen 
            int columns = 40; //Spalten

            //Add columns to the grid 
            for (int i = 0; i < columns; i++) //für die GridCollum-Definition
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

            SnakeHead = new Ellipse
            {
                Width = 20,
                Height = 20,
                Fill = Brushes.Green
            };
            
            //Initialize the food 
        }

        private void SnakeHeadMovement (object sender, KeyEventArgs e)
        {
           // this.Focus(); --> stellt sicher, dass dieses Fenster fokussiert wird bei Snakeheadmovement. vll brauchen wir das auch nicht
            switch (e.Key)
            {
                case Key.Left:
              
                    //hier: SnakeHead.Position....rows -=1;
                    PositionElementInGrid(snakeHead, "", "-1"); //erst rows, dann collum
                    break;
                case Key.Right:
                    // ... rows +=1;
                    PositionElementInGrid(snakeHead, "", "+1");
                    break;
                case Key.Up:
                    PositionElementInGrid(snakeHead, "-1", "");
                    //collums -=1;
                    break;
                case Key.Down:
                    //collums +=1;
                    PositionElementInGrid(snakeHead, "+1", "");
                 
                    break;
            }
            e.Handled = true; 
        }
    }
}