using System;
using WagsReader.Classes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WagsReader.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();

            masterPage.listView.ItemSelected += OnItemSelected;

            if (Device.RuntimePlatform == Device.UWP)
            {
                MasterBehavior = MasterBehavior.Popover;
            }
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            switch (e.SelectedItem)
            {
                case MasterPageItem item:
                    Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                    masterPage.listView.SelectedItem = null;
                    IsPresented = false;
                    break;
            }
        }
    }
}
