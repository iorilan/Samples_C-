using System;
using System.Collections.Generic;
using System.Linq;
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

namespace wpfLab5
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        //    img2.Source = new BitmapImage(new Uri("pack://application:,,,/Images/hills.jpg"));
            img2.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Images/hills.jpg"));
            img3.Source = new BitmapImage(new Uri("/Images/hills.jpg",UriKind.Relative));
        }
    }
}
