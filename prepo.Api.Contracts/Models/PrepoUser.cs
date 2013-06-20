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
            Name = "Number "+id;
        }

        public string Name { get; set; }
    }

    public class DbObject
    {
        public string Id { get; set; }

    }
}