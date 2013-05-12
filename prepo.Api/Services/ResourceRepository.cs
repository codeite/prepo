using prepo.Api.Resources;

namespace prepo.Api.Services
{
    public class ResourceRepository
    {
        public RootResource GetRootResource()
        {
            return new RootResource();
        }
    }
}