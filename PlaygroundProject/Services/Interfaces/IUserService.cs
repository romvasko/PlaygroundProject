using PlaygroundProject.ServicesResponse;
using PlaygroundProject.ViewModels;

namespace PlaygroundProject.Services.Interfaces
{
    public interface IUserService
    {
        public Task<GetTokenResponse> GetToken(Roles role);
        public Task<GetUserInfoResponse> GetUserInfo(string token);
    }
}
