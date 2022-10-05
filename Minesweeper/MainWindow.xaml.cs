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


namespace Minesweeper {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private int gridSize = 2;
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
                    button.Name = $"a{i}e{j}";
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

        private void ClearArea(int x, int y) {
            var xBoxMin = x <= 0 ? 0 : 1;
            var xBoxMax = x >= gridSize-1 ? 0 : 1;
            var yBoxMin = y <= 0 ? 0 : 1;
            var yBoxMax = y >= gridSize-1 ? 0 : 1;
            for (int i = x - xBoxMin; i <= x + xBoxMax; i++) {
                for (int j = y - yBoxMin; j <= y + yBoxMax; j++) {
                    //MessageBox.Show($"{i} {j}");
                    //now how the fuck do i disable them
                    var myTextBlock = MainGrid.Children.OfType<Button>;
                    MessageBox.Show(myTextBlock.ToString());

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
            var coord = clicked.Name.Substring(1).Split('e').Select(int.Parse).ToList();
            if (gridMines[coord[0],coord[1]].Equals('m')) {
                EndGame(0);
            }
            gridMines[coord[0], coord[1]] = 'c';

            if (CheckIfMinesLeft()) {
                EndGame(1);
            }

            
            clicked.Content = "Clicked";
            clicked.IsEnabled = false;
            ClearArea(coord[0], coord[1]);


            //MessageBox.Show($"x: {coord[0]} | y: {coord[1]}");
        }
    }
}
