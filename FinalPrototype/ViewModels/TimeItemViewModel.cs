using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;

namespace FinalPrototype.ViewModels;

/// <summary>
/// A view model representing a chargeable row
/// </summary>
public partial class TimeItemViewModel(string projectId) : ObservableObject
{
	
	#region Fields

	[ObservableProperty] private double _firstEntry;
	[ObservableProperty] private double _secondEntry;
	[ObservableProperty] private double _thirdEntry;
	[ObservableProperty] private double _fourthEntry;
	[ObservableProperty] private double _fifthEntry;
	
	#endregion Fields

	#region Properties

	public string ProjectId
	{
		get => projectId;
		set => SetProperty(ref projectId, value);
	}

	#endregion Properties
}