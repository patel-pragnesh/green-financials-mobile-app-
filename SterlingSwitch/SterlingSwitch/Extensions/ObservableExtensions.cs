using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SterlingSwitch.Extensions
{
   public static class ObservableExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this List<T> items)
        {
            ObservableCollection<T> collection = new ObservableCollection<T>();
            foreach (var item in items)
            {
                collection.Add(item);
            }

            return collection;
        }
    }
}
