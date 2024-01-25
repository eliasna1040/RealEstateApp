using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstateApp.ViewModels;

namespace RealEstateApp.Views;

public partial class ImageListPage : ContentPage
{
    private ImageListPageViewModel _vm;
    
    public ImageListPage(ImageListPageViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        _vm.WatchAccelerometer();
    }

    protected override void OnDisappearing()
    {
        _vm.StopWatchingAccelerometer();
    }
}