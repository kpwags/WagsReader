using WagsReader.Interfaces;
using WagsReader.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(IOSDeviceInfo))]

namespace WagsReader.iOS
{
    public class IOSDeviceInfo : IDeviceInfo
    {
        public IOSDeviceInfo()
        {

        }

        public string GetInoreaderOAuthRedirectUrl()
        {
            return "com.kpwags.wagsreader://oauth2redirect/";
        }

        public string GetDeviceID()
        {
            return UIKit.UIDevice.CurrentDevice.IdentifierForVendor.AsString();
        }
    }
}
