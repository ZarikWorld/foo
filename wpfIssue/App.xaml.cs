using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using wpfIssues.ViewModel;

namespace wpfIssues
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;
        public static IServiceProvider ServiceProvider { get; private set; }
        public static string logFileName { get; } = libCodLibCS.nsFileSystem.clsFileSystem.getLogFilePath();

        public App()
        {
            ConfigLogger();
            RegisterSyncfusionLicense();
            SubscribeToExceptionEvents();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
            ServiceProvider = _serviceProvider;
        }

        private void ConfigLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.File($"{AppDomain.CurrentDomain.BaseDirectory}/logs/.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            Log.Information("Application Starting");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.Information($"Exit Application!");

            Log.CloseAndFlush();
            base.OnExit(e);
        }

        private void RegisterSyncfusionLicense()
        {
            Log.Information("Enter {MethodName} method.", nameof(RegisterSyncfusionLicense));
            const string licenseKey = "Ngo9BigBOggjHTQxAR8/V1NGaF1cWmhIfEx1RHxQdld5ZFRHallYTnNWUj0eQnxTdEZjUX9ccH1WQWRdUEZzXw==";
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(licenseKey);
            Log.Information("Exit {MethodName} method.", nameof(RegisterSyncfusionLicense));
        }

        private void SubscribeToExceptionEvents()
        {
            Log.Information("Enter {MethodName} method.", nameof(SubscribeToExceptionEvents));
            TaskScheduler.UnobservedTaskException += OnTaskSchedulerUnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainUnhandledException;
            DispatcherUnhandledException += OnDispatcherUnhandledException;
            Log.Information("Exit {MethodName} method.", nameof(SubscribeToExceptionEvents));
        }

        private void OnTaskSchedulerUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            ShowErrorMessage("An unexpected error occurred in an unobserved task. Please contact support.");

            var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(e.Exception);
            libCodLibCS.nsFileSystem.clsFileSystem.addProtokollException(App.logFileName, e.Exception, "wpfIssues.App");
            MessageBox.Show(innerException.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            e.SetObserved();
            Application.Current.Shutdown();
        }

        private void OnCurrentDomainUnhandledException(object? sender, UnhandledExceptionEventArgs e)
        {
            ShowErrorMessage("An unexpected error occurred on a non-UI thread. Please contact support.");


            if (e.ExceptionObject is Exception ex)
            {
                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                libCodLibCS.nsFileSystem.clsFileSystem.addProtokollException(App.logFileName, ex, "wpfIssues.App");
                MessageBox.Show(innerException.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                string exceptionObjectType = e.ExceptionObject.GetType().FullName;
                string exceptionObjectString = e.ExceptionObject.ToString();
                string logMessage = $"Unexpected non-exception object in UnhandledException handler: Type={exceptionObjectType}, Object={exceptionObjectString}";
                libCodLibCS.nsFileSystem.clsFileSystem.addProtokollException(App.logFileName, new Exception(logMessage), "wpfIssues.App");
            }

            Application.Current.Shutdown();
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if (e.Exception is Exception ex)
            {
                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(e.Exception);
                libCodLibCS.nsFileSystem.clsFileSystem.addProtokollException(App.logFileName, e.Exception, "wpfIssues.App");
                MessageBox.Show(innerException.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                ShowErrorMessage("A) OnDispatcherUnhandledException");
            }
            else
            {
                //ShowErrorMessage("An 'else' unexpected error occurred on the main UI thread. Please contact support.");
                string? exceptionObjectType = e.Exception.GetType().FullName;
                string? exceptionObjectString = e.Exception.ToString();
                string logMessage = $"Unexpected non-exception object in UnhandledException handler: Type={exceptionObjectType}, Object={exceptionObjectString}";
                libCodLibCS.nsFileSystem.clsFileSystem.addProtokollException(App.logFileName, new Exception(logMessage), "wpfIssues.App");
                ShowErrorMessage("B) OnDispatcherUnhandledException");
            }

            e.Handled = true;
            Application.Current.Shutdown();
        }

        private static void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Unexpected Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<JxIssuesDataService>();   // Register the shared service as a singleton to ensure a single instance
            services.AddTransient<TodosViewModel>();   // Register ViewModels
            services.AddTransient<BacklogViewModel>();  // Register ViewModels
            services.AddTransient<GanttViewModel>();  // Register ViewModels
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Log.Information("Enter {MethodName} method.", nameof(OnStartup));

            base.OnStartup(e);
            
            var taskDataService = ServiceProvider.GetService<JxIssuesDataService>();
            try
            {
                taskDataService!.getMitarbeiters();
                taskDataService!.setUserInfo();
                taskDataService!.getTasks();
            }
            catch (Exception ex)
            {
                Log.Fatal("An error occurred in {MethodName}", nameof(OnStartup));
                var innerException = libCodLibCS.nsFileSystem.clsFileSystem.getInnermostException(ex);
                Log.Fatal("{@innerException}", ex);

                string message = "Ein unerwarteter Fehler verhinderte den Start der Anwendung. Die Anwendung wird jetzt geschlossen.";
                string caption = "Startfehler";

                MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);

                Application.Current.Shutdown();
                return;
            }

            var mainWindow = _serviceProvider.GetService<MainWindow>(); // Ensure MainWindow is also registered if needed
            mainWindow?.Show();
            
            Log.Information("Exit {MethodName} method.", nameof(OnStartup));
        }
    }
}
