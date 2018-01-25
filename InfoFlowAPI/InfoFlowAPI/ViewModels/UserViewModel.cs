using System.Collections.Generic;

namespace InfoFlow.API.ViewModels
{
    public class UserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
    }
}
