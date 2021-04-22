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

using Lib;
using UserWindow;


namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Program Program { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Program = new Program();
            Program.Binding(Connection.LOCAL);
            lblCurrent.Content = "Current Connection: LOCAL";      //LOCAL or LDAP
        }

        private void GetAllUsersAction(object sender, RoutedEventArgs e)
        {
            fieldResults.Items.Clear();
            List<Users> l = Program.GetADUsers();
            l.ForEach(x => fieldResults.Items.Add(x));
            btnDeleteUser.IsEnabled = false;
        }

        private void CreateUserAction(object sender, RoutedEventArgs e)
        {

            Dialoge w = new Dialoge();
            w.ShowDialog();
            if (w.DialogResult == true)
            {
                Program.CreateUser(w.Answer);
            }
        }

        private void ChangeConnectionAction(object sender, RoutedEventArgs e)
        {
            if (Program.Connection == Connection.LOCAL)
            {
                Program = new Program();
                Program.Binding(Connection.LDAP);
                lblCurrent.Content = "Current Connection: LDAP";
            }
            else
            {
                Program = new Program();
                Program.Binding(Connection.LOCAL);
                lblCurrent.Content = "Current Connection: LOCAL";
            }
        }

        private void DeleteUserAction(object sender, RoutedEventArgs e)
        {

        }
    }
}
