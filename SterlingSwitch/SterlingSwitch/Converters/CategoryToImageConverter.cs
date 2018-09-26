using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace SterlingSwitch.Converters
{
    public class CategoryToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string img = "transfers.png";
            if (value is null) return "";
            int cat = (int)value;
            switch (cat)
            {
                case 1:
                    img = "groceries.png";
                    break;
                case 2:
                    img = "transportation.png";
                    break;
                case 3:
                    img = "eatingOut.png";
                    break;
                case 4:
                case 5:
                    img = "shopping.png";
                    break;
                case 6:
                    img = "give.png";
                    break;
                case 7:
                    img = "billssmall.png";
                    break;
                case 8:
                    img = "education.png";
                    break;
                case 9:
                    img = "transfers.png";
                    break;
                default:
                    img = "transfers.png";
                    break;


            }
            return img;
            //if (cat.Equals("Bills", StringComparison.OrdinalIgnoreCase))
            //{
            //    img = "billssmall.png";
            //}
            //else if(cat.Equals("Transfer", StringComparison.OrdinalIgnoreCase))
            //{
            //    img = "transfers.png";
            //}
            //else if (cat.Equals("Entertainment", StringComparison.OrdinalIgnoreCase))
            //{
            //    img = "entertainment.png";
            //}
            //else if (cat.Equals("Eating", StringComparison.OrdinalIgnoreCase))
            //{
            //    img = "eatingOut.png";
            //}
            //else if (cat.Equals("Travel", StringComparison.OrdinalIgnoreCase))
            //{
            //    img = "travel.png";
            //}
            //else if (cat.Equals("Groceries", StringComparison.OrdinalIgnoreCase))
            //{
            //    img = "groceries.png";
            //}
            //else if (cat.Equals("Transportation", StringComparison.OrdinalIgnoreCase))
            //{
            //    img = "transportation.png";
            //}
            //else if (cat.Equals("Charity", StringComparison.OrdinalIgnoreCase))
            //{
            //    img = "give.png";
            //}
            //else if (cat.Equals("Education", StringComparison.OrdinalIgnoreCase))
            //{
            //    img = "education.png";
            //}
            //else if (cat.Equals("Investment", StringComparison.OrdinalIgnoreCase))
            //{
            //    img = "investment.png";
            //}
            //else if (cat.Equals("Income", StringComparison.OrdinalIgnoreCase))
            //{
            //    img = "income.png";
            //}
            //else if (cat.Equals("Domestic Errands", StringComparison.OrdinalIgnoreCase))
            //{
            //    img = "shopping.png"; 
            //}
            //else if (cat.Equals("Shopping", StringComparison.OrdinalIgnoreCase))
            //{
            //    img = "shopping.png"; 
            //}
            //else
            //{
            //    return img;
            //}
            //return img;

        
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
