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
    public partial class MasterPage : ContentPage
    {
        protected MasterPageViewModel vm { get; set; }

        public MasterPage()
        {
            InitializeComponent();

            vm = new MasterPageViewModel();
            vm.Navigation = Navigation;

            BindingContext = vm;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await vm.LoadData();
        }
    }
}
