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
using wpf_backend.Data;

namespace WpfTaskMaster
{
    /// <summary>
    /// Interaction logic for AddProjectWindow.xaml
    /// </summary>
    public partial class AddProjectWindow : Window
    {
        Back back = new Back();

        public AddProjectWindow()
        {
            InitializeComponent();
        }

        private void AddProject_Click(object sender, RoutedEventArgs e)
        {
            // Get data from UI elements
            string projectName = projectNameTextBox.Text;
            string projectDescription = projectDescriptionTextBox.Text;
            DateTime dueDate = dueDatePicker.SelectedDate ?? DateTime.MinValue;
            string status = (statusComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? string.Empty;
            TimeSpan estimate = TimeSpan.Parse("1"); // Add to the front + validation
            string priority = "low";
            string labels = labelsTextBox.Text;

            if (0 != back.addProject(projectName, projectDescription, dueDate, estimate, status, priority))
            {
                MessageBox.Show("Error saving the project", "Error", MessageBoxButton.OK);
            } else
            {
                MessageBox.Show("Project added", "Success", MessageBoxButton.OK);
            }

            // Close the window after adding the project
            this.Close();
        }
    }
}
