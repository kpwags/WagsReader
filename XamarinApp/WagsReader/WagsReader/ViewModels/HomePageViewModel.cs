using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WagsReader.Models;
using Xamarin.Forms;

namespace WagsReader.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
                OnPropertyChanged(nameof(ShowAccountList));
            }
        }

        public bool ShowAccountList
        {
            get
            {
                if (Users.Count > 1)
                {
                    return true;
                }

                return false;
            }
        }

        private ICommand _linkRSSAccount;
        public ICommand LinkRSSAccount
        {
            get
            {
                return _linkRSSAccount ?? (_linkRSSAccount = new Command(OnLinkRSSAccount));
            }
        }

        public HomePageViewModel()
        {
            Users = new ObservableCollection<User>();
        }

        public async Task LoadData()
        {
            await LoadUsers();
        }

        private async Task LoadUsers()
        {
            var users = await User.GetUsersAsync();

            foreach (var user in users)
            {
                Users.Add(user);
            }

            OnPropertyChanged(nameof(ShowAccountList));
        }

        protected async void OnLinkRSSAccount()
        {
            await Navigation.PushAsync(new Views.AddAccountPage());
        }
    }
}
