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
    public partial class Home : ContentPage
    {
        public Home()
        {
            InitializeComponent();
            BindingContext = new HomeViewModel();
        }
    }
}