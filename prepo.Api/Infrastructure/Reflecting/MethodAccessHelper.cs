using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace prepo.Api.Infrastructure.Reflecting
{
    public static class MemberAccessHelper
    {
        public static PropertyInfo GetIndexerProperty<T>()
        {
            var defaultMember = typeof (T).GetDefaultMembers().FirstOrDefault();

            if (defaultMember == null)
            {
                throw new MemberAccessHelperException(string.Format("Type {0} does not have an indexer", typeof(T)));
            }

            if (defaultMember is PropertyInfo)
            {
                return defaultMember as PropertyInfo;
            }
            else
            {
                throw new MemberAccessHelperException("Default property was not an indexer");
            }
        }

        public static MethodInfo GetMethod<T>(Expression<Func<T, object>> expression)
        {
            return ReadMethodInfoFromExpression(expression.Body);
        }

        public static PropertyInfo GetProperty<T>(Expression<Func<T, object>> propertyExpression)
        {
            return ReadPropertyInfoFromExpression(propertyExpression.Body);
        }

        public static PropertyInfo GetProperty<T, TType>(Expression<Func<T, TType>> propertyExpression)
        {
            return ReadPropertyInfoFromExpression(propertyExpression.Body);
        }

        public static MethodInfo GetStaticMethod(Expression<Func<object>> expression)
        {
            return ReadMethodInfoFromExpression(expression.Body);
        }

        private static PropertyInfo ReadPropertyInfoFromExpression(Expression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            var memberExpression = expression as MemberExpression;
            if (memberExpression != null)
            {
                var member = memberExpression.Member;

                if (member is PropertyInfo)
                {
                    return member as PropertyInfo;
                }

                throw new MemberAccessHelperException("Expecting PropertyInfo but got: " + member.GetType());
            }

            var unaryExpression = expression as UnaryExpression;
            if (unaryExpression != null)
            {
                return ReadPropertyInfoFromExpression(unaryExpression.Operand);
            }

            throw new MemberAccessHelperException("I know know what to do with a: " + expression.GetType());
        }

        private static MethodInfo ReadMethodInfoFromExpression(Expression expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");

            var methodCallExpression = expression as MethodCallExpression;
            if (methodCallExpression != null)
            {
                return methodCallExpression.Method;
            }
 
            var unaryExpression = expression as UnaryExpression;
            if (unaryExpression != null)
            {
                return ReadMethodInfoFromExpression(unaryExpression.Operand);
            }

            throw new MemberAccessHelperException("I know know what to do with a: " + expression.GetType());
        }

        public static PropertyInfo GetProperty<T>(this T target, Expression<Func<T, object>> expression)
        {
            return GetProperty(expression);
        }

        public static object GetHiddenMemberValue(object target, string name)
        {
            var type = target.GetType();

            var members = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
            var matchingMembers = members.Where(x => x.Name == name);

            var member = matchingMembers.FirstOrDefault();

            if (member == null)
            {
                throw new MemberAccessHelperException("Could not find " + name + " on " + target.GetType());
            }

            if (member is PropertyInfo)
            {
                var property = member as PropertyInfo;
                return property.GetValue(target);
            }

            throw new MemberAccessHelperException("How do I read a " + member.GetType());
        }

        public static string GetDebugExpression(Expression expression)
        {

            var types = typeof (Expression).GetNestedTypes(BindingFlags.NonPublic);

            var name = expression.GetType().Name + "Proxy";

            if (expression is LambdaExpression)
            {
                name = "LambdaExpressionProxy";
            }

            var type = types.FirstOrDefault(x => x.Name == name);

            if (type == null)
            {
                throw new MemberAccessHelperException("Can't find proxy: " + name);
            }

            var instance = Activator.CreateInstance(type, expression);

            var property = type.GetProperty("DebugView");
            return property.GetValue(instance).ToString();
        }
    }
}