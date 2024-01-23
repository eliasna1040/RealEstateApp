using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RealEstateApp.Models;
public class PropertyListItem : BaseModel
{
    public PropertyListItem(Property property)
    {
        Property = property;
    }

    private Property _property;
    private double _distance;

    public Property Property
    {
        get => _property;
        set
        {
            SetField(ref _property, value);
        }
    }

    public double Distance
    {
        get => _distance;
        set => SetField(ref _distance, value);
    }
}
