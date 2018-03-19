using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MARC2.Model;
using System.Windows.Media;

namespace MARC2.Converter
{
    

    /// <summary>
    /// Scroll View Height Fix for FR classification page and summarize page
    /// </summary>
    public class ScrollViewHalfHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var input = value as System.Double?;
            if (input == 0)
            {
                return input;
            }
            return (null != input ? (input - 140)/2.0 : 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //Debugger.Break();
            return value;
        }
    }

    /// <summary>
    /// Scroll View Height Fix for NFR classification page
    /// </summary>
    public class ScrollViewFourthHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var input = value as System.Double?;
            if (input == 0)
            {
                return input;
            }
            return (null != input ? (input - 240) / 4.0 : 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //Debugger.Break();
            return value;
        }
    }

}
