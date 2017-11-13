using System;
using System.ComponentModel;

namespace WhiteStone.Helpers
{
    /// <summary>
    ///     Utility methods for INotifyPropertyChanged interface
    /// </summary>
    public static class INotifyPropertyChangedHelper
    {
        /// <summary>
        ///     invoke action when propertyName raised
        /// </summary>
        public static void OnPropertyChanged(this INotifyPropertyChanged notifyPropertyChanged, string propertyName, Action action)
        {
            if (notifyPropertyChanged == null)
            {
                throw new ArgumentNullException("notifyPropertyChanged");
            }
            notifyPropertyChanged.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == propertyName)
                {
                    action();
                }
            };
        }

        /// <summary>
        ///     invoke action list when propertyName raised
        /// </summary>
        public static void OnPropertyChanged(this INotifyPropertyChanged notifyPropertyChanged, string propertyName, params Action[] actions)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException("propertyName");
            }

            notifyPropertyChanged.OnPropertyChanged(propertyName, () =>
            {
                foreach (var action in actions)
                {
                    action();
                }
            });
        }
    }
}