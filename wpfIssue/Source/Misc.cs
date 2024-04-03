using System;
using wpfIssues.Model;
using System.Windows;
using Serilog;

namespace wpfIssues
{
    internal class Misc
    {
        //WHO EVER CALLS ME, IS RESPONSBILE FOR EXCEPTION HANDLING (Bubble Up)!
        public static string getExceptionMessage(Exception exception)
        {
            string innerException = exception.InnerException != null ? $"\n\nInnerException:\n{exception.InnerException.Message}" : "";

            string stackTrace = exception.StackTrace != null ? $"\n\nException.StackTrace:\n{exception.StackTrace}" : "";


            string errorMessage = $"\n\nException:" +
                                  $"\n{exception.Message}" +
                                  innerException + stackTrace +
                                  $"\n\n";

            return errorMessage;
        }
        public static void logIt(Exception exception)
        {
            libCodLibCS.nsFileSystem.clsFileSystem.addProtokollException($"{AppDomain.CurrentDomain.BaseDirectory}{Properties.Settings.Default.logFileName}", exception);
        }
        public static void InZwischenablageKopieren(string clipboardString)
        {
            Log.Information("Enter {MethodName} method.", nameof(InZwischenablageKopieren));
            DataObject data = new DataObject();
            data.SetData(DataFormats.Text, clipboardString);
            Clipboard.SetDataObject(data);
            Log.Information("Exit {MethodName} method.", nameof(InZwischenablageKopieren));
        }
        public static string formatInZwischenablage(JxTask task)
        {
            return $"{task.titel}\n{task.web_url}\n\n";
        }
    }
}
