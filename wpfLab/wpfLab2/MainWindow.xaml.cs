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
using System.Threading;
using System.IO;

namespace wpfLab2
{

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.InitListView();
            this.InitTree();

            //this.InitPrograssBar();
        }


        private void InitPrograssBar()
        {
            double max = pgTest.Maximum;
            ThreadPool.QueueUserWorkItem(new WaitCallback((o) =>
            {

                int i = 0;
                while (i <= max)
                {
                    Thread.Sleep(200);
                    pgTest.Dispatcher.Invoke(new Action(() =>
                    {
                        pgTest.Value = ++i;
                    }));
                }
            }));


        }

        ////init 5 root nodes
        List<LeafNode> roots = new List<LeafNode>();
        private void InitTree()
        {
            for (int i = 0; i < 5; i++)
            {
                LeafNode child = new LeafNode() { Data = "root" + i.ToString() };
                child.Children = new List<LeafNode>();
                roots.Add(child);
                ////10 children
                for (int j = 0; j < 10; j++)
                {
                    LeafNode subChild = new LeafNode() { Data = "son" + j.ToString() };
                    subChild.Children = new List<LeafNode>();
                    ////15 children
                    child.Children.Add(subChild);
                    for (int k = 0; k < 15; k++)
                    {
                        LeafNode gs = new LeafNode() { Data = "grantSon" + k.ToString() };
                        subChild.Children.Add(gs);
                    }
                }
            }


            for (int i = 0; i < roots.Count; i++)
            {
                TreeViewItem item = new TreeViewItem();
                AddLeaf(item, roots[i]);
                twLeaf.Items.Add(item);
            }

        }

        private void AddLeaf(TreeViewItem twItem, LeafNode node)
        {
            if (node == null)
            {
                return;
            }
            twItem.Header = node.Data;

            if (node.Children != null && node.Children.Count > 0)
            {
                for (int i = 0; i < node.Children.Count; i++)
                {
                    TreeViewItem it = new TreeViewItem();
                    AddLeaf(it, node.Children[i]);
                    twItem.Items.Add(it);
                }
            }
        }



        private void InitListView()
        {

            List<Student> sl = new List<Student>()
            {
            new Student(){Name = "zhangsan",Id=1,ClassName="yiban"},
            new Student(){Name="lisi",Id=2,ClassName="erban"},
            new Student(){Name = "wang5",Id=3,ClassName="sanban"}
            };

            lstStudent.ItemsSource = sl;

        }


        /// <summary>
        /// repeat button 使用
        /// </summary>
        int count = 0;
        private void btnRepeat_Click(object sender, RoutedEventArgs e)
        {
            count++;
            lblCount.Content = count.ToString();
        }

        /// <summary>
        /// toggle button 的使用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnToggle_Checked(object sender, RoutedEventArgs e)
        {
            lblToggle.Content = "first time";
        }

        private void btnToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            lblToggle.Content = "3rd";
        }

        private void btnToggle_Indeterminate(object sender, RoutedEventArgs e)
        {
            lblToggle.Content = "2nd";
        }

        /// <summary>
        /// tooltip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolTip_Opened(object sender, RoutedEventArgs e)
        {

        }

        private void ToolTip_Closed(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// expender 使用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {

        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// menu 使用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (item != null)
            {
                item.IsChecked = !item.IsChecked;
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = twLeaf.SelectedItem as TreeViewItem;
            if (item != null)
            {
                TreeViewItem pi = item.Parent as TreeViewItem;
                if (pi != null)
                {
                    pi.Items.Remove(item);
                }
                else
                {
                    twLeaf.Items.Remove(item);
                }
            }
        }

        private void twLeaf_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = e.OriginalSource as TreeViewItem;
            item.IsExpanded = true;
        }

        List<LeafNode> nodeList = new List<LeafNode>();
        private void btnGetNodes_Click(object sender, RoutedEventArgs e)
        {
            nodeList.Clear();
            for (int i = 0; i < twLeaf.Items.Count; i++)
            {
                TreeViewItem rootItem = twLeaf.Items[i] as TreeViewItem;
                if (null != rootItem)
                {
                    GetTreeviewNodes(null, rootItem);
                }
            }
        }


        private void GetTreeviewNodes(LeafNode parentNode, TreeViewItem twItem)
        {
            LeafNode node = new LeafNode()
            {
                Data = twItem.Header.ToString(),
                Children = new List<LeafNode>()
            };
            if (parentNode != null)
            {
                parentNode.Children.Add(node);
            }
            else
            {
                nodeList.Add(node);
            }
            if (twItem.Items.Count > 0)
            {
                for (int i = 0; i < twItem.Items.Count; i++)
                {
                    TreeViewItem item = twItem.Items[i] as TreeViewItem;
                    if (null != item)
                    {
                        GetTreeviewNodes(node, item);
                    }
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// prograssBar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnAddPro_Click(object sender, RoutedEventArgs e)
        {
            pgTest.Value += 4;
        }

        /// <summary>
        /// Slider
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnSliderValue_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(slTest.Value.ToString());
        }

        private void slTest_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblSlider.Content = slTest.Value.ToString();
        }

        /// <summary>
        /// inkCanvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveCvs_Click(object sender, RoutedEventArgs e)
        {
            string fileN = "D:/1.png";
            if (File.Exists(fileN))
            {
                File.Delete(fileN);
            }
     //     string  sigPath = System.IO.Path.GetTempFileName();

          

            MemoryStream ms = new MemoryStream();
            FileStream fs = new FileStream(fileN, FileMode.Create);

            RenderTargetBitmap rtb = new RenderTargetBitmap((int)cvs.Width,
                (int)cvs.Height, 0, 0, PixelFormats.Default);
            rtb.Render(this.cvs);
            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            encoder.Save(fs);
            fs.Close();
            
        }

    }
    public class Student
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string ClassName { get; set; }
    }

    public class LeafNode
    {
        private string data;

        public string Data
        {
            get { return data; }
            set { data = value; }
        }

        private List<LeafNode> children;

        public List<LeafNode> Children
        {
            get { return children; }
            set { children = value; }
        }
    }
}
