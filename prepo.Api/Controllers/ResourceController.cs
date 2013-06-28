using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using prepo.Api.Infrastructure;
using prepo.Api.Resources;
using prepo.Api.Resources.Base;

namespace prepo.Api.Controllers
{
    public class ResourceController : ApiController
    {
        private readonly ResourceChainBuilder _resourceChainBuilder;

        public ResourceController(ResourceChainBuilder resourceChainBuilder)
        {
            _resourceChainBuilder = resourceChainBuilder;
        }

        public virtual HttpResponseMessage Get(string path)
        {
            var rootResource = _resourceChainBuilder.Build(path);

            var resource = rootResource.Head;

            var response = Request.CreateResponse(resource != null ? HttpStatusCode.OK : HttpStatusCode.NotFound,
                                                  resource);

            return response;
        }
    }

    public class ResourceChainBuilder
    {
        public IHalResource2 Build(string path)
        {
            var rootResource = new RootResource2();

            var partsStack = new Stack<string>(path.Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries).Reverse());

            rootResource.ReadChildResources(partsStack);

            return rootResource;
        }
    }

    public interface IHalResource2
    {
        void ReadChildResources(Stack<string> partsStack);
        IHalResource2 Child { get; }
        IHalResource2 Head { get; }
        IHalResource2 Owner { get; }
    }

    public abstract class HalItemResource2 : IHalResource2
    {
        protected HalItemResource2(string id, IHalResource2 owner)
        {
            Id = id;
            Owner = owner;
        }

        protected abstract Dictionary<string, IHalResource2> ChildResources { get; }

        public string Id { get; set; }
        public IHalResource2 Owner { get; private set; }

        public IHalResource2 Child { get; private set; }

        public IHalResource2 Head { get { return Child == null ? this : Child.Head; } }

        public void ReadChildResources(Stack<string> partsStack)
        {
            if (partsStack.Any())
            {
                var directChild = partsStack.Pop().ToLowerInvariant();

                if (!ChildResources.ContainsKey(directChild))
                {
                    throw new Exception("Unknown child resource: " + directChild);
                }

                Child = ChildResources[directChild];

                Child.ReadChildResources(partsStack);
            }
        }
    }

    public abstract class HalCollectionResource2 : IHalResource2
    {
        protected HalCollectionResource2(IHalResource2 owner)
        {
            Owner = owner;
        }

        public IHalResource2 Owner { get; private set; }
        public IHalResource2 Child { get; private set; }
        public IHalResource2 Head { get { return Child == null ? this : Child.Head; } }

        protected abstract IItemResourceFactory ChildFactory { get; }

        public void ReadChildResources(Stack<string> partsStack)
        {
            if (partsStack.Any())
            {
                var id = partsStack.Pop();

                Child = ChildFactory.BuildResource(id, this);

                Child.ReadChildResources(partsStack);
            }
        }
    }

    public interface IItemResourceFactory
    {
        HalItemResource2 BuildResource(string id, IHalResource2 owner);
    }

    public class ItemResourceFactory<T> : IItemResourceFactory
        where T : HalItemResource2
    {
        public HalItemResource2 BuildResource(string id, IHalResource2 owner)
        {
            return GenericActivator.CreateInstance<T>(id, owner);
        }
    }

    public class RootResource2 : HalItemResource2
    {
        public RootResource2()
            : base("", null)
        {
        }


        protected override Dictionary<string, IHalResource2> ChildResources
        {
            get
            {
                return new Dictionary<string, IHalResource2>
                {
                    {"users", new UserCollectionResource2(this)}
                };
            }
        }
    }

    public class UserCollectionResource2 : HalCollectionResource2
    {
        public UserCollectionResource2(IHalResource2 owner)
            : base(owner)
        {
        }

        protected override IItemResourceFactory ChildFactory
        {
            get { return new ItemResourceFactory<UserItemResource2>(); }
        }
    }

    public class UserItemResource2 : HalItemResource2
    {
        public UserItemResource2(string id, IHalResource2 owner)
            : base(id, owner)
        {
        }

        protected override Dictionary<string, IHalResource2> ChildResources
        {
            get
            {
                return new Dictionary<string, IHalResource2>
                {
                    {"personas", new PersonaCollectionResource2(this)}
                };
            }
        }
    }

    public class PersonaCollectionResource2 : HalCollectionResource2
    {
        public PersonaCollectionResource2(IHalResource2 owner)
            : base(owner)
        {
        }

        protected override IItemResourceFactory ChildFactory
        {
            get { return new ItemResourceFactory<PersonaItemResource2>(); }
        }
    }

    public class PersonaItemResource2 : HalItemResource2
    {
        public PersonaItemResource2(string id, IHalResource2 owner)
            : base(id, owner)
        {
        }
        protected override Dictionary<string, IHalResource2> ChildResources
        {
            get
            {
                return new Dictionary<string, IHalResource2>
                {
                };
            }
        }
        
    }
}
