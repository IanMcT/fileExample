/*
 * Ian McTavish
 * March 3, 2020
 * Code to handle opening/saving files
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
    /// User class is used by all the files.
    /// It is better form to put in another file but
    /// I wanted to ensure I could explain it easily.
    /// 
    /// Serializable is used by the binary window.  Check there for details
    /// User class contains three properties - number (Id), string (Name), date (Birthday)
    /// </summary>
    [Serializable]
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Birthday { get; set; }
    }

    /// <summary>
    /// Interaction logic for WindowText.xaml
    /// </summary>
    public partial class WindowText : Window
    {
        //List is like an array but it is dynamic - you can add and delete items
        List<User> users = new List<User>();

        public WindowText()
        {
            InitializeComponent();
            
            //add some default data to play with
            users.Add(new User() { Id = 1, Name = "John Doe", Birthday = new DateTime(1971, 7, 23) });
            users.Add(new User() { Id = 2, Name = "Jane Doe", Birthday = new DateTime(1974, 1, 17) });
            users.Add(new User() { Id = 3, Name = "Sammy Doe", Birthday = new DateTime(1991, 9, 2) });

            //This will bind the spreadsheet to the list
            dgSimple.ItemsSource = users;
            //binding allows you to edit the spreadshet - changes you make update your list
            //without any code on your part
        }

        /// <summary>
        /// Opens a file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuOpen_Click(object sender, RoutedEventArgs e)
        {
            //Open file dialog allows the user to pick the file to open
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            //Filter allows you to force the user to see only the approved file types
            ofd.Filter = "Text Files (*.txt)|*.txt|CSV (*.csv)|*.csv|All Files (*.*)|*.*";
            //ShowDialog will open it - clicking the open button returns true
            bool? dialogResult = ofd.ShowDialog();
            //only open a file if the user returns true
            if ((bool)dialogResult)
            {
                //always use try-catch when dealing with files.
                try
                {
                    //Create a streamreader based on the file name the user selected
                    System.IO.StreamReader sr = new System.IO.StreamReader(ofd.FileName);
                    //Clear the list of all the data you have
                    users.Clear();
                    //loop until the end of the file
                    while (!sr.EndOfStream)
                    {
                        //temp variables to store data as you work through the file
                        Int32 tempid;
                        string tempName;
                        Int64 tempBDayInt;
                        DateTime tempBDay;
                        //create a temp user
                        User temp = new User();
                        //read the line
                        string line = sr.ReadLine();
                        //Our file is comma seperated
                        //Note, I have no logic to handle if the user puts a comma in there data
                        string[] items = line.Split(new char[] { ',' });
                        //The Id needs to be a number
                        Int32.TryParse(items[0], out tempid);
                        //Name is a string - pull that item
                        tempName = items[1];
                        //Date is stored a large integer that represents 
                        //the number of 100-nanosecond intervals that have elapsed since 
                        //12:00 midnight, January 1, 1601 A.D. (C.E.) Coordinated Universal Time (UTC)
                        Int64.TryParse(items[2], out tempBDayInt);
                        tempBDay = DateTime.FromFileTimeUtc(tempBDayInt);
                        //add the temp values to our temp user
                        temp.Id = tempid;
                        temp.Name = tempName;
                        temp.Birthday = tempBDay;
                        //add the temp user to the list of users
                        users.Add(temp);
                    }//end while loop
                    //close the streamreader
                    sr.Close();
                }//end try
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }//end catch

                //to update the datagrid spreadsheet we need to reset the ItemsSource
                //it can only be set if the value is null so set it to null first
                dgSimple.ItemsSource = null;
                dgSimple.ItemsSource = users;
            }//end if
        }//end open method

        /// <summary>
        /// Saves the users list as a text file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSave_Click(object sender, RoutedEventArgs e)
        {
            //prepare a string to write to the file
            string s = "";
            
            //loop through the rows
            foreach (User row in users)
            {
                //Add all the data as a comma separated list
                s += row.Id.ToString() + "," +
                    row.Name.ToString() + "," +
                    row.Birthday.ToFileTimeUtc().ToString();

                s += System.Environment.NewLine;//add new line
            }//end for loop for rows
            s = s.Trim();//delete any extra spaces

            //Run the save file dialog
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.Filter = "Text Files (*.txt)|*.txt|CSV (*.csv)|*.csv|All Files (*.*)|*.*";
            bool? dialogResult = sfd.ShowDialog();
            if ((bool)dialogResult)
            {
                //The System.IO namespace contains classes for files
                //note - you could add a using line at the top to reduce the
                //amount of typing
                //The file name is taken from openFileDialog1
                System.IO.StreamWriter writer = new System.IO.StreamWriter(sfd.FileName);

                //Potential errors - try/catch to deal with
                try
                {
                    writer.Write(s);//writes the string
                    writer.Flush();//clears the stream
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message + "\n");
                }
                writer.Close();//Always close your files
            }

        }

        /// <summary>
        /// When the DialogResult is set the ShowDialog method finishes, closing the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuClose_Click(object sender, RoutedEventArgs e)
        {
            //I like to remind people to save their work
            this.menuSave_Click(sender, e);
            this.DialogResult = true;
        }//end menuClose
    }//end class
}//end Namespace
