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

namespace UserWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Dialoge : Window
    {
        public Users Answer { get { return new Users { UserData = new UserData { FirstName = txtFirstName.Text, LastName = txtLastName.Text, Email = txtEmail.Text, Role = (rdStudent.IsChecked == true)?"student":"docent", Password = txtPassword.Text } }; } }

        public Dialoge()
        {
            InitializeComponent();

            btnConfirm.Content = "Create";
        }

        public Dialoge(Users user)
        {
            InitializeComponent();


            rdDocent.IsChecked = rdStudent.IsChecked = false;

            txtFirstName.Text = user.UserData.FirstName;
            txtLastName.Text = user.UserData.LastName;
            txtEmail.Text = user.UserData.Email;
            if (user.UserData.Role == "student") { rdStudent.IsChecked = true; } else { rdDocent.IsChecked = true; } 
            txtPassword.Text = user.UserData.Password;

            btnConfirm.Content = "Update";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelAction(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }


    }
}
