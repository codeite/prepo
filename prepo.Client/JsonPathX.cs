using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace prepo.Client
{
    public static class JsonPathX
    {
        public static string ReadValue(this Dictionary<string, object> json, string path)
        {
            throw new NotImplementedException();
        }

        public static string Replace(this string target, Regex regex, string replace)
        {
            return regex.Replace(target, replace);
        }

        public static string Replace(this string target, Regex regex, MatchEvaluator matchEvaluator)
        {
            return regex.Replace(target, matchEvaluator);
        }

        private static string Normalize(string expr)
        {
            var subx = new List<string>();

            var regex1 = new Regex(@"[\['](\??\(.*?\))[\]']");

            return expr.Replace(regex1, m =>
            {
                subx.Add(m.Groups[1].Value);
                return "[#" + (subx.Count - 1) + "]";
            })
            .Replace(new Regex(@"'?\.'?|\['?"), ";")
            .Replace(new Regex(@";;;|;;"), ";..;")
            .Replace(new Regex(@";$|'?\]|'$"), "")
            .Replace(new Regex(@"#([0-9]+)"), m => subx[Convert.ToInt32(m.Groups[1].Value)]);
        }

        public static void jsonPath(this Dictionary<string, object> obj, string expr, string arg = null)
        {
            resultType = arg ?? "VALUE";

            var normal = Normalize(expr);
        }

        public static string asPath(string path)
        {
            var x = path.Split(';');
            var p = "$";
            for (var i = 1; i < x.Length; i++)
            {
                p += new Regex("^[0-9*]+$").IsMatch(x[i])
                         ? ("[" + x[i] + "]")
                         : ("['" + x[i] + "']");
            }
            return p;
        }

        private static List<string> result = new List<string>();
        private static string resultType;

        public static bool store(string p, string v)
        {
            result[result.Count] = resultType == "PATH" ? asPath(p) : v;
            return p != null;
        }

        public static T Pop<T>(this List<T> list)
        {
            var t = list[0];
            list.RemoveAt(0);
            return t;
        }

        public static void trace(string expr, dynamic val, dynamic path)
        {
            /*
         if (expr != null)
         {
             
             var x = expr.Split(';').ToList();
             var loc = x.Pop();
             var xs = string.Join(';', x);
            if (val && val.hasOwnProperty(loc))
               trace(xs, val[loc], path + ";" + loc);
            else if (loc === "*")
               walk(loc, x, val, path, function(m,l,x,v,p) { P.trace(m+";"+x,v,p); });
            else if (loc === "..") {
               trace(x, val, path);
               walk(loc, x, val, path, function(m,l,x,v,p) { typeof v[m] === "object" && P.trace("..;"+x,v[m],p+";"+m); });
            }
            else if (/,/.test(loc)) { // [name1,name2,...]
               for (var s=loc.split(/'?,'?/),i=0,n=s.length; i<n; i++)
                  P.trace(s[i]+";"+x, val, path);
            }
            else if (/^\(.*?\)$/.test(loc)) // [(expr)]
               P.trace(P.eval(loc, val, path.substr(path.lastIndexOf(";")+1))+";"+x, val, path);
            else if (/^\?\(.*?\)$/.test(loc)) // [?(expr)]
               P.walk(loc, x, val, path, function(m,l,x,v,p) { if (P.eval(l.replace(/^\?\((.*?)\)$/,"$1"),v[m],m)) P.trace(m+";"+x,v,p); });
            else if (/^(-?[0-9]*):(-?[0-9]*):?([0-9]*)$/.test(loc)) // [start:end:step]  phyton slice syntax
               P.slice(loc, x, val, path);
         }
         else
            store(path, val);
         */
        }
        public static void walk(dynamic loc, dynamic expr, dynamic val, dynamic path, dynamic f)
        {
            /*
           if (val is Array) {
              for (var i=0,n=val.length; i<n; i++)
                 if (i in val)
                    f(i,loc,expr,val,path);
           }
           else if (typeof val === "object") {
              for (var m in val)
                 if (val.hasOwnProperty(m))
                    f(m,loc,expr,val,path);
           }
             * */
        }

        public static void slice(dynamic loc, dynamic expr, dynamic val, dynamic path)
        {
            /*
           if (val instanceof Array) {
              var len=val.length, start=0, end=len, step=1;
              loc.replace(/^(-?[0-9]*):(-?[0-9]*):?(-?[0-9]*)$/g, function($0,$1,$2,$3){start=parseInt($1||start);end=parseInt($2||end);step=parseInt($3||step);});
              start = (start < 0) ? Math.max(0,start+len) : Math.min(len,start);
              end   = (end < 0)   ? Math.max(0,end+len)   : Math.min(len,end);
              for (var i=start; i<end; i+=step)
                 P.trace(i+";"+expr, val, path);
           }
           */
        }

        public static void eval(dynamic x, dynamic _v, dynamic _vname)
        {
            /*
             * try { return $ && _v && eval(x.replace(/@/g, "_v")); }
          catch(e) { throw new SyntaxError("jsonPath: " + e.message + ": " + x.replace(/@/g, "_v").replace(/\^/g, "_a")); }
             */
        }

        /*
           var $ = obj;
           if (expr && obj && (P.resultType == "VALUE" || P.resultType == "PATH")) {
              P.trace(P.normalize(expr).replace(/^\$;/,""), obj, "$");
              return P.result.length ? P.result : false;
           }
        */
    }
}