using IdentityModel.Client;
using System;
using System.Windows.Input;
using WagsReader.Services;
using WagsReader.Services.Interfaces;
using Xamarin.Auth;
using Xamarin.Auth.Presenters;
using Xamarin.Forms;

namespace WagsReader.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private ICommand _loginToInoreader;
        public ICommand LoginToInoreader
        {
            get
            {
                return _loginToInoreader ?? (_loginToInoreader = new Command(OnLoginToInoreader));
            }
        }

        public OAuthLoginPresenter Presenter;
        public IRSSService RSSService;

        public HomeViewModel()
        {
            Presenter = new OAuthLoginPresenter();
        }

        protected async void OnLoginToInoreader()
        {
            try
            {
                RSSService = new InoreaderService();
                var user = await RSSService.Login();

                if (user != null)
                {
                    
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
    }
}
