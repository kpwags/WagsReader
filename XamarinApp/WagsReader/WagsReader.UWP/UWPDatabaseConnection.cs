using SQLite;
using System.IO;
using System.Threading.Tasks;
using WagsReader.Interfaces;
using WagsReader.UWP;
using Windows.Storage;

[assembly: Xamarin.Forms.Dependency(typeof(UWPDatabaseConnection))]

namespace WagsReader.UWP
{
    public class UWPDatabaseConnection : IDatabaseConnection
    {
        private string _databasePath;

        public SQLiteConnection DBConnection()
        {
            string localDirectory = Path.Combine(ApplicationData.Current.LocalFolder.Path);
            string DBName = Constants.DatabaseName;

            _databasePath = Path.Combine(localDirectory, DBName);

            var connectionString = new SQLiteConnectionString(_databasePath, false);
            return new SQLiteConnection(connectionString);
        }

        public string ExportDB()
        {
            return "ok";
        }

        public string GetPath()
        {
            return _databasePath;
        }

        public async Task<object> GetDBContent()
        {
            object dbContent = null;

            if (File.Exists(_databasePath))
            {
                string localDirectory = Path.Combine(ApplicationData.Current.LocalFolder.Path);
                string destinationFile = Path.Combine(localDirectory, "WagsReaderCopy.db");

                if (File.Exists(destinationFile))
                {
                    File.Delete(destinationFile);
                }

                File.Copy(_databasePath, destinationFile, true);
                dbContent = await File.ReadAllBytesAsync(destinationFile);

                if (File.Exists(destinationFile))
                {
                    File.Delete(destinationFile);
                }
            }

            return dbContent;
        }
    }
}
