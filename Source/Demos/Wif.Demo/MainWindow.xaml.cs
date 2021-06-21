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
using Frontier.Wif.Utilities.Helpers;

namespace Wif.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = new MainViewModel();
        }

        private void CreateDirectory_OnClick(object sender, RoutedEventArgs e)
        {
            var file = @"D:\Users\leoli\Documents\MPSExport\test_file_dir\test_file.txt";
            var dir = @"D:\Users\leoli\Documents\MPSExport\test_dir";
            FileHelper.CreateDirectory(file);
            FileHelper.CreateDirectory(dir);
        }
    }
}
