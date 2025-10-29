using System.Net.Http.Json;
using SchoolApp.Models;

namespace SchoolApp.Services;

public class SchoolApiService
{
    private readonly HttpClient _httpClient;

    public SchoolApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Classroom>?> GetClassesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Classroom>>("api/classes");
    }

    public async Task<List<Student>?> GetStudentsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Student>>("api/students");
    }
}
