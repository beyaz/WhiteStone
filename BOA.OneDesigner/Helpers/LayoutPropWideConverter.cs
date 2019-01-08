using System;
using System.Globalization;
using System.Windows.Data;

namespace BOA.OneDesigner.Helpers
{
    public class LayoutPropWideConverter : IValueConverter
    {
        #region Public Properties
        public int EmptyStringValue { get; set; } 
        #endregion

        #region Public Methods
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                var s = (string) value;

                var result = 0;

                int.TryParse(s, out result);

                if (result>12)
                {
                    result = 12;
                }

                return result;
            }

            return value;
        }
        #endregion
    }
}