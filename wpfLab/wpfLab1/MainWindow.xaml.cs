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
using wpfLab1;
namespace wpfLab1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnHello_Click(object sender, RoutedEventArgs e)
        {
            string s = "hEllo   wOrLd,  hi,   world ,   aa dd  dw   WWdd a ";

            lblHello.Content = s.GetNormalFormat();
            stkDiv.Width = this.Width;
            stkDiv.Height = this.Height;
            
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            stkDiv.Width = 0;
            stkDiv.Height = 0;
        }
    }
}
