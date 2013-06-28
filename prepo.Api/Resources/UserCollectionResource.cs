using prepo.Api.Contracts.Models;
using prepo.Api.Infrastructure;
using prepo.Api.Resources.Base;
using prepo.Api.Services;

namespace prepo.Api.Resources
{
    public class UserCollectionResource : HalPagedCollectionResource<PrepoUser>
    {
        public const string CollectionName = "users";
        /*
        public UserCollectionResource(string location)
            : base(UriBuilderHelper.Combine(location, "users"), "user", "users")
        {
        }
        */

        public UserCollectionResource(IHalResource owner)
            : base(owner, CollectionName, UserItemResource.ItemName)
        {
        }

        protected override IItemResourceFactory<PrepoUser> ChildFactory
        {
            get { return new ItemResourceFactory<UserItemResource, PrepoUser>(); }
        }

        
    }
}