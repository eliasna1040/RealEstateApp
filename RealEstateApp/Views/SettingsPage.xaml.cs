using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstateApp.ViewModels;

namespace RealEstateApp.Views;

public partial class SettingsPage : ContentPage
{
    private SettingsPageViewModel _vm;
    
    public SettingsPage(SettingsPageViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        await _vm.LoadLocales();
        _vm.GetPreferences();
    }

    protected override void OnDisappearing()
    {
        _vm.SetPreferences();
    }
}