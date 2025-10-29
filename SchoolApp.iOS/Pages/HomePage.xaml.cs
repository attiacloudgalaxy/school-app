using SchoolApp.iOS.Services;

namespace SchoolApp.iOS.Pages;

public partial class HomePage : ContentPage
{
    private readonly SchoolApiService _apiService;

    public HomePage(SchoolApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
        
        // Test connection on load
        _ = TestConnectionAsync();
    }

    private async Task TestConnectionAsync()
    {
        try
        {
            var isConnected = await _apiService.TestConnectionAsync();
            
            if (isConnected)
            {
                StatusIcon.Text = "✓";
                StatusLabel.Text = "Connected to API";
                StatusLabel.TextColor = Colors.White;
            }
            else
            {
                StatusIcon.Text = "✗";
                StatusLabel.Text = "API Connection Failed";
                StatusLabel.TextColor = Colors.White;
            }
        }
        catch (Exception ex)
        {
            StatusIcon.Text = "✗";
            StatusLabel.Text = "Connection Error";
            await DisplayAlert("Error", $"Failed to connect: {ex.Message}", "OK");
        }
    }

    private async void OnClassesClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//ClassesPage");
    }

    private async void OnStudentsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//StudentsPage");
    }

    private async void OnTestConnectionClicked(object sender, EventArgs e)
    {
        await TestConnectionAsync();
        await DisplayAlert("Connection Test", StatusLabel.Text, "OK");
    }
}
