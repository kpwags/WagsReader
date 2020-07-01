using System;
using WagsReader.Services;
using WagsReader.Services.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WagsReader
{
    public partial class App : Application
    {
        //public static INavigationService NavigationService { get; } = new NavigationService();

        public App()
        {
            InitializeComponent();

            //NavigationService.Configure("Home", typeof(Views.Home));
            //NavigationService.Configure("InoreaderLogin", typeof(Views.InoreaderLogin));

            //var homePage = ((NavigationService)NavigationService).SetRootPage("Home");

            MainPage = new NavigationPage(new Views.Home());
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
    }
}
