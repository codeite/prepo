using System.Collections.Generic;
using prepo.Api.Services;

namespace prepo.Api.Controllers.Test
{
    /*
    // Moved
    public interface IHalResource2
    {
        void ReadChildResources(Stack<string> partsStack);
        IHalResource2 Child { get; }
        IHalResource2 Head { get; }
        IHalResource2 Owner { get; }
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
    */
}
