using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab3.Managers
{
    interface ILoaderOwner : INotifyPropertyChanged
    {
        #region Properties
        public bool IsEnabled { get; set; }
        public Visibility LoaderVisibility { get; set; }
        #endregion
    }
}
