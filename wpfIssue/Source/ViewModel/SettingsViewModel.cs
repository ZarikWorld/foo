using System.Windows;
using System.Windows.Input;

namespace wpfIssues.ViewModel
{
    internal class SettingsViewModel
    {
        public ICommand ChangeThemeCommand { get; }
        public SettingsViewModel()
        {
            ChangeThemeCommand = new Command.RelayCommand(ChangeTheme);
        }
        private void ChangeTheme(object parameter)
        {
            string? themeName = parameter as string;
            if (themeName != null)
            {
                wpfIssues.MainWindow.ApplyTheme(themeName);
                Properties.Settings.Default.CurrentTheme = themeName;
                Properties.Settings.Default.Save();
            }
        }
        public static void ApplyTheme(string theme)
        {
            switch (theme)
            {
                case "Light":
                    Syncfusion.SfSkinManager.SfSkinManager.SetVisualStyle(Application.Current.MainWindow, Syncfusion.SfSkinManager.VisualStyles.FluentLight);
                    break;
                case "Dark":
                    Syncfusion.SfSkinManager.SfSkinManager.SetVisualStyle(Application.Current.MainWindow, Syncfusion.SfSkinManager.VisualStyles.FluentDark);
                    break;
            }
        }
        public bool IsLightThemeSelected
        {
            get { return Properties.Settings.Default.CurrentTheme == "Light"; }
            set
            {
                if (value)
                {
                    ChangeTheme("Light");
                }
            }
        }
        public bool IsDarkThemeSelected
        {
            get { return Properties.Settings.Default.CurrentTheme == "Dark"; }
            set
            {
                if (value)
                {
                    ChangeTheme("Dark");
                }
            }
        }
    }
}
