namespace RealEstateApp.Models;

public class BarometerMeasurement : BaseModel
{
    private double _pressure;
    private double _altitude;
    private string _label;
    private double _heightChange;

    public double Pressure
    {
        get => _pressure;
        set => SetField(ref _pressure, value);
    }

    public double Altitude
    {
        get => _altitude;
        set
        {
            if (SetField(ref _altitude, value)) OnPropertyChanged(nameof(Display));
        }
    }

    public string Label
    {
        get => _label;
        set
        {
            if (SetField(ref _label, value)) OnPropertyChanged(nameof(Display));
        }
    }

    public double HeightChange
    {
        get => _heightChange;
        set => SetField(ref _heightChange, value);
    }

    public string Display => $"{Label}: {Altitude:N2}m";
}