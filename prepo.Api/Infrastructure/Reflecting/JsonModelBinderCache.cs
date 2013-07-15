using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace prepo.Api.Infrastructure.Reflecting
{
    public delegate object JsonModelBinder(Dictionary<string, object> json, params object[] arguments);

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

            if (!_binders.TryGetValue(type, out binder))
            {
                binder = CreateBinderFor(type);
                _binders[type] = binder;
            }

            return binder;
        }

        public JsonModelBinder CreateBinderFor(Type type)
        {
            Expression uncompiled;
            return CreateBinderFor(type, out uncompiled);
        }

        public JsonModelBinder CreateBinderFor(Type type, out Expression uncompiled)
        {
            var ctor = type.GetConstructors().SingleOrDefault();
            if (ctor == null)
            {
                throw new ArgumentNullException("ctor", "Type does not have blank blank constructor");
            }

            // Creating a parameter expression.
            ParameterExpression jsonValue = Expression.Parameter(typeof(Dictionary<string, object>), "json");

            var instance = Expression.Parameter(type, "instance");
            var arguments = Expression.Parameter(typeof(object[]), "arguments");

            var expandedArguments = ctor.GetParameters().Select((p,i) => Expression.Parameter(p.ParameterType, "argument_"+i)).ToList();

            var newArgs = expandedArguments.Select((pe, i) =>
                Expression.Convert(Expression.ArrayIndex(arguments, Expression.Constant(i)), pe.Type) as Expression
                ).ToList();

            var bodyExpressions = new List<Expression>
            {
                Expression.Assign(instance, Expression.New(ctor, newArgs))
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
                new []{instance}.Concat(expandedArguments),
                bodyExpressions
            );

            var lambda = Expression.Lambda<JsonModelBinder>(block, jsonValue, arguments);
            uncompiled = lambda;

            return lambda.Compile();
        }

        private Expression ReadJsonAsCorrectType(MethodCallExpression readJson, Type type)
        {
            if (type.IsArray)
            {
                return ReadIntoArray(readJson, type);
            }

            var method = ReadJsonAsPrimative(type);
            if (method != null)
            {
                return Expression.Call(method, readJson);
            }

            Expression uncompiled;
            CreateBinderFor(type, out uncompiled);

            var jsonObject = Expression.Convert(readJson, typeof (Dictionary<string, object>));
            var nullArgs = Expression.Constant(new object[0]);


            var subCall = Expression.Convert(Expression.Invoke(uncompiled, jsonObject, nullArgs), type);

            return subCall;
        }

        private BlockExpression ReadIntoArray(MethodCallExpression readJson, Type type)
        {
            var itemType = type.GetElementType();

            var enumerableObjectsType = typeof (IEnumerable<object>);

            ParameterExpression jsonList = Expression.Variable(enumerableObjectsType, "jsonList");// IEnumerable<object> jsonList;
            ParameterExpression arrayLength = Expression.Variable(typeof (int), "arrayVar"); // int arrayVar;
            ParameterExpression count = Expression.Variable(typeof (int), "count"); // int count;
            ParameterExpression arrayVar = Expression.Variable(type, "arrayVar"); // T arrayVar;
            ParameterExpression enumerator = Expression.Variable(typeof (IEnumerator<object>), "enumerator");// IEnumerator<object> enumerator;

            MethodInfo countMethod = MemberAccessHelper.GetStaticMethod(() => Enumerable.Count<object>(null));
            MethodInfo enumeratorMethod = MemberAccessHelper.GetMethod<IEnumerable<object>>(x => x.GetEnumerator());
            MethodInfo currentMethod = MemberAccessHelper.GetProperty<IEnumerator<object>>(x => x.Current).GetGetMethod();
            MethodInfo moveNextMethod = MemberAccessHelper.GetMethod<IEnumerator<object>>(x => x.MoveNext());

            var readJsonAsArray = Expression.Assign(jsonList, Expression.Convert(readJson, enumerableObjectsType));
            var readArrayLength = Expression.Assign(arrayLength, Expression.Call(countMethod, jsonList));
            var createArray = Expression.Assign(arrayVar, Expression.New(type.GetConstructor(new[] {typeof (int)}), arrayLength));
            var getEnumerator = Expression.Assign(enumerator, Expression.Call(jsonList, enumeratorMethod));

            var endOfLoopLabel = Expression.Label("endOfLoop");

            //var readValue = Expression.Convert(Expression.Call(enumerator, currentMethod), itemType);
            var readValue = ReadJsonAsCorrectType(Expression.Call(enumerator, currentMethod), itemType);

            var loopBody = Expression.Block(
                Expression.Call(enumerator, moveNextMethod),
                Expression.Assign(Expression.ArrayAccess(arrayVar, count),
                                  readValue),
                Expression.Assign(count, Expression.Increment(count))
                );

            var loop = Expression.Loop(
                Expression.IfThenElse(Expression.LessThan(count, arrayLength),
                                      loopBody,
                                      Expression.Break(endOfLoopLabel)
                    ), endOfLoopLabel);

            var methodBlock = Expression.Block(type,
                                               new[] {jsonList, arrayLength, arrayVar, count, enumerator},
                                               readJsonAsArray,
                                               readArrayLength,
                                               createArray,
                                               getEnumerator,
                                               loop,
                                               arrayVar
                );
            return methodBlock;
        }

        private static MethodInfo ReadJsonAsPrimative(Type type)
        {
            var name = "To" + type.Name;

            var method = typeof (Convert).GetMethod(name, BindingFlags.Public | BindingFlags.Static, null,
                                                    CallingConventions.Any, new[] {typeof (object)}, null);
            return method;
        }
    }
}