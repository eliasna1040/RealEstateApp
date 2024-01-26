using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.Net.Maui;

namespace RealEstateApp.Views;

public partial class BarcodeCameraPage : ContentPage
{
    public BarcodeCameraPage()
    {
        InitializeComponent();
        BarcodeReader.Options = new BarcodeReaderOptions()
        {
            Formats = BarcodeFormat.QrCode,
            Multiple = false,
            AutoRotate = true,
            TryInverted = true,
            TryHarder = true
        };
        BarcodeReader.BarcodesDetected += OnBarcodesDetected;
    }

    private void OnBarcodesDetected(object sender, BarcodeDetectionEventArgs args)
    {
        BarcodeResult result = args.Results.FirstOrDefault();
        if (result == null) return;
        
        BarcodeReader.IsDetecting = false;
        Browser.OpenAsync(result.Value).Wait();
    }
    
    

    protected override void OnDisappearing()
    {
        BarcodeReader.BarcodesDetected -= OnBarcodesDetected;
    }
}