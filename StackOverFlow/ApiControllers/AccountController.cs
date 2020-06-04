using StackOverFlow.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StackOverFlow.ApiControllers
{
    public class AccountController : ApiController
    {
        IUsersService usersService;
        public AccountController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public string Get(string Email)
        {
            if (this.usersService.GetUsersByEmail(Email) != null)
            {
                return "Found";
            }
            else
            {
                return "Not Found";
            }
            
        }
    }
}
