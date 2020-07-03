using System.Threading.Tasks;
using WagsReader.Classes;
using WagsReader.Models;

namespace WagsReader.Services.Interfaces
{
    public interface IIdentityService
    {
        AuthRequest CreateAuthorizationRequest();

    }
}
