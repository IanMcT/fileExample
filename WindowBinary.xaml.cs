/*
 * Ian McTavish
 * March 3, 2020
 * Code to handle opening/saving files
 * in a binary format (i.e. machine readable, not human!)
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
using System.Windows.Shapes;

namespace fileExample
{
    /// <summary>
    /// Interaction logic for WindowBinary.xaml
    /// </summary>
    public partial class WindowBinary : Window
    {
        //WindowText.xaml.cs contains the User class
        List<User> users = new List<User>();

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowBinary()
        {
            InitializeComponent();
            //Set some default data to play with
            users.Add(new User() { Id = 1, Name = "John Doe", Birthday = new DateTime(1971, 7, 23) });
            users.Add(new User() { Id = 2, Name = "Jane Doe", Birthday = new DateTime(1974, 1, 17) });
            users.Add(new User() { Id = 3, Name = "Sammy Doe", Birthday = new DateTime(1991, 9, 2) });
            //Bind the datagrid to the list
            dgSimple.ItemsSource = users;
        }

        /// <summary>
        /// Opens the data from a binary file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuOpen_Click(object sender, RoutedEventArgs e)
        {
            //The BinaryFormatter will format the data
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = 
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            //Open file dialog allows us to let the user pick the file
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            //Note the extension I am using for data files
            ofd.Filter = "Data Files (*.dat)|*.dat|All Files (*.*)|*.*";
            bool? dialogResult = ofd.ShowDialog();
            //only fun if the user click Open
            if ((bool)dialogResult)
            {
                //Dealing with files - use a try-catch
                try
                {
                    //This sets a stream of data to the file
                    System.IO.Stream stream = 
                        new System.IO.FileStream(ofd.FileName, System.IO.FileMode.Open);
                    //Want to clear out the old users first
                    users.Clear();
                    //because the User and List classes are serializable
                    //you simply need to run the Deserialize method on the stream
                    //It will return an object that you can cast to the
                    //List<User> data type
                    users = (List<User>)formatter.Deserialize(stream);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            //Force the datagrid to update by resetting the binding
            dgSimple.ItemsSource = null;
            dgSimple.ItemsSource = users;
        }

        /// <summary>
        /// Save the users list to a binary file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSave_Click(object sender, RoutedEventArgs e)
        {
            // Need to create the binary formatter
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = 
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            //Let the user pick where to save the file
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.Filter = "Data Files (*.dat)|*.dat|All Files (*.*)|*.*";
            bool? dialogResult = sfd.ShowDialog();
            //Only run if they hit the save button
            if ((bool)dialogResult)
            {
                try
                {
                    //Create the instance of the file stream to write to
                    System.IO.Stream stream =
                            new System.IO.FileStream(sfd.FileName, System.IO.FileMode.OpenOrCreate);

                    //serializes the data and sends it to the file stream
                    formatter.Serialize(stream, users);
                    //flush and close the stream
                    stream.Flush();
                    stream.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }//end catch
            }//end if
        }

        /// <summary>
        /// Makes sure the user can save the data before closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuClose_Click(object sender, RoutedEventArgs e)
        {
            this.menuSave_Click(sender, e);
            this.DialogResult = true;
        }
    }
}
