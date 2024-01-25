using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstateApp.ViewModels;

namespace RealEstateApp.Views;

public partial class CompassPage : ContentPage
{
    private CompassPageViewModel _vm;
    public CompassPage(CompassPageViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        _vm.WatchCompassCommand();
    }

    protected override void OnDisappearing()
    {
        _vm.StopWatchingCompassCommand();
    }
}