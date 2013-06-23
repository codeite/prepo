using prepo.Api.Contracts.Models;
using prepo.Api.Resources;
using prepo.Api.Services;

namespace prepo.Api.Controllers
{
    public abstract class DefaultController<TCollection, TItem, TDbo>
        : ResourceApiController<TCollection, TItem, TDbo>
        where TCollection : HalCollectionResource<TDbo>
        where TItem : HalItemResource<TDbo>
        where TDbo : DbObject
    {
        private readonly ResourceRepository<TCollection, TItem, TDbo> _repository;

        public DefaultController(ResourceRepository<TCollection, TItem, TDbo> repository)
        {
            _repository = repository;
        }

        protected override TCollection GetResourceCollection(int? page, int count = 10)
        {
            return _repository.GetCollectionResource(page, count);
        }

        protected override TItem GetResource(string id)
        {
            return _repository.GetById(id);
        }

        protected override SaveResourceResult<TItem> SaveResource(string id, TItem content)
        {
            if (content == null)
            {
                _repository.Delete(id);
                return new SaveResourceResult<TItem>(SaveResourceResult <TItem>.ActionPerfomedOptions.Deleted);
            }
            else
            {
                var updated = _repository.SaveItem(ref id, content);
                return new SaveResourceResult<TItem>(updated ? SaveResourceResult<TItem>.ActionPerfomedOptions.Updated : SaveResourceResult<TItem>.ActionPerfomedOptions.Created)
                {
                    Location = content.SelfLink.Href,
                    Resource = _repository.GetById(id),
                };
            }
        }
    }
}