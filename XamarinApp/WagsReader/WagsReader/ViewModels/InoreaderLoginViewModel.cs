using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace WagsReader.ViewModels
{
    public class InoreaderLoginViewModel : BaseViewModel
    {
        private string _username;
        public string Username
        {
            get { return _username; }
            set { _username = value;  }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        private ICommand _loginToInoreader;
        public ICommand LoginToInoreader
        {
            get { return _loginToInoreader ?? (_loginToInoreader = new Command(OnLogin)); }
        }

        public InoreaderLoginViewModel()
        {

        }

        protected void OnLogin()
        {
            
        }
    }
}
