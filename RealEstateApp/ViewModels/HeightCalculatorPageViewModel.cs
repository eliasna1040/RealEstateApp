using System.Collections.ObjectModel;
using System.Windows.Input;
using RealEstateApp.Models;

namespace RealEstateApp.ViewModels;

public class HeightCalculatorPageViewModel : BaseViewModel
{
    public ObservableCollection<BarometerMeasurement> MeasurementCollection { get; set; } = new ObservableCollection<BarometerMeasurement>();

    private double _currentPressure;

    public double CurrentPressure
    {
        get => _currentPressure;
        set => SetField(ref _currentPressure, value);
    }

    public double CurrentAltitude
    {
        get => _currentAltitude;
        set => SetField(ref _currentAltitude, value);
    }

    public string MeasurementLabel
    {
        get => _measurementLabel;
        set => SetField(ref _measurementLabel, value, nameof(MeasurementLabel), ((Command)SaveMeasurementCommand).ChangeCanExecute);
    }

    private double _currentAltitude;
    public void WatchBarometerCommand()
    {
        Barometer.Start(SensorSpeed.Game);
        Barometer.ReadingChanged += BarometerReadingChanged;
    }

    private void BarometerReadingChanged(object sender, BarometerChangedEventArgs eventArgs)
    {
        CurrentPressure = eventArgs.Reading.PressureInHectopascals;

        CurrentAltitude = 44307.694 * (1 - Math.Pow(CurrentPressure / 1020, 0.190284));
    }

    private Command _saveMeasurementCommand;
    private string _measurementLabel;
    public ICommand SaveMeasurementCommand => _saveMeasurementCommand ??= new Command(() =>
    {
        BarometerMeasurement measurement = new BarometerMeasurement()
        {
            Altitude = CurrentAltitude,
            Pressure = CurrentPressure,
            Label = MeasurementLabel
        };
        
        if (MeasurementCollection.Any())
        {
            measurement.HeightChange = measurement.Altitude - MeasurementCollection.Last().Altitude;
        }
        
        MeasurementCollection.Add(measurement);
    }, () => !string.IsNullOrWhiteSpace(_measurementLabel));

    public void StopWatchingBarometer()
    {
        Barometer.ReadingChanged -= BarometerReadingChanged;
        Barometer.Stop();
    }
}