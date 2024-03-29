﻿using RealEstateApp.Models;
using RealEstateApp.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RealEstateApp.ViewModels;

[QueryProperty(nameof(Mode), "mode")]
[QueryProperty(nameof(Property), "MyProperty")]
public class AddEditPropertyPageViewModel : BaseViewModel
{
    readonly IPropertyService service;

    public AddEditPropertyPageViewModel(IPropertyService service)
    {
        this.service = service;
        Agents = new ObservableCollection<Agent>(service.GetAgents());
        if (DeviceInfo.VersionString != "14")
        {
            Connectivity.ConnectivityChanged += (sender, args) => ((Command)SetLocationByAddress).ChangeCanExecute();
        }
    }

    public string Mode { get; set; }

    #region PROPERTIES

    public ObservableCollection<Agent> Agents { get; }

    private Property _property;

    public Property Property
    {
        get => _property;
        set
        {
            SetProperty(ref _property, value);
            Title = Mode == "newproperty" ? "Add Property" : "Edit Property";

            if (_property.AgentId != null)
            {
                SelectedAgent = Agents.FirstOrDefault(x => x.Id == _property?.AgentId);
            }
        }
    }

    private Agent _selectedAgent;

    public Agent SelectedAgent
    {
        get => _selectedAgent;
        set
        {
            if (Property != null)
            {
                _selectedAgent = value;
                Property.AgentId = _selectedAgent?.Id;
            }
        }
    }

    string statusMessage;

    public string StatusMessage
    {
        get { return statusMessage; }
        set { SetProperty(ref statusMessage, value); }
    }

    Color statusColor;

    public Color StatusColor
    {
        get { return statusColor; }
        set { SetProperty(ref statusColor, value); }
    }

    #endregion


    private Command savePropertyCommand;
    public ICommand SavePropertyCommand => savePropertyCommand ??= new Command(async () => await SaveProperty());

    private async Task SaveProperty()
    {
        if (IsValid() == false)
        {
            Vibration.Vibrate(TimeSpan.FromSeconds(3));
            StatusMessage = "Please fill in all required fields";
            StatusColor = Colors.Red;
        }
        else
        {
            service.SaveProperty(Property);
            await Shell.Current.GoToAsync("///propertylist");
        }
    }

    public bool IsValid()
    {
        if (string.IsNullOrEmpty(Property.Address)
            || Property.Beds == null
            || Property.Price == null
            || Property.AgentId == null)
            return false;
        return true;
    }

    private Command cancelSaveCommand;

    public ICommand CancelSaveCommand =>
        cancelSaveCommand ??= new Command(async () =>
        {
            Vibration.Cancel();
            await Shell.Current.GoToAsync("..");
        });

    private Command _getCurrentLocationCommand;

    public ICommand GetCurrentLocationCommand => _getCurrentLocationCommand ??= new Command(async () =>
    {
        try
        {
            Location location = await Geolocation.GetLastKnownLocationAsync();
            Property.Latitude = location?.Latitude;
            Property.Longitude = location?.Longitude;

            if (location != null)
            {
                Property.Address = (await Geocoding.GetPlacemarksAsync(location)).Select(x =>
                        $"{x.Thoroughfare} {x.FeatureName}, {x.Locality} {x.PostalCode}, {x.CountryName}")
                    .First();
            }
        }
        catch (PermissionException e)
        {
            await Shell.Current.DisplayAlert("Permissions needed", "Grant access to location to use this feature",
                "Close");
        }
    });

    private Command _setLocationByAddress;

    public ICommand SetLocationByAddress => _setLocationByAddress ??= new Command(async () =>
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Property.Address))
                {
                    await Shell.Current.DisplayAlert("Hey", "Pls enter an address", "Ok");
                    return;
                }

                Location location = (await Geocoding.GetLocationsAsync(Property.Address)).First();
                Property.Longitude = location.Longitude;
                Property.Latitude = location.Latitude;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        },
        () => Connectivity.Current.NetworkAccess == NetworkAccess.Internet &&
              Connectivity.ConnectionProfiles.Contains(ConnectionProfile.WiFi));
}