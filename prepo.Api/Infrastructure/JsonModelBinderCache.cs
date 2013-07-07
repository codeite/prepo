using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace prepo.Api.Infrastructure
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

            var indexer = typeof (Dictionary<string, object>).GetProperty("Item");

            foreach (var propertyInfo in type.GetProperties())
            {
                var readJson = Expression.Call(jsonValue, indexer.GetGetMethod(), Expression.Constant(propertyInfo.Name.ToLowerInvariant()));

                var readJsonAsString = Expression.Convert(readJson, typeof (string));

                bodyExpressions.Add(
                    Expression.Call(
                        instance,
                        propertyInfo.GetSetMethod(),
                        readJsonAsString));
            }
            
            bodyExpressions.Add(instance);

            BlockExpression block = Expression.Block(
                new []
                {
                   instance
                },
                bodyExpressions
            );

            

            return Expression.Lambda<JsonModelBinder>(block, jsonValue).Compile();

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
    }
}