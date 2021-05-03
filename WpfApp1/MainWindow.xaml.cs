using System;
using System.Collections.Generic;
using System.Diagnostics;
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

using Lib;
using UserWindow;


namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CRUD Program { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Program = new CRUD();
            Program.Binding(Connection.LOCAL);
            lblCurrent.Content = "Current Connection: LOCAL";      //LOCAL or LDAP
            btnCreateUser.IsEnabled = btnDeleteUser.IsEnabled = btnUpdateUser.IsEnabled = false;
        }

        private void GetAllUsersAction(object sender, RoutedEventArgs e)
        {
            fieldResults.Items.Clear();
            List<Users> l = Program.GetADUsers();
            if (l != null)
            {
                l.ForEach(x => fieldResults.Items.Add(x));
            }
            else
            {
                MessageBox.Show("No Users found or Database not connected!");
            }
            
            btnCreateUser.IsEnabled = btnDeleteUser.IsEnabled = btnUpdateUser.IsEnabled = true;
        }

        private void CreateUserAction(object sender, RoutedEventArgs e)
        {

            Dialoge w = new Dialoge("c");
            w.ShowDialog();
            if (w.DialogResult == true)
            {
                if (Program.CreateUser(new Users { UserData = new UserData { FirstName = w.Answer } }))
                {
                    MessageBox.Show("User succesfully created!");
                    btnCreateUser.IsEnabled = btnDeleteUser.IsEnabled = btnUpdateUser.IsEnabled = false;
                }
            }
        }

        private void ChangeConnectionAction(object sender, RoutedEventArgs e)
        {
            if (Program.Connection == Connection.LOCAL)
            {
                Program = new CRUD();
                Program.Binding(Connection.LDAP);
                lblCurrent.Content = "Current Connection: LDAP";
            }
            else
            {
                Program = new CRUD();
                Program.Binding(Connection.LOCAL);
                lblCurrent.Content = "Current Connection: LOCAL";
            }
            btnCreateUser.IsEnabled = btnDeleteUser.IsEnabled = btnUpdateUser.IsEnabled = false;
        }

        private void DeleteUserAction(object sender, RoutedEventArgs e)
        {
            if (fieldResults.SelectedIndex != -1)
            {
                Debug.WriteLine(fieldResults.SelectedValue.ToString());
                if (Program.DeleteUser(fieldResults.SelectedValue.ToString()))
                {
                    MessageBox.Show("User succesfully deleted!");
                    btnCreateUser.IsEnabled = btnDeleteUser.IsEnabled = btnUpdateUser.IsEnabled = false;
                }   
            }
            else
            {
                MessageBox.Show("Select a user first!");
            }
        }

        private void UpdateUserAction(object sender, RoutedEventArgs e)
        {
            if (fieldResults.SelectedIndex != -1)
            {
                Dialoge w = new Dialoge("u");
                w.ShowDialog();

                if (w.DialogResult == true)
                {
                    Debug.WriteLine(fieldResults.SelectedValue.ToString());
                    if (Program.UpdateUser(fieldResults.SelectedValue.ToString(), w.Answer))
                    {
                        MessageBox.Show("User succesfully updated!");
                        btnCreateUser.IsEnabled = btnDeleteUser.IsEnabled = btnUpdateUser.IsEnabled = false;
                    }
                }
            }
            else
            {
                MessageBox.Show("Select a user first!");
            }
        }
    }
}
