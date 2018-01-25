using System.Linq;
using InfoFlow.Core.Enums;
using Microsoft.AspNetCore.Authorization;

namespace InfoFlow.API.Helpers
{
    public class Auth : AuthorizeAttribute
    {
        public Auth(params Role[] roles)
        {
            Roles = string.Join(',', roles.Select(r => r.ToString()));
        } 
    }
}
