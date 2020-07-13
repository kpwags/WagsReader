using System.Threading.Tasks;

namespace WagsReader.Services.Interfaces
{
    public interface IRSSService
    {
        Task<Models.User> Login();
    }
}
