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
    public partial class InoreaderLogin : ContentPage
    {
        public InoreaderLogin()
        {
            InitializeComponent();
            BindingContext = new InoreaderLoginViewModel();
        }

        private void BackToHomeButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}