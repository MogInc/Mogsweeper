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
    /// Interaction logic for Header.xaml
    /// </summary>
    public partial class Header : Page {
        public Header() {
            InitializeComponent();
        }

        private void mniExtraAbout_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show("Property of Mog Inc.", "Mog Inc.", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
