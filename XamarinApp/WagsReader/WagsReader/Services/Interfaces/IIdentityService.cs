using System.Threading.Tasks;
using WagsReader.Models;

namespace WagsReader.Services.Interfaces
{
    public interface IIdentityService
    {
        string CreateAuthorizationRequest();
        Task<UserToken> GetTokenAsync(string code);
        Task<string> GetAsync(string uri, string accessToken);
    }
}
