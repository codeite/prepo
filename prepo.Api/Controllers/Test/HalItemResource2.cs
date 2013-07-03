using System;
using System.Collections.Generic;
using System.Linq;

namespace prepo.Api.Controllers.Test
{
    /*
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
    */
}