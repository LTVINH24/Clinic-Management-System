using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using ClinicManagementSystem.Service;

namespace ClinicManagementSystem.Services
{
	/// <summary>
	/// Lớp GoogleDriveService chứa các phương thức để tương tác với Google Drive
	/// </summary>
	public class GoogleDriveService
    {
        private readonly string _apiKey;

		public GoogleDriveService()
		{
            _apiKey = ConfigurationManager.AppSetting
                .GetSection("GoogleDriveSettings:ApiKey").Value;
		}

		public async Task<List<string>> GetFilesFromFolderAsync(string folderId)
        {
            string apiUrl = $"https://www.googleapis.com/drive/v3/files?q='{folderId}'+in+parents&key={_apiKey}";

            using HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                JsonDocument document = JsonDocument.Parse(jsonResponse);

                List<string> fileNames = new List<string>();
                foreach (var file in document.RootElement.GetProperty("files").EnumerateArray())
                {
                    fileNames.Add(file.GetProperty("name").GetString());
                }

                return fileNames;
            }

            return new List<string> { "Không thể tải danh sách file" };
        }
    }
}
