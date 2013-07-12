using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace prepo.Api.Infrastructure.Reflecting
{
    public delegate object JsonModelBinder(Dictionary<string, object> json);

    public class JsonModelBinderCache
    {
        private readonly Dictionary<Type, JsonModelBinder> _binders = new Dictionary<Type, JsonModelBinder>();

        public JsonModelBinder GetJsonModelBinder<T>()
        {
            return GetModelFinderFor(typeof (T));
        }

        public JsonModelBinder GetModelFinderFor(Type type)
        {
            JsonModelBinder binder;

            if (_binders.TryGetValue(type, out binder))
            {
                return binder;
            }
            else
            {
                binder = CreateBinderFor(type);
                _binders[type] = binder;
                return binder;
            }
        }

        class TestClassB
        {
            public string Abc { get; set; }
        }

        public JsonModelBinder CreateBinderFor(Type type)
        {
            Expression uncompiled;
            return CreateBinderFor(type, out uncompiled);
        }

        public JsonModelBinder CreateBinderFor(Type type, out Expression uncompiled)
        {
            var ctor = type.GetConstructor(new Type[0]);

            Expression<Func<TestClassB, object>> test = tcb =>
                                                  tcb.Abc;
         
            // Creating a parameter expression.
            ParameterExpression jsonValue = Expression.Parameter(typeof(Dictionary<string, object>), "json");

            var instance = Expression.Parameter(type, "instance");

            var bodyExpressions = new List<Expression>
            {
                Expression.Assign(
                    instance,
                    Expression.New(ctor)
                    )
            };

            //var indexer = typeof (Dictionary<string, object>).GetProperty("Item");
            var indexer = MemberAccessHelper.GetIndexerProperty<Dictionary<string, object>>();
            var tester = MemberAccessHelper.GetMethod<Dictionary<string, object>>(x => x.ContainsKey(null));

            foreach (var propertyInfo in type.GetProperties())
            {
                var readJson = Expression.Call(jsonValue, indexer.GetGetMethod(), Expression.Constant(propertyInfo.Name.ToLowerInvariant()));

                var readJsonAsCorrectType = ReadJsonAsCorrectType(readJson, propertyInfo.PropertyType);

                var testIfPropertyExists = Expression.Call(jsonValue, tester, Expression.Constant(propertyInfo.Name.ToLowerInvariant()));

                var assignJsonToProperty = Expression.Call(instance, propertyInfo.GetSetMethod(), readJsonAsCorrectType);

                bodyExpressions.Add(
                    Expression.IfThen(testIfPropertyExists, assignJsonToProperty)
                    );


            }
            
            bodyExpressions.Add(instance);

            BlockExpression block = Expression.Block(
                new []
                {
                   instance
                },
                bodyExpressions
            );

            var lambda = Expression.Lambda<JsonModelBinder>(block, jsonValue);
            uncompiled = lambda;

            return lambda.Compile();

            //int factorial = Expression.Lambda<Func<int, int>>(block, value).Compile()(5);

            return json =>
            {
                var instnace = Activator.CreateInstance(type);
                var properties = type.GetProperties();

                foreach (var propertyInfo in properties)
                {
                    propertyInfo.SetValue(instnace, json[propertyInfo.Name.ToLowerInvariant()]);
                }

                return instnace;
            };
        }

        private Expression ReadJsonAsCorrectType(MethodCallExpression readJson, Type type)
        {
            var name = "To" + type.Name;

            var method = typeof(Convert).GetMethod(name, BindingFlags.Public | BindingFlags.Static, null, CallingConventions.Any, new[] { typeof(object) }, null);

            if (method != null)
            {
                return Expression.Call(method, readJson);
            }

            Expression uncompiled;
            JsonModelBinder child = CreateBinderFor(type, out uncompiled);

            return uncompiled;

            throw new Exception(string.Format("Don't know how to convert type: {0} (Tried Convert.{1})", type, name));
        }
    }

    public static class JsonReadingHelper
    {
        public static string ReadString(object value)
        {
            return value.ToString();
        }

        public static int ReadInt32(object value)
        {
            if (value is int)
            {
                return (int) value;
            }
            else
            {
                return Convert.ToInt32(value);
            }
        }
    }
}