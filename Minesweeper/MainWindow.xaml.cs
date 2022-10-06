using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Minesweeper {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private int gridSize = 10;
        private int mineAmount = 10;
        private bool gameStarted = false;
        //gridMines
        //m - mine
        //c - clear
        // - default
        private char[,] gridMines;
        private Button[,] buttons;

        public MainWindow() {
            InitializeComponent();
            StartGame();
        }
        private void MakeGrid() {
            for (int i = 0; i < gridSize; i++) {
                var column = new ColumnDefinition();
                column.Width = new GridLength(1, GridUnitType.Star);
                MainGrid.ColumnDefinitions.Add(column);

                var row = new RowDefinition();
                row.Height = new GridLength(1, GridUnitType.Star);
                MainGrid.RowDefinitions.Add(row);
            }    
        }
        private void AddButtons() {
            buttons = new Button[gridSize, gridSize];

            for (int i = 0; i < gridSize; i++) {
                for (int j = 0; j < gridSize; j++) {
                    var button = new Button();                    
                    button.Name = $"a{i}e{j}";
                    button.Click += new RoutedEventHandler(buttonClick);
                    button.HorizontalContentAlignment = HorizontalAlignment.Center;
                    button.VerticalContentAlignment = VerticalAlignment.Center;
                    OverrideButtonStyle(button);
                    //TODO rightclick to plant flag
                    //TODO prevent where first click is bomb
                    Grid.SetColumn(button, i);
                    Grid.SetRow(button, j);
                    MainGrid.Children.Add(button);
                    buttons[i, j] = button;
                }
            }
        }
        private void CalculateMines() {
            gridMines = new char[gridSize, gridSize];
            Random random = new Random();
            for (int i = 0; i < mineAmount; i++) {
                var x = random.Next(gridSize);
                var y = random.Next(gridSize);
                if (gridMines[x, y].Equals('m')) {
                    i--;
                    continue;
                } else {
                    gridMines[x, y] = 'm';
                }
            }
        }

        private void OverrideButtonStyle(Button button) {
            ParserContext context = new ParserContext();
            context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
            context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");

            string template =
                "<Style x:Key=\"SomeButtonStyle\" TargetType=\"Button\">" +
                    "<Setter Property=\"Background\" Value=\"LightGray\" />" +
                    "<Setter Property=\"Margin\" Value=\"2\"/>" +
                    "<Setter Property=\"Template\">" +
                        "<Setter.Value>" +
                            "<ControlTemplate TargetType=\"Button\">" +
                                "<Grid Background=\"{TemplateBinding Background}\">" +
                                    "<ContentPresenter />" +
                                "</Grid>" +
                            "</ControlTemplate>" +
                        "</Setter.Value>" +
                    "</Setter>" +
                "</Style>";
           
            button.Style = (Style)XamlReader.Parse(template, context);
        }

        private void ClearArea(int x, int y) {
            Button button = buttons[x, y];

            button.Click -= new RoutedEventHandler(buttonClick);
            button.Background = Brushes.DarkGray;

            var xMin = x <= 0 ? 0 : x - 1;
            var xMax = x >= gridSize - 1 ? gridSize - 1 : x + 1;
            var yMin = y <= 0 ? 0 : y - 1;
            var yMax = y >= gridSize - 1 ? gridSize - 1 : y + 1;

            var surroundingMines = 0;
            List<int[]> clearNeighborSquares = new List<int[]>();

            for (int i = xMin; i <= xMax; i++)
            {
                for (int j = yMin; j <= yMax; j++)
                {
                    if (i == x && y == j) continue;

                    if (gridMines[i, j] == 'm') surroundingMines++;
                    else if (gridMines[i, j] == 'c') continue;
                    else clearNeighborSquares.Add(new int[] { i, j });
                }
            }

            gridMines[x, y] = 'c';

            if (surroundingMines > 0)
            {
                button.Content = surroundingMines.ToString();
            }
            else
            {
                foreach (int[] coords in clearNeighborSquares)
                {
                    ClearArea(coords[0], coords[1]);
                }
            }
        }

        private void StartGame() {
            MakeGrid();
            AddButtons();
            CalculateMines();
        }
        private void Restart() {
            MainGrid.Children.Clear();
            AddButtons();
            CalculateMines();
            gameStarted = false;
        }
        private void EndGame(int gId) {
            switch (gId) {
                case 0:
                    MessageBox.Show("EH OH");
                    break;
                case 1:
                    MessageBox.Show("Gz u won");
                    break;
            }
            Restart();
        }
        private bool CheckIfMinesLeft() {
            foreach (var item in gridMines) {               
                if (!item.Equals('c') && !item.Equals('m')) {
                    return false;
                }
            }
            return true;
        }

        private void MoveMine(int x, int y) {
            bool mineMoved = false;
            Random random = new Random();

            while (!mineMoved)
            {
                var randX = random.Next(gridSize);
                var randY = random.Next(gridSize);

                if (gridMines[randX, randY] == '\0')
                {
                    gridMines[randX, randY] = 'm';
                    mineMoved = true;
                }
        }
    }

        private void buttonClick(object sender, EventArgs e) {
            Button clicked = (Button)sender;
            var coord = clicked.Name.Substring(1).Split('e').Select(int.Parse).ToList();
            if (gridMines[coord[0],coord[1]].Equals('m')) {
                if (!gameStarted)
                {
                    MoveMine(coord[0], coord[1]);
                    gameStarted = true;
                }
                else
                {
                    EndGame(0);
                }               
            }            

            if (CheckIfMinesLeft()) {
                EndGame(1);
            }

            ClearArea(coord[0], coord[1]);
        }
    }
}
