using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prepo.Api.Contracts.Models
{
    public class PrepoUser : DbObject
    {
    }

    public class DbObject
    {
        public string Id { get; set; }

    }
}