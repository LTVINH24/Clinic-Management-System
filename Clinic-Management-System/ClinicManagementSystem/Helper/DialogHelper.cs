using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Windows.Foundation;

namespace ClinicManagementSystem.Helper
{
    public static class DialogHelper
    {
        public static IAsyncOperation<ContentDialogResult> ShowMessage(string title, string message, XamlRoot xamlRoot)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = xamlRoot
            };

            return dialog.ShowAsync();
        }
    }
} 