using System;
using System.Threading.Tasks;
using WagsReader.Interfaces;
using WagsReader.Services;
using WagsReader.Services.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using WagsReader.Models;

namespace WagsReader
{
    public partial class App : Application
    {
        private Session _userSession;
        public Session UserSession
        {
            get { return _userSession; }
            set
            {
                if (_userSession == value)
                    return;

                _userSession = value;
                OnPropertyChanged(nameof(UserSession));
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new Views.MainPage ();

            Task.Run(async () =>
            {
                await CreateDatabaseTablesAsync();
            });
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private async Task CreateDatabaseTablesAsync()
        {
            SQLiteConnection db = DependencyService.Get<IDatabaseConnection>().DBConnection();
            await Task.Run(() =>
            {
                db.RunInTransaction(() =>
                {
                    db.CreateTable<User>();
                    db.CreateTable<UserToken>();
                    db.CreateTable<Folder>();
                });
            });
        }
    }
}
