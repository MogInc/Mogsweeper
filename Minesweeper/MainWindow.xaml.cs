﻿using System;
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


namespace Minesweeper {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private int gridSize = 9;
        private int mineAmount = 10;
        private bool[,] gridMines;
        public MainWindow() {
            InitializeComponent();
            MakeGrid();
            CalculateMines();
        }
        private void MakeGrid() {
            for (int i = 0; i < gridSize; i++) {
                var column = new ColumnDefinition();
                column.Width = new GridLength(1, GridUnitType.Star);
                MainGrid.ColumnDefinitions.Add(column);

                var row = new RowDefinition();
                row.Height = new GridLength(1, GridUnitType.Star);
                MainGrid.RowDefinitions.Add(row);
                for (int j = 0; j < gridSize; j++) {
                    var button = new Button();
                    button.Content = "Snide Inc.";
                    button.Name = $"a{i}e{j}";
                    button.Click += new RoutedEventHandler(buttonClick);
                    Grid.SetColumn(button, i);
                    Grid.SetRow(button, j);
                    MainGrid.Children.Add(button);
                }
            }    
        }
        private void CalculateMines() {
            gridMines = new bool[gridSize, gridSize];
            Random random = new Random();
            for (int i = 0; i < mineAmount; i++) {
                var x = random.Next(gridSize);
                var y = random.Next(gridSize);
                if (gridMines[x, y]) {
                    i--;
                    continue;
                } else {
                    gridMines[x, y] = true;
                }
            }
            MessageBox.Show("");
        }

        private void buttonClick(object sender, EventArgs e) {
            //do something or...
            Button clicked = (Button)sender;
            var coord = clicked.Name.Substring(1).Split('e').Select(int.Parse).ToList();



            MessageBox.Show($"x: {coord[0]} | y: {coord[1]}");
        }
    }
}
