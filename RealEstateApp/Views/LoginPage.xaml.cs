using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstateApp.ViewModels;

namespace RealEstateApp.Views;

public partial class LoginPage : ContentPage
{
    private LoginPageViewModel _vm;
    
    public LoginPage(LoginPageViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        await _vm.LoadTokens();
    }
}