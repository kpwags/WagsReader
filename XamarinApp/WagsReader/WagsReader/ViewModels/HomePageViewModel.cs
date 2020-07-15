using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WagsReader.Models;
using WagsReader.Services;
using Xamarin.Essentials;
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
            }
        }

        public HomePageViewModel()
        {
            Users = new ObservableCollection<User>();
        }

        public async Task LoadData()
        {
            await LoadUsers();

            // fetch data if internet is available
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                if (Users.Count > 0)
                {
                    foreach (var user in Users)
                    {
                        switch (user.Type)
                        {
                            case AccountType.Inoreader:
                                await FetchInoreaderData(user);
                                break;
                        }
                    }
                }
            }
        }

        private async Task LoadUsers()
        {
            var users = await User.GetUsersAsync(recursive: true);

            foreach (var user in users)
            {
                Users.Add(user);
            }
        }

        private async Task FetchInoreaderData(User user)
        {
            IsLoading = true;
            LoadingText = "Fetching feeds...";

            var inoreaderService = new InoreaderService();
            await inoreaderService.GetLatestFeedsForUser(user);

            IsLoading = false;
            LoadingText = "";
        }
    }
}
