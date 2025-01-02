using ClinicManagementSystem.Services;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;

namespace ClinicManagementSystem.Views.DoctorView
{
    public sealed partial class DocumentPage : Page
    {
        private readonly string folderId = "1gRr54Q86nlZxqH60pCX_QYP-QZQ-U-d4";
        private readonly GoogleDriveService driveService;

        public DocumentPage()
        {
            this.InitializeComponent();
            driveService = new GoogleDriveService();
            LoadFileList();
        }
		/// <summary>
		/// Load danh sách file từ Google Drive
		/// </summary>
		private async void LoadFileList()
        {
            try
            {
                List<string> fileList = await driveService.GetFilesFromFolderAsync(folderId);
                FileListView.ItemsSource = fileList;
            }
            catch (Exception ex)
            {
                FileListView.ItemsSource = new List<string> { $"Lỗi: {ex.Message}" };
            }
        }
		/// <summary>
		/// Hàm xử lí sự kiện khi nhấn vào nút mở Google Drive
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OpenGoogleDrive_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            string folderUrl = $"https://drive.google.com/drive/folders/{folderId}?usp=sharing";
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = folderUrl,
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(psi);
        }
    }
}
