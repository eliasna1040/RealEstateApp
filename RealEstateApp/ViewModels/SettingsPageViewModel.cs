using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RealEstateApp.ViewModels;

public class SettingsPageViewModel : BaseViewModel
{
    private const double _defaultVolume = 0.5;
    private const double _defaultPitch = 1;
    
    private double _pitch;
    private double _volume;
    private Locale _locale;

    public double Volume
    {
        get => _volume;
        set => SetField(ref _volume, value);
    }

    public double Pitch
    {
        get => _pitch;
        set => SetField(ref _pitch, value);
    }

    public Locale Locale
    {
        get => _locale;
        set => SetField(ref _locale, value);
    }

    public ObservableCollection<Locale> Locales { get; set; } = new ObservableCollection<Locale>();

    public void SetPreferences()
    {
        Preferences.Set(nameof(Volume), Volume);
        Preferences.Set(nameof(Pitch), Pitch);
    }

    public void GetPreferences()
    {
        Volume = Preferences.Get(nameof(Volume), _defaultVolume);
        Pitch = Preferences.Get(nameof(Pitch), _defaultPitch);
        Locale = Locales.FirstOrDefault(x => x.Id == Preferences.Get(nameof(Locale), Locales.FirstOrDefault()?.Id));
    }

    public async Task LoadLocales()
    {
        if (Locales.Any())
        {
            Locales.Clear();
        }

        foreach (Locale locale in await TextToSpeech.GetLocalesAsync())
        {
            Locales.Add(locale);
        }
    }

    private Command _resetSettingsCommand;

    public ICommand ResetSettingsCommand => _resetSettingsCommand ??= new Command(() =>
    {
        Volume = _defaultVolume;
        Pitch = _defaultPitch;
    });
}