using System.Collections.Generic;
using System.Linq;
using prepo.Api.Services;

namespace prepo.Api.Controllers.Test
{
    /*
    public abstract class HalCollectionResource2 : IHalResource2
    {
        protected HalCollectionResource2(IHalResource2 owner)
        {
            Owner = owner;
        }

        public IHalResource2 Owner { get; private set; }
        public IHalResource2 Child { get; private set; }
        public IHalResource2 Head { get { return Child == null ? this : Child.Head; } }

        protected abstract IItemResourceFactory<TDbo> ChildFactory { get; }

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
    */
}