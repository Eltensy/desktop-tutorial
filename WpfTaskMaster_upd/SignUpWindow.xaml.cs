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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using wpf_backend;


namespace WpfTaskMaster
{
    /// <summary>
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        public Back back = new Back();

        private System.Windows.Threading.DispatcherTimer dispatcherTimer;

        public SignUpWindow()
        {
            InitializeComponent();

            Loaded += SignUpWindow_Loaded;

            // Ініціалізація таймера
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(CloseWindow);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1); // 1 секунда
        }

        private void SignUpWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TranslateTransform trans = new TranslateTransform();
            stackPanel.RenderTransform = trans;

            DoubleAnimation anim = new DoubleAnimation(-400, 0, TimeSpan.FromSeconds(0.7));
            trans.BeginAnimation(TranslateTransform.YProperty, anim);

        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            // Зчитування введених даних
            string name = txtFullName.Text;
            string login = txtLogin.Text;
            string password = txtPassword.Password;
            string confirmPassword = txtConfirmPassword.Password;

            // Валідація ім'я користувача та паролю
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill in all fields.", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Обмеження на ім'я користувача (наприклад, не менше 5 символів)
            if (login.Length < 5)
            {
                MessageBox.Show("Username must be at least 5 characters long.", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Обмеження на пароль
            if (password.Length < 4 && password.Length > 20)
            {
                MessageBox.Show("Password must be 5 to 20 characters long.", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Валідація підтвердження паролю
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Registration Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            DoubleAnimation heightAnimation = new DoubleAnimation
            {
                From = stackPanel.ActualHeight, // Початкова висота StackPanel
                To = 0, // Кінцева висота (зникає)
                Duration = TimeSpan.FromSeconds(1) // Тривалість анімації
            };

            // Створення анімації для ширини StackPanel
            DoubleAnimation widthAnimation = new DoubleAnimation
            {
                From = stackPanel.ActualWidth,
                To = 0,
                Duration = TimeSpan.FromSeconds(1)
            };

            switch (back.signUp(login, name, password))
            {
                case -1:
                    MessageBox.Show("Login already exists!", "Registration error", MessageBoxButton.OK);
                    return;
                    break;
                case 0:
                    // Запуск анімацій
                    stackPanel.BeginAnimation(StackPanel.HeightProperty, heightAnimation);
                    stackPanel.BeginAnimation(StackPanel.WidthProperty, widthAnimation);

                    // Перевірка, чи таймер не запущений
                    if (!dispatcherTimer.IsEnabled)
                    {
                        // Запуск таймера
                        dispatcherTimer.Start();
                    }
                    break;
                default:
                    throw new Exception("Error during registration!");
                    break;
            }

        }

        private void CloseWindow(object sender, EventArgs e)
        {
            // Перехід на головне вікно
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            // Зупинка таймера, щоб не викликати CloseWindow більше одного разу
            dispatcherTimer.Stop();
            this.Close();
        }

        private void LoginLink_Click(object sender, RoutedEventArgs e)
        {

            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();

            this.Close();
        }

    }
}
