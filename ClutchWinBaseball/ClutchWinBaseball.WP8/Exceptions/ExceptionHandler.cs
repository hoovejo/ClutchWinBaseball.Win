using Newtonsoft.Json;
using System;
using System.Text;
using System.Windows;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Popups;

namespace ClutchWinBaseball.WP8.Exceptions
{
    public static class ExceptionHandler
    {
        const string FileName = "errorreporting.txt";
        const string EmailTarget = "joe_hoover7@hotmail.com";

        private static TypedEventHandler<DataTransferManager, DataRequestedEventArgs> handler;
        private static ErrorMessageInfo errormessage;

        public async static void HandleException(ApplicationUnhandledExceptionEventArgs ex, string extra = "")
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
                var cache = new CacheFileManager(ApplicationData.Current.LocalFolder);
                var jsonString = JsonConvert.SerializeObject(errormessage);
                await cache.CacheUpdateAsync(FileName, jsonString);
            }
            catch { }
        }

        public async static void HandleException(Exception ex, string extra = "")
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
                var cache = new CacheFileManager(ApplicationData.Current.LocalFolder);
                var jsonString = JsonConvert.SerializeObject(errormessage);
                await cache.CacheUpdateAsync(FileName, jsonString);
            }
            catch { }
        }

        public async static void CheckForPreviousException()
        {
            var cache = new CacheFileManager(ApplicationData.Current.LocalFolder);
            try
            {
                errormessage = null;
                var jsonString = await cache.CacheInquiryAsync(FileName);
                errormessage = JsonConvert.DeserializeObject<ErrorMessageInfo>(jsonString);

                if (errormessage != null)
                {
                    //ShowErrorMessageDialog();
                    await cache.DeleteFileAsync(FileName);
                }
            }
            catch { }
        }

        private static void ShowErrorMessageDialog()
        {
            // Register handler for DataRequested events for sharing
            if (handler != null)
                DataTransferManager.GetForCurrentView().DataRequested -= handler;

            handler = new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(OnDataRequested);
            DataTransferManager.GetForCurrentView().DataRequested += handler;

            // Create the message dialog and set its content
                //(("An error occured last time ClutchWin was run, do you want to help us out and send the error to us?");

            /*
            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            messageDialog.Commands.Add(new UICommand(
                "Share Error",
                new UICommandInvokedHandler(ExceptionHandler.CommandInvokedHandler)));
            messageDialog.Commands.Add(new UICommand(
                "Cancel",
                new UICommandInvokedHandler(ExceptionHandler.CommandInvokedHandler)));
            */
            // Set the command that will be invoked by default
            //messageDialog.DefaultCommandIndex = 0;

            // Set the command to be invoked when escape is pressed
            //messageDialog.CancelCommandIndex = 1;

            // Show the message dialog
            //await messageDialog.ShowAsync();
        }

        /*
        private static void CommandInvokedHandler(IUICommand command)
        {
            if (command.Label == "Cancel")
            {
                DataTransferManager.GetForCurrentView().DataRequested -= handler;
            }
            else
            {
                DataTransferManager.ShowShareUI();
            }
        }
         */

        private static void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var request = args.Request;
            request.Data.Properties.Title = errormessage.UserMessage;
            request.Data.Properties.Description = errormessage.Info;

            // Share recipe text
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

            request.Data.SetText(builder.ToString());
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