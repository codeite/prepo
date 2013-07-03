using System;
using prepo.Api.Contracts.Models;
using prepo.Api.Resources;
using prepo.Api.Services;

namespace prepo.Api.Controllers
{
    public class UsersController : DefaultController<UserCollectionResource, UserItemResource, PrepoUser>
    {
        public UsersController(UserResourceRepository repository) 
            : base(repository)
        {
        }
    }
}