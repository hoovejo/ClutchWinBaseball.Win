using Microsoft.Phone.Tasks;
using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using System.Windows;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage;

namespace ClutchWinBaseball.WP8.Exceptions
{
    public static class ExceptionHandler
    {
        const string FileName = "errorreporting.txt";
        const string EmailTarget = "joe_hoover7@hotmail.com";
        const string MessageBoxTitle = "Problem Report";
        const string ModelText = "An error occured last time ClutchWin was run, do you want to help us out and send the error to us?";

        private static ErrorMessageInfo errormessage;

        public static void HandleException(ApplicationUnhandledExceptionEventArgs ex, string extra = "")
        {
            try
            {
                errormessage = new ErrorMessageInfo()
                {
                    UserMessage = string.IsNullOrEmpty(extra) ? "Unexpected error" : extra,
                    Info = string.IsNullOrEmpty(ex.ExceptionObject.Message) ? "Error message" : ex.ExceptionObject.Message,
                    Exception = ex.ExceptionObject.Message,
                    ExceptionDetail = ex.ExceptionObject.StackTrace
                };

                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    SafeDeleteFile(store);
                    var jsonString = JsonConvert.SerializeObject(errormessage);
                    using (TextWriter output = new StreamWriter(store.CreateFile(FileName)))
                    {
                        output.WriteLine(jsonString);
                    }
                }
            }
            catch { }
        }

        public static void HandleException(Exception ex, string extra = "")
        {
            try
            {
                errormessage = new ErrorMessageInfo()
                {
                    UserMessage = string.IsNullOrEmpty(extra) ? "Unexpected error" : extra,
                    Info = string.IsNullOrEmpty(ex.Message) ? "Error message" : ex.Message,
                    Exception = ex.InnerException.ToString(),
                    ExceptionDetail = ex.StackTrace
                };

                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    SafeDeleteFile(store);
                    var jsonString = JsonConvert.SerializeObject(errormessage);
                    using (TextWriter output = new StreamWriter(store.CreateFile(FileName)))
                    {
                        output.WriteLine(jsonString);
                    }
                }
            }
            catch { }
        }

        public static void CheckForPreviousException()
        {
            try
            {
                errormessage = null;
                string contents = null;
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (store.FileExists(FileName))
                    {
                        using (TextReader reader = new StreamReader(store.OpenFile(FileName, FileMode.Open, FileAccess.Read, FileShare.None)))
                        {
                            contents = reader.ReadToEnd();
                        }
                        SafeDeleteFile(store);
                    }
                }

                if (contents != null)
                {
                    errormessage = JsonConvert.DeserializeObject<ErrorMessageInfo>(contents);
                }

                if (errormessage != null)
                {
                    if (MessageBox.Show(ModelText, MessageBoxTitle, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        EmailComposeTask email = new EmailComposeTask();
                        email.To = EmailTarget;
                        email.Subject = errormessage.UserMessage;
                        email.Body = ReportContents();
                        SafeDeleteFile(IsolatedStorageFile.GetUserStoreForApplication());
                        email.Show();
                    }
                }
            }
            catch { }
        }

        private static string ReportContents()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Please send report to:{0}", Environment.NewLine);
            builder.Append(EmailTarget);
            builder.AppendLine();

            builder.AppendFormat("Error Detail{0}", Environment.NewLine);
            builder.Append(errormessage.Exception);

            builder.AppendLine();
            builder.AppendFormat("{0}Additional Info{0}", Environment.NewLine);
            builder.Append(errormessage.ExceptionDetail);
            builder.AppendLine();

            return builder.ToString();
        }

        private static void SafeDeleteFile(IsolatedStorageFile store)
        {
            try
            {
                store.DeleteFile(FileName);
            }
            catch { }
        }
    }

    public class ErrorMessageInfo
    {
        public string UserMessage;
        public string Info;
        public string Exception;
        public string ExceptionDetail;
    }
}