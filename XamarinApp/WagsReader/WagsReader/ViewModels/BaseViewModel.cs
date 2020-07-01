using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WagsReader.Services.Interfaces;

namespace WagsReader.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //public INavigationService NavigationService { get; } = App.NavigationService;

        public Func<object, bool> AllowNavigation => ShouldAllowNavigation;

        private bool _canNavigate;
        public bool CanNavigate
        {
            get
            {
                return _canNavigate;
            }

            set
            {
                _canNavigate = value;
                OnPropertyChanged(nameof(CanNavigate));
            }
        }

        protected BaseViewModel()
        {

        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(propertyName));
        }

        protected bool ShouldAllowNavigation(object arg)
        {
            return CanNavigate;
        }

        
    }
}
