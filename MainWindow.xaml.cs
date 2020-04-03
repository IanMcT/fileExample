/*
 * Ian McTavish
 * March 3, 2020
 * Program to demonstrate how to read/write different file formats
 */
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

namespace fileExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Constructor 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Opens the window for the text file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTextFile_Click(object sender, RoutedEventArgs e)
        {
            //WindowText is a Window (Program - Add New Window)
            //this creates an instance of the window
            WindowText wt = new WindowText();
            //WindowState allows us to make it open full screen
            wt.WindowState = WindowState.Maximized;
            //this is where the window actually opens up
            wt.ShowDialog();
        }

        /// <summary>
        /// Opens the window for the Binary File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBinaryFile_Click(object sender, RoutedEventArgs e)
        {
            //See the Text file example above.  Similar code.
            WindowBinary wb = new WindowBinary();
            wb.WindowState = WindowState.Maximized;
            wb.ShowDialog();
        }


        /// <summary>
        /// Opens the window for the XML File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnXMLFile_Click(object sender, RoutedEventArgs e)
        {
            Window1XML wx = new Window1XML();
            wx.WindowState = WindowState.Maximized;
            wx.ShowDialog();
        } 
    }
}
