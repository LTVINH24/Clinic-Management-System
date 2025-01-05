using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Windows.Foundation;

namespace ClinicManagementSystem.Helper
{
    /// <summary>
    /// Helper cho các dialog
    /// </summary>
    public static class DialogHelper
    {
        /// <summary>
        /// Hiển thị dialog thông báo
        /// </summary>
        /// <param name="title">Tiêu đề dialog</param>
        /// <param name="message">Nội dung dialog</param>
        /// <param name="xamlRoot">XamlRoot</param>
        /// <returns>Kết quả dialog</returns>
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