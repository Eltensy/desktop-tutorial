using System.Configuration;
using System.Data;
using System.Windows;

namespace WpfTaskMaster
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ApplyTheme("DarkTheme");
        }

        private void ApplyTheme(string themeName)
        {
            if (Resources.MergedDictionaries.Count > 0)
                Resources.MergedDictionaries.Clear();

            if (themeName == "LightTheme")
                Resources.MergedDictionaries.Add((ResourceDictionary)Application.Current.Resources["LightTheme"]);
            else if (themeName == "DarkTheme")
                Resources.MergedDictionaries.Add((ResourceDictionary)Application.Current.Resources["DarkTheme"]);
        }

    }

}
