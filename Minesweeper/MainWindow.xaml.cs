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


namespace Minesweeper {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private int gridSize = 9;
        public MainWindow() {
            InitializeComponent();
            test();
        }
        private void test() {
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
                    Grid.SetColumn(button, i);
                    Grid.SetRow(button, j);
                    MainGrid.Children.Add(button);
                }
            }
        }
    }
}
