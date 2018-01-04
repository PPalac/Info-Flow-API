using System.Threading.Tasks;
using InfoFlowAPI.ViewModels;

namespace InfoFlowAPI.Services.Interfaces
{
    public interface IAccountService
    {
        string BuildToken(UserViewModel user);
        Task<UserViewModel> AuthenticateAsync(LoginViewModel login);
        Task<bool> RegisterUserAsync(RegisterUserViewModel user);
    }
}
