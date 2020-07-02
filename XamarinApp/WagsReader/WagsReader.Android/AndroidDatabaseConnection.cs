using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using WagsReader.Droid;
using System.Threading.Tasks;
using SQLite;
using System.IO;
using static System.Diagnostics.Debug;
using Xamarin.Forms;
using WagsReader.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidDatabaseConnection))]

namespace WagsReader.Droid
{
    public class AndroidDatabaseConnection
    {
        private string _databasePath;

        public SQLiteConnection DBConnection()
        {
            string DBName = Constants.DatabaseName;

            _databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), DBName);

            var connectionString = new SQLiteConnectionString(_databasePath, false);
            return new SQLiteConnection(connectionString);
        }

        public string ExportDB()
        {
            string fileLocation = "";

            try
            {
                string deviceId = DependencyService.Get<IDeviceInfo>().GetDeviceID();
                string DBName = Constants.DatabaseName;

                _databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), DBName);

                var bytes = File.ReadAllBytes(_databasePath);
                fileLocation = string.Format("/sdcard/Database{0:dd-MM-yyyy_HH-mm-ss-tt}.sqlite", DateTime.Now);

                File.WriteAllBytes(fileLocation, bytes);
            }
            catch (Exception ex)
            {
                WriteLine($"Error Exporting SQLite DB: {ex}");
            }

            return fileLocation;
        }

        public string GetPath()
        {
            return _databasePath;
        }

        public async Task<object> GetDBContent()
        {
            object dbContent = null;
            var fileLocation = ExportDB();

            if (!string.IsNullOrEmpty(fileLocation))
            {
                if (File.Exists(fileLocation))
                {
                    dbContent = await File.ReadAllBytesAsync(fileLocation);
                }
            }

            return dbContent;
        }
    }
}