using WagsReader.Interfaces;
using WagsReader.UWP;

[assembly: Xamarin.Forms.Dependency(typeof(UWPDeviceInfo))]

namespace WagsReader.UWP
{
    public class UWPDeviceInfo : IDeviceInfo
    {
        public string GetInoreaderOAuthRedirectUrl()
        {
            return "com.kpwags.wagsreader://oauth2redirect/";
        }

        public bool IsUsingNativeUI()
        {
            return true;
        }
    }
}
