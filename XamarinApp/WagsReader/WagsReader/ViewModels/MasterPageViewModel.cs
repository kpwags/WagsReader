using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WagsReader.Classes;
using WagsReader.Models;
using WagsReader.Views;

namespace WagsReader.ViewModels
{
    public class MasterPageViewModel : BaseViewModel
    {
        private ObservableCollection<MasterPageItem> _menuItems;
        public ObservableCollection<MasterPageItem> MenuItems
        {
            get { return _menuItems;  }
            set
            {
                _menuItems = value;
                OnPropertyChanged(nameof(MenuItems));
            }
        }

        public MasterPageViewModel()
        {
            MenuItems = new ObservableCollection<MasterPageItem>();
            
        }

        public async Task LoadData()
        {
            MenuItems.Add(new MasterPageItem { Title = "Home", TargetType = typeof(HomePage) });

            await LoadAccounts();

            MenuItems.Add(new MasterPageItem { Title = "Add Account", TargetType = typeof(AddAccountPage) });
        }

        private async Task LoadAccounts()
        {
            var users = await User.GetUsersAsync(true);

            foreach (var user in users)
            {
                MenuItems.Add(new MasterPageItem { Title = user.AccountName, TargetType = typeof(AddAccountPage) });

                if (user.Folders != null)
                {
                    foreach (var folder in user.Folders.OrderBy(f => f.Name))
                    {
                        MenuItems.Add(new MasterPageItem { Title = folder.Name, TargetType = typeof(AddAccountPage), Level = 2, UnreadCount = folder.UnreadCount });
                    }
                }
            }
        }
    }
}
