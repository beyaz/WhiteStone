using System;
using System.ComponentModel;

namespace WhiteStone.Common
{
    /// <summary>
    ///     Base class of <see cref="INotifyPropertyChanged" /> property required classes.
    /// </summary>
    [Serializable]
    public abstract class ContractBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members
        /// <summary>
        ///     Notifies clients that a property value has changed.
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Notifies clients that a property value has changed.
        /// </summary>
        /// <param name="prop"></param>
        public virtual void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        #endregion
    }
}