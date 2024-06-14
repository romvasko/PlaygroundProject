using PlaygroundProject.ServicesResponce;
using PlaygroundProject.ViewModels;

namespace PlaygroundProject.Services.Interfaces
{
    public interface IUserService
    {
        public Task<GetTokenResponce> GetToken(Roles role);
    }
}
