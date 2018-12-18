using System;
using System.Globalization;
using System.Windows.Data;

namespace BOA.Jaml
{
    public class ConverterClass : IValueConverter
    {
        #region Public Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}