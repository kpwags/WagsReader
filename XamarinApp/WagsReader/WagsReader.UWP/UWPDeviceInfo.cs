using System;
using WagsReader.Interfaces;
using WagsReader.UWP;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.System.Profile;
using static System.Diagnostics.Debug;

[assembly: Xamarin.Forms.Dependency(typeof(UWPDeviceInfo))]

namespace WagsReader.UWP
{
    public class UWPDeviceInfo : IDeviceInfo
    {
        public string GetInoreaderOAuthRedirectUrl()
        {
            return "com.kpwags.wagsreader://oauth2redirect/";
        }

        public string GetDeviceID()
        {
            return UWPDeviceID();
        }

        private static string UWPDeviceID()
        {
            try
            {
                var token = HardwareIdentification.GetPackageSpecificToken(null);
                var hardwareId = token.Id;

                byte[] bytes = new byte[hardwareId.Length];

                using (var dr = DataReader.FromBuffer(hardwareId))
                {
                    dr.ReadBytes(bytes);
                }

                var alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
                var hashed = alg.HashData(CryptographicBuffer.CreateFromByteArray(bytes));

                return CryptographicBuffer.EncodeToHexString(hashed);
            }
            catch (Exception ex)
            {
                WriteLine($"Error generating device ID: {ex.Message}");
                return "";
            }
        }
    }
}
