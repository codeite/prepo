using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prepo.Api.Infrastructure
{
    public class GenericActivator
    {
        public static T CreateInstance<T>(params object[] args) where T : class
        {
            return Activator.CreateInstance(typeof(T), args) as T;
        }
    }
}