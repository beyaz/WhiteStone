using System;
using System.Globalization;
using System.Windows.Data;

namespace WhiteStone.UI.Container
{
    public class StringToNullableInt32Converter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var intValue = 0;

            if (int.TryParse(value.ToString(), out intValue))
            {
                return intValue;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(value.ToString()))
            {
                return null;
            }

            return System.Convert.ToString(value);
        }
        #endregion
    }
}