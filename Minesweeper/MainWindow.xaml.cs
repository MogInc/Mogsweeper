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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;


namespace Minesweeper {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private int gridSize = 10;
        private int mineAmount = 1;
        //gridMines
        //m - mine
        //c - clear
        // - default
        private char[,] gridMines;
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
            for (int i = 0; i < gridSize; i++) {
                for (int j = 0; j < gridSize; j++) {
                    var button = new Button();
                    button.Content = "Snide Inc.";
                    button.Name = $"x{i}y{j}";
                    button.Click += new RoutedEventHandler(buttonClick);
                    //TODO rightclick to plant flag
                    //TODO prevent where first click is bomb
                    Grid.SetColumn(button, i);
                    Grid.SetRow(button, j);
                    MainGrid.Children.Add(button);
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

        private List<string> CalcArea(int x, int y) {
            var xBoxMin = x <= 0 ? 0 : 1;
            var xBoxMax = x >= gridSize-1 ? 0 : 1;
            var yBoxMin = y <= 0 ? 0 : 1;
            var yBoxMax = y >= gridSize-1 ? 0 : 1;
            List<string> buttonNames = new List<string>();
            for (int i = x - xBoxMin; i <= x + xBoxMax; i++) {
                for (int j = y - yBoxMin; j <= y + yBoxMax; j++) {
                    if (gridMines[i, j].Equals('m')) {
                        return new List<string>() {$"x{x}y{y}"};
                    }
                    if (gridMines[i,j] != 'c') {
                        buttonNames.Add($"x{i}y{j}");
                    }
                }
            }
            return buttonNames;
        }
        private void DecideNextChunkToBeCleared(List<string> potentialTiles) {
            var b = true;
            foreach (var item in potentialTiles) {
                var coord = item.Substring(1).Split("y").Select(int.Parse).ToList();
                for (int i = 0; i < coord.Count; i++) {
                    if (coord[i] == 0 || coord[i] >= gridSize - 1) {
                        b = false;
                        break; 
                    }
                }
                if (b) {
                    ClearArea(CalcArea(coord[0], coord[1]));
                }
            }
        }
        public void ClearArea(List<string> buttonNames) {
            var test = MainGrid.Children.OfType<Button>();           
            foreach (var button in test) {
                if (buttonNames.Contains(button.Name)) {
                    var coord = button.Name.Substring(1).Split('y').Select(int.Parse).ToList();
                    gridMines[coord[0],coord[1]] = 'c';
                    button.IsEnabled = false;
                    button.Name = "Mogged";
                }
            }
            //DecideNextChunkToBeCleared(buttonNames);
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
        private void MoveMine() { 
            
        }
        private void buttonClick(object sender, EventArgs e) {
            Button clicked = (Button)sender;
            var coord = clicked.Name.Substring(1).Split('y').Select(int.Parse).ToList();
            if (gridMines[coord[0],coord[1]].Equals('m')) {
                EndGame(0);
                return;
            }
            gridMines[coord[0], coord[1]] = 'c';
            clicked.IsEnabled = false;
            ClearArea(CalcArea(coord[0], coord[1]));
            if (CheckIfMinesLeft()) {
                EndGame(1);
            }
            //MessageBox.Show($"x: {coord[0]} | y: {coord[1]}");
        }
    }
}
