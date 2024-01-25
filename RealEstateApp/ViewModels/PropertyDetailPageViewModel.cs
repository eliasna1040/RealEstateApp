using RealEstateApp.Models;
using RealEstateApp.Services;
using RealEstateApp.Views;
using System.Windows.Input;

namespace RealEstateApp.ViewModels;

[QueryProperty(nameof(PropertyListItem), "MyPropertyListItem")]
public class PropertyDetailPageViewModel : BaseViewModel
{
    private readonly IPropertyService service;

    public PropertyDetailPageViewModel(IPropertyService service)
    {
        this.service = service;
    }

    Property property;

    public Property Property
    {
        get => property;
        set { SetField(ref property, value); }
    }


    Agent agent;

    public Agent Agent
    {
        get => agent;
        set { SetField(ref agent, value); }
    }


    PropertyListItem propertyListItem;

    public PropertyListItem PropertyListItem
    {
        set
        {
            SetField(ref propertyListItem, value);

            Property = propertyListItem.Property;
            Agent = service.GetAgents().FirstOrDefault(x => x.Id == Property.AgentId);
        }
    }

    public bool TextoSpeechIsEnabled
    {
        get => _textoSpeechIsEnabled;
        set
        {
            if (value == _textoSpeechIsEnabled) return;
            SetField(ref _textoSpeechIsEnabled, value);
        }
    }

    private Command editPropertyCommand;

    public ICommand EditPropertyCommand => editPropertyCommand ??= new Command(async () =>
    {
        await Shell.Current.GoToAsync($"{nameof(AddEditPropertyPage)}?mode=editproperty", true,
            new Dictionary<string, object>
            {
                { "MyProperty", property }
            });
    });

    private CancellationTokenSource cts;

    private Command _playTextToSpeech;

    public ICommand PlayTextToSpeech => _playTextToSpeech ??= new Command(async () =>
    {
        cts = new CancellationTokenSource();
        TextoSpeechIsEnabled = true;
        await TextToSpeech.SpeakAsync(Property.Description, cts.Token);
        TextoSpeechIsEnabled = false;
    });

    private Command _stopTextToSpeech;
    private bool _textoSpeechIsEnabled;

    public ICommand StopTextToSpeech => _stopTextToSpeech ??= new Command(async () =>
    {
        if (cts.IsCancellationRequested)
            return;
        await cts.CancelAsync();
        TextoSpeechIsEnabled = false;
    });

    private Command _goToImageListPageCommand;

    public ICommand GoToImageListPageCommand => _goToImageListPageCommand ??= new Command(async () =>
    {
        ShellNavigationQueryParameters param = new ShellNavigationQueryParameters()
        {
            { nameof(Models.Property), Property }
        };

        await Shell.Current.GoToAsync(nameof(ImageListPage), param);
    });
}