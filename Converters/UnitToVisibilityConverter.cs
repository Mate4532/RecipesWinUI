using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using RecipesWinUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipesWinUI.Converters
{
    public class UnitToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is MeasurementUnit unit &&
                parameter is string target &&
                Enum.TryParse<MeasurementUnit>(target, out var expected))
            {
                return unit == expected ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
