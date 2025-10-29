using System.Net.Http.Json;
using SchoolApp.iOS.Models;

namespace SchoolApp.iOS.Services
{
    public class SchoolApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:5178/"; // For iOS Simulator
        // For physical device, use: "http://YOUR_COMPUTER_IP:5178/"

        public SchoolApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl),
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        public async Task<List<Classroom>?> GetClassesAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Classroom>>("api/classes");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error fetching classes: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Student>?> GetStudentsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Student>>("api/students");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error fetching students: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("health");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
