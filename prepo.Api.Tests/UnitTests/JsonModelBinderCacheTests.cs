using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Codeite.Core.Json;
using FluentAssertions;
using NUnit.Framework;
using prepo.Api.Infrastructure;
using prepo.Api.Infrastructure.Reflecting;

namespace prepo.Api.Tests.UnitTests
{
    [TestFixture]
    public class JsonModelBinderCacheTests
    {
        [Test]
        public void TestStringIntoString()
        {
            // Arrange
            var cache = new JsonModelBinderCache();
            var json = DynamicJsonObject.ReadJson(@"{'alpha':'first'}") as Dictionary<string, object>;

            // Act
            var binder = cache.CreateBinderFor(typeof (TestClassA));
            var instance = binder(json);

            // Assert
            instance.Should().BeOfType<TestClassA>();
            var typedInstance = instance as TestClassA;
            typedInstance.Alpha.Should().Be("first");
        }

        [Test, TestCaseSource("TestCaseTypes")]
        public void TestValueTypes(string jsonText, Func<TestClassB, object> accessor, object expectedValue )
        {
            Console.WriteLine("JSON: "+jsonText);
            // Arrange
            var cache = new JsonModelBinderCache();
            var json = DynamicJsonObject.ReadJson(jsonText) as Dictionary<string, object>;
            Expression uncompiled;

            // Act
            var binder = cache.CreateBinderFor(typeof(TestClassB), out uncompiled);


            Console.WriteLine(MemberAccessHelper.GetDebugExpression(uncompiled));

            object instance =binder(json);

            // Assert
            instance.Should().BeOfType<TestClassB>();
            var typedInstance = instance as TestClassB;
            accessor.Invoke(typedInstance).Should().Be(expectedValue);
        }

        [Test]
        public void TestCharValues()
        {
            // Arrange
            var cache = new JsonModelBinderCache();
            var json = DynamicJsonObject.ReadJson(@"{'mychar':'Q'}") as Dictionary<string, object>;
            Expression uncompiled;

            // Act
            var binder = cache.CreateBinderFor(typeof(TestClassC), out uncompiled);


            Console.WriteLine(MemberAccessHelper.GetDebugExpression(uncompiled));

            object instance = binder(json);

            // Assert
            instance.Should().BeOfType<TestClassC>();
            var typedInstance = instance as TestClassC;
            typedInstance.MyChar.Should().Be('Q');
        }

        [Test]
        public void TestBooleanValues()
        {
            // Arrange
            var cache = new JsonModelBinderCache();
            var json = DynamicJsonObject.ReadJson(@"{'mytrueboolean':true, 'myfalseboolean':false}") as Dictionary<string, object>;
            Expression uncompiled;

            // Act
            var binder = cache.CreateBinderFor(typeof(TestClassD), out uncompiled);


            Console.WriteLine(MemberAccessHelper.GetDebugExpression(uncompiled));

            object instance = binder(json);

            // Assert
            instance.Should().BeOfType<TestClassD>();
            var typedInstance = instance as TestClassD;
            typedInstance.MyTrueBoolean.Should().Be(true);
            typedInstance.MyFalseBoolean.Should().Be(false);
        }

        [Test]
        public void TestChildValues()
        {
            // Arrange
            var cache = new JsonModelBinderCache();
            var json = DynamicJsonObject.ReadJson(@"{'child':{'alpha':'my child'}}") as Dictionary<string, object>;
            Expression uncompiled;

            // Act
            var binder = cache.CreateBinderFor(typeof(TestClassE), out uncompiled);


            Console.WriteLine(MemberAccessHelper.GetDebugExpression(uncompiled));

            object instance = binder(json);

            // Assert
            // Assert
            instance.Should().BeOfType<TestClassE>();
            var typedInstance = instance as TestClassE;
            typedInstance.Child.Should().NotBeNull();

            typedInstance.Child.Alpha.Should().Be("my child");
        }


        static readonly object[] TestCaseTypes =
        {
            new TestCase<TestClassB, Int32>(o => o.MyInt32, 100).ToArray(),
            new TestCase<TestClassB, UInt32>(o => o.MyUInt32, 101).ToArray(),

            new TestCase<TestClassB, Int64>(o => o.MyInt64, 102).ToArray(),
            new TestCase<TestClassB, UInt64>(o => o.MyUInt64, 103).ToArray(),

            new TestCase<TestClassB, Int16>(o => o.MyInt16, 104).ToArray(),
            new TestCase<TestClassB, UInt16>(o => o.MyUInt16, 105).ToArray(),

            new TestCase<TestClassB, Byte>(o => o.MyByte, 106).ToArray(),
            new TestCase<TestClassB, SByte>(o => o.MySByte, 107).ToArray(),

            new TestCase<TestClassB, Decimal>(o => o.MyDecimal, 108.9m).ToArray(),
            new TestCase<TestClassB, Double>(o => o.MyDouble, 109.77).ToArray(),
            new TestCase<TestClassB, Single>(o => o.MySingle, 110.3645f).ToArray(),
        };
    }

    public class TestCase<TClass, TType >
    {
        public TestCase(Expression<Func<TClass, object>> accessor, TType expected)
        {
            Accessor = accessor.Compile();
            Expected = expected;

            Json = string.Format(@"{{'{0}': {1}}}", MemberAccessHelper.GetProperty(accessor).Name.ToLower(), expected);
        }

        public string Json { get; set; }
        public Func<TClass, object> Accessor { get; set; }
        public TType Expected { get; set; }

        public object[] ToArray()
        {
            return new object[]{Json, Accessor, Expected};
        }
    }

    public class TestClassA
    {
        public string Alpha { get; set; }
    }

    public class TestClassB
    {
        public Int32 MyInt32 { get; set; }
        public UInt32 MyUInt32 { get; set; }

        public Int64 MyInt64 { get; set; }
        public UInt64 MyUInt64 { get; set; }

        public Int16 MyInt16 { get; set; }
        public UInt16 MyUInt16 { get; set; }

        public Byte MyByte { get; set; }
        public SByte MySByte { get; set; }

        public Decimal MyDecimal { get; set; }
        public Double MyDouble { get; set; }
        public Single MySingle { get; set; }
    }

    public class TestClassC
    {
        public Char MyChar { get; set; }
    }

    public class TestClassD
    {
        public Boolean MyTrueBoolean { get; set; }
        public Boolean MyFalseBoolean { get; set; }
    }

    public class TestClassE
    {
        public TestClassA Child { get; set; }
    }
}