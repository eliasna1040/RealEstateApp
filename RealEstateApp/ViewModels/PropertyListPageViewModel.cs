using RealEstateApp.Models;
using RealEstateApp.Services;
using RealEstateApp.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace RealEstateApp.ViewModels;

public class PropertyListPageViewModel : BaseViewModel
{
    public ObservableCollection<PropertyListItem> PropertiesCollection { get; } = new();

    private readonly IPropertyService service;

    public PropertyListPageViewModel(IPropertyService service)
    {
        Title = "Property List";
        this.service = service;
    }

    bool isRefreshing;

    public bool IsRefreshing
    {
        get => isRefreshing;
        set => SetField(ref isRefreshing, value);
    }

    private bool sortByDistance = false;

    private Command getPropertiesCommand;

    public ICommand GetPropertiesCommand =>
        getPropertiesCommand ??= new Command(async () => await GetPropertiesAsync());

    async Task GetPropertiesAsync()
    {
        if (IsBusy)
            return;
        try
        {
            IsBusy = true;

            List<Property> properties = service.GetProperties();

            Location currentLocation =
                await Geolocation.Default.GetLastKnownLocationAsync() ?? await Geolocation.GetLocationAsync();


            IEnumerable<PropertyListItem> propertyListItems = properties.Select(x => new PropertyListItem(x)
            {
                Distance = currentLocation.CalculateDistance(new Location(x.Latitude.Value, x.Longitude.Value),
                    DistanceUnits.Kilometers)
            });

            if (sortByDistance)
            {
                propertyListItems = propertyListItems.OrderBy(x => x.Distance);
            }

            if (PropertiesCollection.Count != 0)
                PropertiesCollection.Clear();
            
            foreach (PropertyListItem item in propertyListItems)
            {
                PropertiesCollection.Add(item);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get monkeys: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    private Command _goToDetailsCommand;

    public ICommand GoToDetailsCommand => _goToDetailsCommand ??=
        new Command(async (property) => await GoToDetails((PropertyListItem)property));

    async Task GoToDetails(PropertyListItem propertyListItem)
    {
        if (propertyListItem == null)
            return;

        await Shell.Current.GoToAsync(nameof(PropertyDetailPage), true, new Dictionary<string, object>
        {
            { "MyPropertyListItem", propertyListItem }
        });
    }

    private Command goToAddPropertyCommand;

    public ICommand GoToAddPropertyCommand => goToAddPropertyCommand ??= new Command(async () =>
    {
        await Shell.Current.GoToAsync($"{nameof(AddEditPropertyPage)}?mode=newproperty", true,
            new Dictionary<string, object>
            {
                { "MyProperty", new Property() }
            });
    });

    private Command _sortPropertiesByDistance;

    public ICommand SortPropertiesByDistance => _sortPropertiesByDistance ??= new Command(async () =>
    {
        sortByDistance = true;
        await GetPropertiesAsync();
    });
}