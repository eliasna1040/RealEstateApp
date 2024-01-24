using System.Globalization;

namespace RealEstateApp.Converters;

public class EmptyDoubleConverter : IValueConverter, IMarkupExtension
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (double.TryParse(value?.ToString(), out double valueAsDouble) && valueAsDouble == 0) return string.Empty;
        
        return $" ({(!double.IsNegative(valueAsDouble) ? '+' : "")}{valueAsDouble:F2})";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }

    public object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}