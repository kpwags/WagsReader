using SQLite;
using System;
using System.IO;
using System.Threading.Tasks;
using WagsReader.Interfaces;
using WagsReader.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(IOSDatabaseConnection))]

namespace WagsReader.iOS
{
    public class IOSDatabaseConnection : IDatabaseConnection
    {
        private string _databasePath;

        public SQLiteConnection DBConnection()
        {
            string personalDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libraryDirectory = Path.Combine(personalDirectory, "..", "Library");
            string DBName = Constants.DatabaseName;

            _databasePath = Path.Combine(libraryDirectory, DBName);

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
                dbContent = await File.ReadAllBytesAsync(_databasePath);
            }

            return dbContent;
        }
    }
}