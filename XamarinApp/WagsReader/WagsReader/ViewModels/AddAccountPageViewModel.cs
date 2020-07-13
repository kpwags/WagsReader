using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using WagsReader.Models;
using WagsReader.Services;
using WagsReader.Services.Interfaces;
using Xamarin.Auth;
using Xamarin.Auth.Presenters;
using Xamarin.Forms;

namespace WagsReader.ViewModels
{
    public class AddAccountPageViewModel : BaseViewModel
    {
        public OAuthLoginPresenter Presenter;
        public IRSSService RSSService;

        #region Bindings
        private int _currentStep;
        public int CurrentStep
        {
            get { return _currentStep; }
            set
            {
                _currentStep = value;
                OnPropertyChanged(nameof(CurrentStep));
                OnPropertyChanged(nameof(ShowStep1));
                OnPropertyChanged(nameof(ShowStep2));
            }
        }

        public bool ShowStep1
        {
            get { return _currentStep == 1; }
        }

        public bool ShowStep2
        {
            get { return _currentStep == 2; }
        }

        private User _user;
        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        private string _accountName;
        public string AccountName
        {
            get { return _accountName; }
            set
            {
                _accountName = value;
                OnPropertyChanged(nameof(AccountName));
            }
        }
        #endregion

        #region Command Properties
        private ICommand _loginToInoreader;
        public ICommand LoginToInoreader
        {
            get
            {
                if (_loginToInoreader == null)
                {
                    _loginToInoreader = new Command(OnLoginToInoreader);
                }

                return _loginToInoreader;
            }
        }

        private ICommand _saveAccount;
        public ICommand SaveAccount
        {
            get
            {
                if (_saveAccount == null)
                {
                    _saveAccount = new Command(SaveAccountInfo);
                }

                return _saveAccount;
            }
        }
        #endregion

        public AddAccountPageViewModel()
        {
            Presenter = new OAuthLoginPresenter();
            CurrentStep = 1;
        }

        protected async void OnLoginToInoreader()
        {
            try
            {
                RSSService = new InoreaderService();
                User = await RSSService.Login();

                if (User != null)
                {
                    AccountName = User.AccountName;
                    CurrentStep = 2;
                }
            }
            catch (AuthException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Auth Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error Handling OAuth: {ex.Message}");
            }
        }

        protected async void SaveAccountInfo()
        {
            try
            {
                var user = await User.GetUserAsync(User.UserId);
                user.AccountName = AccountName;

                User = await User.Save(user);

                AccountName = "";
                CurrentStep = 1;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving user account name: {ex.Message}");
            }
        }
    }
}
