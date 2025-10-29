using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SchoolApp.iOS.Models;
using SchoolApp.iOS.Services;

namespace SchoolApp.iOS.Pages;

public partial class StudentsPage : ContentPage, INotifyPropertyChanged
{
    private readonly SchoolApiService _apiService;
    private ObservableCollection<Student> _students;
    private ObservableCollection<Student> _filteredStudents;
    private bool _isLoading;
    private bool _isRefreshing;
    private string _searchText;

    public ObservableCollection<Student> Students
    {
        get => _students;
        set
        {
            _students = value;
            OnPropertyChanged();
            FilterStudents();
        }
    }

    public ObservableCollection<Student> FilteredStudents
    {
        get => _filteredStudents;
        set
        {
            _filteredStudents = value;
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

    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
            FilterStudents();
        }
    }

    public bool IsEmpty => FilteredStudents == null || FilteredStudents.Count == 0;

    public ICommand RefreshCommand { get; }

    public StudentsPage(SchoolApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
        _students = new ObservableCollection<Student>();
        _filteredStudents = new ObservableCollection<Student>();
        _searchText = string.Empty;
        
        RefreshCommand = new Command(async () => await LoadStudentsAsync());
        
        BindingContext = this;
        
        // Load data on initialization
        _ = LoadStudentsAsync();
    }

    private async Task LoadStudentsAsync()
    {
        try
        {
            IsLoading = true;

            var students = await _apiService.GetStudentsAsync();
            
            if (students != null)
            {
                Students = new ObservableCollection<Student>(students);
            }
            else
            {
                await DisplayAlert("Error", "Failed to load students from API", "OK");
                Students = new ObservableCollection<Student>();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            Students = new ObservableCollection<Student>();
        }
        finally
        {
            IsLoading = false;
            IsRefreshing = false;
        }
    }

    private void FilterStudents()
    {
        if (Students == null)
        {
            FilteredStudents = new ObservableCollection<Student>();
            return;
        }

        if (string.IsNullOrWhiteSpace(SearchText))
        {
            FilteredStudents = new ObservableCollection<Student>(Students);
        }
        else
        {
            var filtered = Students.Where(s => 
                s.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                s.Id.ToString().Contains(SearchText) ||
                s.ClassroomId.ToString().Contains(SearchText)
            );
            
            FilteredStudents = new ObservableCollection<Student>(filtered);
        }
    }

    public new event PropertyChangedEventHandler PropertyChanged;

    protected new void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
