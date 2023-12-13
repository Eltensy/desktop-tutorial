using System.Globalization;
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
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.VisualBasic;
using wpf_backend.Data;
using wpf_backend;

namespace WpfTaskMaster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Back back = new Back();

        public MainWindow()
        {
            InitializeComponent();

            // Масив з завданнями
            //Tasks = new ObservableCollection<TaskItem>
            //{
            //    // елементи TaskItem
            //    // Наприклад:
            //    // new TaskItem
            //    //{
            //    //    TaskName = "Task 1",
            //    //    Description = "Description for Task 1",
            //    //    DueDate = "Due Date: 2023-11-30",
            //    //    Priority = "Priority: High",
            //    //    Labels = "Labels: Work, Important"
            //    //}
            //};
            //
            


            // Встановлення вкладки "today" за замовчуванням

            Button todayButton = FindName("today") as Button;

            // Перевірка, чи кнопка існує та встановлення її як значення lastClickedButton
            if (todayButton != null)
            {
                lastClickedButton = todayButton;


                lastClickedButton.Background = Brushes.Gray;
            }
            PopulateProjectsPanel();
            TaskPanel();
        }

        public static class LanguageManager
    {
        public static void SetLanguage(string languageCode)
        {
            ResourceDictionary dict = new ResourceDictionary();
                switch (languageCode)
                {
                    case "en":
                        dict.Source = new Uri($"pack://application:,,,/languages/Strings.en.xaml");
                        break;
                    case "ua":
                        dict.Source = new Uri($"pack://application:,,,/languages/Strings.ua.xaml");
                        break;
                    default:
                        dict.Source = new Uri($"pack://application:,,,/languages/Strings.ua.xaml");
                        break;

                }
                
            Application.Current.Resources.MergedDictionaries.Add(dict);
       
            }     
    }

        private Button lastClickedButton;

        private void PopulateProjectsPanel()
        {
            // Очистити ListBox перед додаванням нових елементів
            projectsListBox.Items.Clear();

            List<Project> usr_projs = back.getUserProjects();
            if(usr_projs == null )
            {
                // Очистити ListBox перед додаванням нових елементів
                projectsListBox.Items.Clear();
                return;
            } else
            {
                foreach(var proj in usr_projs)
                {
                    projectsListBox.Items.Add(proj.title);
                }
            }
        }

        private void TaskPanel()
        {
            // Очистити ListBox перед додаванням нових елементів
            tasksListBox.Items.Clear();

            var usr_projs = back.getUserTasks(null);
            if (usr_projs == null)
            {
                // Очистити ListBox перед додаванням нових елементів
                tasksListBox.Items.Clear();
                return;
            }
            else
            {
                foreach (var proj in usr_projs)
                {
                    tasksListBox.Items.Add($"Task:\n  Title: {proj.title}\n  Description: {proj.description}\n" +
                        $"  Deadline: {proj.deadline.ToString()}\n  Priority: {proj.priority}\n  Status: {proj.state}");
                }
            }
        }
        private void SidebarButton_Click(object sender, RoutedEventArgs e)
        {
            //// Змінюємо ширину ListBox при кожному кліку на кнопку
            //if (Main_column.Width != 750)
            //{
            //    Main_column.Width = 750;
            //    // tasksListBox.HorizontalAlignment = HorizontalAlignment.Left;
            //    tasksListBox.Margin = new Thickness(0, 62, 300, 0);
            //}
            //else
            //{

            //    Main_column.Width = 597;
            //    tasksListBox.Margin = new Thickness(0, 62, 0, 0);
            //}

            // Змінюємо видимість бокової панелі при кожному кліку на кнопку
            if (sidePanel.Visibility == Visibility.Visible)
            {
                sidePanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                sidePanel.Visibility = Visibility.Visible;
            }

            // Змінюємо видимість splitter при кожному кліку на кнопку
            if (splitter.Visibility == Visibility.Visible)
            {
                splitter.Visibility = Visibility.Collapsed;
            }
            else
            {
                splitter.Visibility = Visibility.Visible;
            }

            if (addTaskButton.Visibility == Visibility.Visible)
            {
                addTaskButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                addTaskButton.Visibility = Visibility.Visible;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (lastClickedButton != null)
            {
                // Скасувати виділення попередньої натиснутої кнопки
                lastClickedButton.Background = Brushes.Transparent;
            }

            // Встановити новий фон для натиснутої кнопки
            Button clickedButton = (Button)sender;
            clickedButton.Background = Brushes.Gray;

            // Оновити останню натиснуту кнопку
            lastClickedButton = clickedButton;
        }

        private void AddProjectButton_Click(object sender, RoutedEventArgs e)
        {
            // Create an instance of the AddProject window
            AddProjectWindow addProjectWindow = new AddProjectWindow();
            addProjectWindow.Owner = this;

            addProjectWindow.ShowDialog();
        }

        private void ToggleProjects_Checked(object sender, RoutedEventArgs e)
        {
            // Показати TreeView при виборі ToggleButton
            projectsListBox.Visibility = Visibility.Visible;
        }

        private void ToggleProjects_Unchecked(object sender, RoutedEventArgs e)
        {
            // Приховати TreeView при скасуванні вибору ToggleButton
            projectsListBox.Visibility = Visibility.Collapsed;
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            // Логіка для зміни пароля
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            back.terminateSession();

            SignUpWindow signUpWindow = new SignUpWindow();
            signUpWindow.Show();


            this.Close();
        }

        private void ChangeLanguage_Click(object sender, RoutedEventArgs e)
        {
            if (themeComboBox.SelectedItem is ComboBoxItem selectedComboBoxItem)
            {
                var selectedText = selectedComboBoxItem.Content?.ToString();
                testBlock.Text = selectedText;
            }

        }
        
        private void ApplyTheme(string themeName)
        {
            if (Resources.MergedDictionaries.Count > 0)
            {
                if (themeName == "LightTheme")
                    Resources.MergedDictionaries.Add((ResourceDictionary)Application.Current.Resources["LightTheme"]);
                else if (themeName == "DarkTheme")
                    Resources.MergedDictionaries.Add((ResourceDictionary)Application.Current.Resources["DarkTheme"]);
            }
        }
        
        private void ChangeTheme_Click(object sender, RoutedEventArgs e)
        {
            if (themeComboBox.SelectedItem != null)
            {
                ApplyTheme("DarkTheme");
                Console.WriteLine($"Выбранный элемент: {((ComboBoxItem)themeComboBox.SelectedItem).Content}");
            }
        }

        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
            accountSettingsPanel.Visibility = (accountSettingsPanel.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
            settingsPanel.Visibility = Visibility.Collapsed;
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            settingsPanel.Visibility = (settingsPanel.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
            accountSettingsPanel.Visibility = Visibility.Collapsed;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // Створення та відображення нового вікна для додавання завдань
            AddTaskWindow addTaskWindow = new AddTaskWindow();
            addTaskWindow.Owner = this;
            addTaskWindow.ShowDialog();
        }

        private void HomeToday_Click(object sender, RoutedEventArgs e)
        {
            TaskPanel();
            if (lastClickedButton != null)
            {
                // Скасувати виділення попередньої натиснутої кнопки
                lastClickedButton.Background = Brushes.Transparent;
            }

            // Встановити новий фон для кнопки "Today"
            today.Background = Brushes.Gray;

            // Оновити останню натиснуту кнопку
            lastClickedButton = today;

        }
        
        private void ShowContentForButton(Button button)
        {
            // Очистити вміст правої панелі

            //Main_column.Children.Clear(); 

            // Відображення відповідного вмісту для кожної кнопки
            switch (button.Name)
            {
                case "today":
                    // Логіка для відображення вмісту для кнопки "Today"
                    break;
                case "calendar":

                    break;
                case "projects":

                    PopulateProjectsPanel();
                    break;
                case "labels":

                    break;
                case "productivity":

                    break;
                default:
                    break;
            }
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Отримати вибраний спосіб сортування з випадаючого списку
            ComboBoxItem selectedSortItem = sortComboBox.SelectedItem as ComboBoxItem;
            string selectedSortOption = selectedSortItem?.Content?.ToString() ?? string.Empty;

            // Викликати функцію сортування відповідно до обраного варіанту
            switch (selectedSortOption)
            {
                case "Alphabetically":
                    SortTasksAlphabetically();
                    break;
                case "By Date":
                    SortTasksByDate();
                    break;
                case "By Priority":
                    SortTasksByPriority();
                    break;
            }
        }

        private void SortTasksAlphabetically()
        {
            // Логіка для сортування за алфавітом
            List<ListBoxItem> sortedItems = tasksListBox.Items.Cast<ListBoxItem>().OrderBy(item =>
            {
                // Отримати текст завдання та сортувати за ним
                CheckBox checkBox = ((StackPanel)item.Content).Children.OfType<CheckBox>().FirstOrDefault();

                return checkBox?.Content?.ToString();
            }).ToList();

            // Очистити та додати відсортовані елементи назад до ListBox
            tasksListBox.Items.Clear();
            foreach (ListBoxItem sortedItem in sortedItems)
            {
                tasksListBox.Items.Add(sortedItem);
            }
        }

        private void SortTasksByDate()
        {
            // Логіка для сортування за датою
            List<ListBoxItem> sortedItems = tasksListBox.Items.Cast<ListBoxItem>().OrderBy(item =>
            {
                // Отримати дату завдання та сортувати за нею
                TextBlock dueDateTextBlock = ((StackPanel)item.Content).Children.OfType<TextBlock>().FirstOrDefault(tb => tb.Text.StartsWith("Due Date:"));
                DateTime dueDate;
                if (DateTime.TryParse(dueDateTextBlock?.Text?.Replace("Due Date:", ""), out dueDate))
                {
                    return dueDate;
                }
                // Якщо дата не вдалося розпізнати, поверніть DateTime.MaxValue
                return DateTime.MaxValue;
            }).ToList();

            // Очистити та додати відсортовані елементи назад до ListBox
            tasksListBox.Items.Clear();
            foreach (ListBoxItem sortedItem in sortedItems)
            {
                tasksListBox.Items.Add(sortedItem);
            }
        }

        private void SortTasksByPriority()
        {
            List<ListBoxItem> sortedItems = tasksListBox.Items.Cast<ListBoxItem>().OrderByDescending(item =>
            {
                // Отримати пріоритет завдання та сортувати за ним
                TextBlock priorityTextBlock = ((StackPanel)item.Content).Children.OfType<TextBlock>().FirstOrDefault(tb => tb.Text.StartsWith("Priority:"));
                string priority = priorityTextBlock?.Text?.Replace("Priority:", "").Trim();

                // Перетворення текстового представлення пріоритету в числове значення
                int priorityValue = GetPriorityValue(priority);
                return priorityValue;
            }).ToList();

            tasksListBox.Items.Clear();
            tasksListBox.Items.Clear();
            foreach (ListBoxItem sortedItem in sortedItems)
            {
                tasksListBox.Items.Add(sortedItem);
            }
        }

        private int GetPriorityValue(string priority)
        {
            switch (priority.ToLower())
            {
                case "high":
                    return 3;
                case "medium":
                    return 2;
                case "low":
                    return 1;
                default:
                    return 0;
            }
        }

        private void tasksListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void GroupComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ChangeLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (languageComboBox.SelectedItem != null)
            {
                if (languageComboBox.SelectedItem is ComboBoxItem selectedComboBoxItem)
                {
                    if (selectedComboBoxItem.Content?.ToString() == "English") { 
                        LanguageManager.SetLanguage("en");
                        back.updateDBElement("User_settings", "lang", "en", null, null);
                    }
                    else { 
                        LanguageManager.SetLanguage("ua");
                        back.updateDBElement("User_settings", "lang", "ua", null, null);
                    }
                }
                else
                {
                    // Handle the case when the selected item is not a ComboBoxItem
                }
            }
            else
            {
                // Handle the case when no item is selected
            }
        }

        private void ChangeTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}