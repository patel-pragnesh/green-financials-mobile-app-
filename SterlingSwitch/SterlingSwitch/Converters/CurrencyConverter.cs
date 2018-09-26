using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using SterlingSwitch.Helper;
using Xamarin.Forms;

namespace SterlingSwitch.Converters
{
    public class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            if (parameter == null)
                return string.Empty;

            var amount = value.ToString();
            var currency = parameter as string;
            return $"{Utilities.GetCurrency(currency)} {amount}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }
}
