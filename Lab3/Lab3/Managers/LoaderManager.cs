using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab3.Managers
{
    class LoaderManager
    {
        #region Fields
        private static readonly object _locker = new object();
        private static LoaderManager? _instance;
        private ILoaderOwner _loaderOwner;
        #endregion

        #region Properties
        public static LoaderManager Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                lock (_locker)
                {
                    return _instance ??= new LoaderManager();
                }
            }
        }
        #endregion

        private LoaderManager() 
        { }

        public void Initialize(ILoaderOwner loaderOwner)
        {
            _loaderOwner = loaderOwner;
        }

        public void ShowLoader()
        {
            _loaderOwner.IsEnabled = false;
            _loaderOwner.LoaderVisibility = Visibility.Visible;
        }

        public void HideLoader()
        {
            _loaderOwner.IsEnabled = true;
            _loaderOwner.LoaderVisibility = Visibility.Collapsed;
        }
    }
}
