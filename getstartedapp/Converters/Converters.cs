using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace getstartedapp.Converters;

public sealed class BoolToOpacityConverter : IValueConverter
{
    public object Convert(object? value, Type t, object? p, CultureInfo c)
        => value is true ? 0.6 : 1.0;
    
    public object ConvertBack(object? v, Type t, object? p, CultureInfo c)
        => Avalonia.Data.BindingOperations.DoNothing;
}