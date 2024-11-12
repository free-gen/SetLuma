using System;
using System.Globalization;
using System.Windows.Data;

namespace SetLuma
{
    public class ProgressWidthConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 3 && values[0] is double actualWidth && 
                values[1] is double value && values[2] is double maximum && maximum > 0)
            {
                return actualWidth * (value / maximum);
            }
            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
