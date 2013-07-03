using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prepo.Api.Contracts.Models
{
    public class PrepoUser : DbObject
    {
        public PrepoUser(string id)
        {
            Id = id;
        }

        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class PrepoPersona : DbObject
    {
        public PrepoPersona(string id)
        {
            Id = id;
        }
    }

    public class PrepoRoot : DbObject
    {
        public PrepoRoot()
        {
            Id = string.Empty;
        }
    }
}