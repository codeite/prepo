using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prepo.Api.Infrastructure
{
    public static class UriBuilderHelper
    {
        public static string Combine(string first, params string[] parts)
        {
            if (parts.Length == 0)
            {
                return first;
            }

            first = first.TrimEnd('/');
            var last = parts.Last().TrimStart('/');
            var rest = parts.Take(parts.Length - 1).Select(p => p.Trim('/'));

            var all = new[] {first}.Concat(rest).Concat(new[] {last});

            return string.Join("/", all);
        }
    }
}