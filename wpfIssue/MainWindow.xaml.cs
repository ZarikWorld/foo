using System;
using System.Windows;
using wpfIssues.Model;
using Serilog;

namespace wpfIssues
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Log.Information("Enter {MethodName} method.", nameof(MainWindow));
            loadConfig(); 
            InitializeComponent();
            ApplyTheme(Properties.Settings.Default.CurrentTheme);
            Log.Information("Exit {MethodName} method.", nameof(MainWindow));
        }
        private static void loadConfig()
        {
            try
            {
                Log.Information("Enter {MethodName} method.", nameof(loadConfig));
                string userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                string configFilePath = System.IO.Path.Combine(userProfilePath, Properties.Settings.Default.configFileName);
                var config = new AppConfig(configFilePath);
                Properties.Settings.Default.mitarbeiter_id = config.mitarbeiter_id;
                //MainModel.projects_ids = config.projects_ids;
                Log.Information("Exit {MethodName} method.", nameof(loadConfig));                
            }
            catch (Exception ex)
            {
                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                MessageBox.Show(innerException.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

                Log.Fatal("{@innerException}", innerException);
                Log.Information("Shutdown application!");
                Application.Current.Shutdown();
            }
        }
        private void saveCurrentWindowLocation()
        {
            try
            {
                Log.Information("Enter {MethodName} method.", nameof(saveCurrentWindowLocation));
                if (this.WindowState == WindowState.Normal)
                {
                    Properties.Settings.Default.WindowTop = this.Top;
                    Properties.Settings.Default.WindowLeft = this.Left;
                    Properties.Settings.Default.WindowWidth = this.Width;
                    Properties.Settings.Default.WindowHeight = this.Height;
                }
                else
                {
                    // Save the RestoreBounds if the window is not in the Normal state.
                    Properties.Settings.Default.WindowTop = this.RestoreBounds.Top;
                    Properties.Settings.Default.WindowLeft = this.RestoreBounds.Left;
                    Properties.Settings.Default.WindowWidth = this.RestoreBounds.Width;
                    Properties.Settings.Default.WindowHeight = this.RestoreBounds.Height;
                }

                Properties.Settings.Default.WindowState = this.WindowState.ToString();
                Properties.Settings.Default.Save();
                Log.Information("Exit {MethodName} method.", nameof(saveCurrentWindowLocation));
            }
            catch (Exception ex)
            {
                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                Log.Error("Unable to save current window cordinates: {@innerException}", innerException);
            }
        }
        private void loadPreviousWindowLocation()
        {
            try
            {
                Log.Information("Enter {MethodName} method.", nameof(loadPreviousWindowLocation));
                if (Properties.Settings.Default.WindowWidth != 0 && Properties.Settings.Default.WindowHeight != 0)
                {
                    this.Top = Properties.Settings.Default.WindowTop;
                    this.Left = Properties.Settings.Default.WindowLeft;
                    this.Width = Properties.Settings.Default.WindowWidth;
                    this.Height = Properties.Settings.Default.WindowHeight;

                    if (Enum.TryParse(Properties.Settings.Default.WindowState, out WindowState state))
                    {
                        if (state == WindowState.Maximized)
                        {
                            // Position the window first and then maximize it
                            this.Loaded += (s, e) => this.WindowState = WindowState.Maximized;
                        }
                        else
                        {
                            this.WindowState = state;
                        }
                    }
                }
                Log.Information("Exit {MethodName} method.", nameof(loadPreviousWindowLocation));
            }
            catch (Exception ex)
            {
                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                Log.Error("Unable to load previous window cordinates: {@innerException}", innerException);
            }
        }
        public static void ApplyTheme(string theme)
        {
            Log.Information("Enter {MethodName} method.", nameof(ApplyTheme));
            switch (theme)
            {
                case "Light":
                    Syncfusion.SfSkinManager.SfSkinManager.SetVisualStyle(Application.Current.MainWindow, Syncfusion.SfSkinManager.VisualStyles.FluentLight);
                    break;
                case "Dark":
                    Syncfusion.SfSkinManager.SfSkinManager.SetVisualStyle(Application.Current.MainWindow, Syncfusion.SfSkinManager.VisualStyles.FluentDark);
                    break;
            }
            Log.Information("Exit {MethodName} method.", nameof(ApplyTheme));
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Log.Information("Enter {MethodName} method.", nameof(Window_Loaded));
            loadPreviousWindowLocation();
            Log.Information("Exit {MethodName} method.", nameof(Window_Loaded));
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Log.Information("Enter {MethodName} method.", nameof(Window_Closing));
            saveCurrentWindowLocation();
            Log.Information("Exit {MethodName} method.", nameof(Window_Closing));
        }
    }
}