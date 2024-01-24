using System.Windows.Input;
using RealEstateApp.Models;

namespace RealEstateApp.ViewModels;

[QueryProperty(nameof(Property), nameof(Models.Property))]
public class CompassPageViewModel : BaseViewModel
{
    private Property _property;

    public Property Property
    {
        get => _property;
        set => SetField(ref _property, value);
    }

    public double CurrentHeading
    {
        get => _currentHeading;
        set => SetField(ref _currentHeading, value);
    }

    public double Rotation
    {
        get => _rotation;
        set => SetField(ref _rotation, value);
    }

    public string CurrentAspect
    {
        get => _currentAspect;
        set => SetField(ref _currentAspect, value);
    }

    private Command _watchCompassCommand;
    private Command _stopWatchingCompassCommand;
    private double _currentHeading;
    private double _rotation;
    private string _currentAspect;

    public ICommand WatchCompassCommand => _watchCompassCommand ??= new Command(() =>
    {
        Compass.Start(SensorSpeed.Game);
        Compass.ReadingChanged += CompassReadingChanged;
    });

    public ICommand StopWatchingCompassCommand => _stopWatchingCompassCommand ??= new Command(() =>
    {
        Compass.ReadingChanged -= CompassReadingChanged;
        Compass.Stop();
        Property.Aspect = CurrentAspect;
    });

    public void CompassReadingChanged(object sender, CompassChangedEventArgs eventArgs)
    {
        CurrentHeading = eventArgs.Reading.HeadingMagneticNorth;
        Rotation = -CurrentHeading;
        CurrentAspect = CurrentHeading switch
        {
            >= 337.5 or <= 22.5 => "North",
            >= 22.5 and <= 67.5 => "Northeast",
            >= 67.5 and <= 112.5 => "East",
            >= 112.5 and <= 157.5 => "Southeast",
            >= 157.5 and <= 202.5 => "South",
            >= 202.5 and <= 247.5 => "Southwest",
            >= 247.5 and <= 292.5 => "West",
            >= 292.5 and <= 337.5 => "Northwest",
            _ => "Unknown"
        };
    }
}