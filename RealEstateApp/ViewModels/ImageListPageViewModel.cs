using System.Collections.ObjectModel;
using System.Windows.Input;
using RealEstateApp.Models;

namespace RealEstateApp.ViewModels;

[QueryProperty(nameof(Property), nameof(Models.Property))]
public class ImageListPageViewModel : BaseViewModel
{
    public ObservableCollection<string> ImageUrlCollection { get; set; } = new ObservableCollection<string>();

    public Property Property
    {
        set
        {
            if (ImageUrlCollection.Any())
            {
                ImageUrlCollection.Clear();
            }

            value.ImageUrls.ForEach(x => ImageUrlCollection.Add(x));
        }
    }

    public int Position
    {
        get => _position;
        set => SetField(ref _position, value);
    }

    private int _position;

    public void WatchAccelerometer()
    {
        Accelerometer.Start(SensorSpeed.Game);
        Accelerometer.ShakeDetected += ShakeDetected;
    }

    public void StopWatchingAccelerometer()
    {
        Accelerometer.ShakeDetected -= ShakeDetected;
        Accelerometer.Stop();
    }

    private void ShakeDetected(object sender, EventArgs eventArgs)
    {
        if (_position == ImageUrlCollection.Count - 1)
        {
            Position = 0;
        }
        else
        {
            Position++;
        }
    }
}