/*
 * Ian McTavish
 * March 3, 2020
 * Code to handle opening/saving files
 * in an XML format (i.e. machine and human readable)
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
    /// Interaction logic for Window1XML.xaml
    /// </summary>
    public partial class Window1XML : Window
    {
        //WindowText.xaml.cs contains the User class
        List<User> users = new List<User>();

        /// <summary>
        /// Constructor
        /// </summary>
        public Window1XML()
        {
            InitializeComponent();
            //set some default data
            users.Add(new User() { Id = 1, Name = "John Doe", Birthday = new DateTime(1971, 7, 23) });
            users.Add(new User() { Id = 2, Name = "Jane Doe", Birthday = new DateTime(1974, 1, 17) });
            users.Add(new User() { Id = 3, Name = "Sammy Doe", Birthday = new DateTime(1991, 9, 2) });

            //Bind the datagrid to the list
            dgSimple.ItemsSource = users;
        }

        /// <summary>
        /// Opens an xml file and updates the users based on it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuOpen_Click(object sender, RoutedEventArgs e)
        {
            //Get the user to pick the file - note the filter
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
            bool? dialogResult = ofd.ShowDialog();
            //only run if they clicked open
            if ((bool)dialogResult)
            {
                //working with files, so try/catch
                try
                {
                    //XmlReader is used to read through the file
                    using (System.Xml.XmlReader reader = System.Xml.XmlReader.Create(ofd.FileName))
                    {
                        //clear out the old users to start fresh
                        users.Clear();

                        //temp variables to keep track of data as it is read
                        Int32 tempid;
                        string tempName;
                        Int64 tempBDayInt;
                        DateTime tempBDay;

                        User temp = new User();

                        //The read method reads every element - when at the end it returns false 
                        //stopping the loop
                        while (reader.Read())
                        {
                            //only detect start elements
                            if (reader.IsStartElement())
                            {
                                //get element name and switch on it
                                switch (reader.Name)
                                {
                                    case "Id":
                                        if (reader.Read())
                                        {
                                            //Set the id based on the value in the element
                                            Int32.TryParse(reader.Value.Trim(), out tempid);
                                            temp.Id = tempid;
                                        }
                                        break;
                                    case "Name":
                                        if (reader.Read())
                                        {
                                            //set the name based on the value in the element
                                            tempName = reader.Value.Trim() ;
                                            temp.Name = tempName;
                                        }
                                        break;
                                    case "Birthday":
                                        if (reader.Read())
                                        {
                                            //set the birthday based on the value in the element
                                            Int64.TryParse(reader.Value.Trim(), out tempBDayInt);
                                            tempBDay = DateTime.FromFileTimeUtc(tempBDayInt);
                                            temp.Birthday = tempBDay;
                                            //Birthday is the last element 
                                            //add the temp user to users and create a blank temp user
                                            users.Add(temp);
                                            temp = new User();
                                        }
                                        break;
                                }//end switch
                            }//end if
                        }//end while
                        
                    }//end using

                       

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            //Force the datagrid to update based on the new list of users
            dgSimple.ItemsSource = null;
            dgSimple.ItemsSource = users;
        }

        /// <summary>
        /// Saves an xml file based on the users list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuSave_Click(object sender, RoutedEventArgs e)
        {
            //Get the user to pick where they will save it - note the filter
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.Filter = "XML (*.xml)|*.xml|All Files (*.*)|*.*";
            bool? dialogResult = sfd.ShowDialog();
            //only run if they click Save
            if ((bool)dialogResult)
            {
                try
                {
                    //using a writer allows us to create a new document
                    using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(sfd.FileName))
                    {
                        //start the document, add the first tag
                        writer.WriteStartDocument();
                        writer.WriteStartElement("Users");
                        //loop through the list
                        foreach (User row in users)
                        {
                            //Write each element and the value
                            writer.WriteStartElement("User");
                            writer.WriteElementString("Id", row.Id.ToString());
                            writer.WriteElementString("Name", row.Name);
                            writer.WriteElementString("Birthday", row.Birthday.ToFileTimeUtc().ToString());

                            //closer the user tag
                            writer.WriteEndElement();
                        }//end for loop for rows
                        //close the Users tag
                        writer.WriteEndElement();
                        //finish the document
                        writer.WriteEndDocument();
                    }//end using

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }//end catch
            }//end if
        }

        /// <summary>
        /// Prompt the user to save and then close the window.
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