using WagsReader.Droid;
using WagsReader.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidDeviceInfo))]

namespace WagsReader.Droid
{
    public class AndroidDeviceInfo : IDeviceInfo
    {
        public string GetInoreaderOAuthRedirectUrl()
        {
            return "com.kpwags.wagsreader://oauth2redirect/";
        }

        public string GetDeviceID()
        {
            return Android.OS.Build.GetSerial();
        }
    }
}
