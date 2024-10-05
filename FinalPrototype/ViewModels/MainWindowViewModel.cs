using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;

namespace FinalPrototype.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    #region Attributes

    private Timer _timer = new();

    private double _counter;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ApplyTimeCommand))]
    private bool _canApplyTime;

    private ObservableCollection<TimeItemViewModel> _items = [];

    private ObservableCollection<string> _selectableProjects =
    [
        "Project A",
        "Project B",
        "Project C",
        "Project D",
        "Project E"
    ];
    private string? _selectedProject;
    
    private TimeItemViewModel? _selectedRow;

    private string _trackTimeString = "Select project in table to track your time!";

    [ObservableProperty] 
    [NotifyCanExecuteChangedFor(nameof(StartTimeCommand))]
    [NotifyCanExecuteChangedFor(nameof(StopTimeCommand))]
    private bool _isRowSelected;

    #endregion Attributes
    
    #region Properties

    /// <summary>
    /// Get a string for start time button
    /// </summary>
    public string StartTimeString => "Start Time";

    /// <summary>
    /// Get a string for stop time button
    /// </summary>
    public string StopTimeString => "Stop Time";

    public string TrackTimeString
    {
        get => _trackTimeString;
        set => SetProperty(ref _trackTimeString, value);
    }
    

    /// <summary>
    /// currently selected row
    /// </summary>
    public TimeItemViewModel? SelectedRow
    {
        get => _selectedRow;
        set
        {
            SetProperty(ref _selectedRow, value);
            TrackTimeString = $"Track your time for selected project {_selectedRow?.ProjectId}";
            IsRowSelected = value != null;
            OnPropertyChanged(nameof(IsRowSelected));
        }
    }

    public ObservableCollection<TimeItemViewModel> Items => _items;
    
    /// <summary>
    /// The selectable projects for this row
    /// </summary>
    public ObservableCollection<string> SelectableProjects
    {
        get => _selectableProjects;
        set => SetProperty(ref _selectableProjects, value);
    }

    /// <summary>
    /// The currently selected project
    /// </summary>
    public string? SelectedProject
    {
        get => _selectedProject;
        set => SetProperty(ref _selectedProject, value);
    }

    public double Counter
    {
        get => _counter;
        set => SetProperty(ref _counter, value);
    }

    #endregion Properties
    
    #region Constructor

    public MainWindowViewModel()
    {
        SelectedProject = "Project A";
        _timer.Interval = 1000;
        _timer.Elapsed += TimerOnElapsed;
    }

    #endregion Constructor
    
    #region Methods

    /// <summary>
    /// Add a line to the current timesheet
    /// </summary>
    [RelayCommand()]
    private void AddLine()
    {
        if (_selectedProject == null)
        {
            return;
        }

        TimeItemViewModel item = new(_selectedProject);
        Items.Add(item);
        SelectableProjects.Remove(_selectedProject);
        SelectedProject = SelectableProjects.FirstOrDefault();
    }

    [RelayCommand(CanExecute = nameof(IsRowSelected))]
    private void StartTime()
    {
        _timer.Start();
        CanApplyTime = false;
    }
    
    [RelayCommand(CanExecute = nameof(IsRowSelected))]
    private void StopTime()
    {
        _timer.Stop();
        CanApplyTime = true;
    }

    [RelayCommand(CanExecute = nameof(CanApplyTime))]
    private void ApplyTime()
    {
        if (SelectedRow == null)
        {
            return;
        }
        SelectedRow.FirstEntry = Counter;
        Counter = 0;
    }
    
    private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        Counter += 0.1;
    }
    
    #endregion Methods

}
