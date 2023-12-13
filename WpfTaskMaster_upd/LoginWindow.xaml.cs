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
using wpf_backend;

namespace WpfTaskMaster
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public Back back = new Back();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            // Зчитування введених даних
            string login = txtLogin.Text;
            string password = txtPassword.Password;

            switch (back.loginCheck(login, password))
            {
                case -1:
                    MessageBox.Show("Incorrect password!", "Login error", MessageBoxButton.OK);
                    break;
                case -2:
                    MessageBox.Show("Incorrect login!", "Login error", MessageBoxButton.OK);
                    break;
                case 0:
                    // Перехід на головне вікно
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                    break;
                default:
                    throw new Exception("Error during login");
                    break;
            }

        }


        private void SignUpLink_Click(object sender, RoutedEventArgs e)
        {

            SignUpWindow signUpWindow = new SignUpWindow();
            signUpWindow.Show();

            this.Close();
        }
    }
}
