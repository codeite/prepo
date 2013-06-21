using prepo.Api.Contracts.Models;

namespace prepo.Api.Resources
{
    public class UserResource : HalItemResource<PrepoUser>
    {
        public const string Self = RootResource.Self + "users/";

        public UserResource(PrepoUser user)
            : base(Self + user.Id, user)
        { }
    }
}