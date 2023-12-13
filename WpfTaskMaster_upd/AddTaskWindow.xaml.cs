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
    /// Interaction logic for AddTaskWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        Back back = new Back();

        public AddTaskWindow()
        {
            InitializeComponent();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            // Get data from UI elements
            string taskName = taskNameTextBox.Text;
            string taskDescription = taskDescriptionTextBox.Text;
            DateTime dueDate = dueDatePicker.SelectedDate ?? DateTime.MinValue;
            TimeSpan estimate = TimeSpan.Parse("1"); // Add to the front + validation
            string priority = (priorityComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? string.Empty;
            string status = (statusComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? string.Empty;
            string labels = labelsTextBox.Text;

            // Validate input
            //if (string.IsNullOrEmpty(taskName))
            //{
            //    MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}

            // Check if the task with the same name already exists
            //if (dbContext.Tasks.Any(t => t.TaskName == taskName))
            //{
            //    MessageBox.Show("Task with the same name already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}

            if (0 != back.addTask(2, taskName, taskDescription, dueDate, estimate, status, priority))
            {
                MessageBox.Show("Error saving the task", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Task added", "Success", MessageBoxButton.OK);
            }


            // Close the window after adding the task
            this.Close();
        }
    }
}
