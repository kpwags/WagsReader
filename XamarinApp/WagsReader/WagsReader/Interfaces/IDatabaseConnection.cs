using SQLite;
using System.Threading.Tasks;

namespace WagsReader.Interfaces
{
    public interface IDatabaseConnection
    {
        SQLiteConnection DBConnection();
        string ExportDB();
        string GetPath();
        Task<object> GetDBContent();
    }
}
