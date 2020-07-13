using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WagsReader.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WagsReader.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        protected HomePageViewModel vm { get; set; }

        public HomePage()
        {
            InitializeComponent();

            vm = new HomePageViewModel
            {
                Navigation = Navigation
            };

            BindingContext = vm;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            //await vm.LoadData();
        }
    }
}
