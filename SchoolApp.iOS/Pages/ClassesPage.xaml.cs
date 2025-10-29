using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SchoolApp.iOS.Models;
using SchoolApp.iOS.Services;

namespace SchoolApp.iOS.Pages;

public partial class ClassesPage : ContentPage, INotifyPropertyChanged
{
    private readonly SchoolApiService _apiService;
    private ObservableCollection<Classroom> _classes;
    private bool _isLoading;
    private bool _isRefreshing;

    public ObservableCollection<Classroom> Classes
    {
        get => _classes;
        set
        {
            _classes = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsEmpty));
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }

    public bool IsRefreshing
    {
        get => _isRefreshing;
        set
        {
            _isRefreshing = value;
            OnPropertyChanged();
        }
    }

    public bool IsEmpty => Classes == null || Classes.Count == 0;

    public ICommand RefreshCommand { get; }

    public ClassesPage(SchoolApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
        _classes = new ObservableCollection<Classroom>();
        
        RefreshCommand = new Command(async () => await LoadClassesAsync());
        
        BindingContext = this;
        
        // Load data on initialization
        _ = LoadClassesAsync();
    }

    private async Task LoadClassesAsync()
    {
        try
        {
            IsLoading = true;

            var classes = await _apiService.GetClassesAsync();
            
            if (classes != null)
            {
                Classes = new ObservableCollection<Classroom>(classes);
            }
            else
            {
                await DisplayAlert("Error", "Failed to load classes from API", "OK");
                Classes = new ObservableCollection<Classroom>();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            Classes = new ObservableCollection<Classroom>();
        }
        finally
        {
            IsLoading = false;
            IsRefreshing = false;
        }
    }

    public new event PropertyChangedEventHandler PropertyChanged;

    protected new void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
