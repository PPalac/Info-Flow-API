using System.Threading.Tasks;
using InfoFlow.Core.Enums;
using InfoFlow.API.ViewModels;

namespace InfoFlow.API.Services.Interfaces
{
    public interface IAccountService
    {
        string BuildToken(UserViewModel user);
        Task<UserViewModel> AuthenticateAsync(LoginViewModel login);
        Task<string> RegisterStudentAsync(RegisterUserViewModel user);
        Task<string> AddToRole(string username, Role role);
        Task<string> GenerateRegisterToken();
        bool CheckRegisterToken(string param);
    }
}
