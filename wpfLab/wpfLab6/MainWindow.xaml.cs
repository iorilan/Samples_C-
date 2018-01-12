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
using Microsoft.Win32;
using System.IO;


namespace wpfLab6
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.InitPhoto();
        }
        public List<Photo> photos = new List<Photo>();

        private void InitPhoto()
        {

            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.ShowDialog();
            string rootPath = fbd.SelectedPath;
            //MessageBox.Show(rootPath);
            GetAllImagePath(rootPath);
            lstImgs.ItemsSource = photos;

        }
        public void GetAllImagePath(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] files = di.GetFiles("*.*", SearchOption.AllDirectories);

            if (files != null && files.Length > 0)
            {
                foreach (var file in files)
                {
                    if (file.Extension==(".jpg") ||
                        file.Extension == (".png") ||
                        file.Extension == (".bmp") ||
                        file.Extension == (".gif"))
                    {
                        photos.Add(new Photo()
                        {
                            FullPath = file.FullName
                        });
                    }
                }
            }
        }
    }
}
