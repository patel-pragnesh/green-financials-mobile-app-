using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace SterlingSwitch.Converters
{
    public class CreditDebitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "Gray";

            string valuestring = value as string;

            if (string.Equals(valuestring, "Debit", StringComparison.CurrentCultureIgnoreCase))
            {
                return "Red";
            }
            else if (string.Equals(valuestring, "Credit", StringComparison.CurrentCultureIgnoreCase))
            {
                return "Green";
            }
            else
                return "Gray";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
