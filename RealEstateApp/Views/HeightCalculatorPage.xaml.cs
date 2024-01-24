using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstateApp.ViewModels;

namespace RealEstateApp.Views;

public partial class HeightCalculatorPage : ContentPage
{
    private HeightCalculatorPageViewModel _vm;
    
    public HeightCalculatorPage(HeightCalculatorPageViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _vm = vm;
    }

    protected override void OnAppearing()
    {
        _vm.WatchBarometerCommand.Execute(null);
    }

    protected override void OnDisappearing()
    {
        _vm.StopWatchingBarometer.Execute(null);
    }
}