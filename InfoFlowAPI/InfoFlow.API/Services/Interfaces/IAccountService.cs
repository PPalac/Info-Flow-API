using System.Threading.Tasks;
using InfoFlow.Core.Enums;
using InfoFlow.API.ViewModels;

namespace InfoFlow.API.Services.Interfaces
{
    public interface IAccountService
    {
        string BuildToken(UserViewModel user);
        Task<UserViewModel> AuthenticateAsync(LoginViewModel login);
        Task<bool> RegisterStudentAsync(RegisterUserViewModel user);
        Task<bool> AddToRole(string userName, Role role);
    }
}
