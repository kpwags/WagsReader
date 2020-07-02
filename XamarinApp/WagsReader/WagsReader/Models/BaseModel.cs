using SQLite;
using System;
using System.Threading.Tasks;
using WagsReader.Interfaces;
using Xamarin.Forms;

namespace WagsReader.Models
{
    public class BaseModel
    {
        protected static SQLiteConnection _db;

        public BaseModel()
        {
            if (_db == null)
            {
                _db = DependencyService.Get<IDatabaseConnection>().DBConnection();
                _db.Execute("PRAGMA foreign_keys = ON");
            }
        }

        public static bool TableExists(string tableName)
        {
            string query = $"SELECT * FROM sqlite_master WHERE type = 'table' AND tbl_name = ?";
            var result = _db.ExecuteScalar<string>(query, tableName);
            
            if (result != null)
            {
                return true;
            }

            return false;
        }

        public static int DropTable(string tableName)
        {
            if (!TableExists(tableName))
            {
                return 1;
            }

            string query = $"DROP TABLE IF EXISTS [{tableName}]";
            return _db.Execute(query);
        }
    }
}
