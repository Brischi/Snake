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
    }
}