using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using FluentAssertions;
using NUnit.Framework;
using prepo.Api.Infrastructure;
using prepo.Api.Infrastructure.Reflecting;

namespace prepo.Api.Tests.UnitTests
{
    [TestFixture]
    public class TestMethodAccessHelper
    {
        [Test]
        public void CanGetReferenceProperty()
        {
            // Act
            var propertyInfo = MemberAccessHelper.GetProperty<TestClassAlpha>(x => x.MyFirstStringProperty);

            // Assert
            ((object) propertyInfo).Should().NotBeNull();
            propertyInfo.Name.Should().Be("MyFirstStringProperty");
        }
        
        [Test]
        public void CanGetComplexProperty()
        {
            // Act
            var propertyInfo = MemberAccessHelper.GetProperty<TestClassWithComplexProperty>(x => x.TestClassAlphaProperty);

            // Assert
            ((object) propertyInfo).Should().NotBeNull();
            propertyInfo.Name.Should().Be("TestClassAlphaProperty");
        }

        [Test]
        public void GettingSubPropertyWorks()
        {
            // Act
            var propertyInfo = MemberAccessHelper.GetProperty<TestClassWithComplexProperty>(x => x.TestClassAlphaProperty.MyFirstStringProperty);

            // Assert
            ((object)propertyInfo).Should().NotBeNull();
            propertyInfo.Name.Should().Be("MyFirstStringProperty");
        }

        [Test]
        public void CanGetValueProperty()
        {
            // Act
            var propertyInfo = MemberAccessHelper.GetProperty<TestClassAlpha>(x => x.MyFirstIntProperty);

            // Assert
            ((object)propertyInfo).Should().NotBeNull();
            propertyInfo.Name.Should().Be("MyFirstIntProperty");
        }

        [Test]
        public void GetMethodThrowsException()
        {
            // Act
           Action act = ()=> MemberAccessHelper.GetProperty<TestClassAlpha>(x => x.MyStringMethod());

            // Assert
           var subject = act.ShouldThrow<MemberAccessHelperException>().Subject;
           Console.WriteLine(subject.Message);
        }

        [Test]
        public void GetFieldThrowsException()
        {
            // Act
            Action act = () => MemberAccessHelper.GetProperty<TestClassAlpha>(x => x.MyField);

            // Assert
            var subject = act.ShouldThrow<MemberAccessHelperException>().Subject;
            Console.WriteLine(subject.Message);
        }

        [Test]
        public void CanGetDefaultIndexer()
        {
            // Act
            var propertyInfo = MemberAccessHelper.GetIndexerProperty<TestClassAlpha>();

            // Assert
            ((object)propertyInfo).Should().NotBeNull();
            propertyInfo.Name.Should().Be("Item");
        }

        [Test]
        public void CanGetNamedIndexer()
        {
            // Act
            var propertyInfo = MemberAccessHelper.GetIndexerProperty<TestClassNamedIndexer>();

            // Assert
            ((object)propertyInfo).Should().NotBeNull();
            propertyInfo.Name.Should().Be("SomeCrazyValue");
        }

        [Test]
        public void GetIndexerWithNoIndexorThrowsException()
        {
            // Act
            Action act = () => MemberAccessHelper.GetIndexerProperty<object>();

            // Assert
            var subject = act.ShouldThrow<MemberAccessHelperException>().Subject;
            Console.WriteLine(subject.Message);
        }

        [Test]
        public void CanGetReferenceMethod()
        {
            // Act
            var methodInfo = MemberAccessHelper.GetMethod<TestClassAlpha>(x => x.MyStringMethod());

            // Assert
            ((object)methodInfo).Should().NotBeNull();
            methodInfo.Name.Should().Be("MyStringMethod");
        }

        [Test]
        public void CanGetValueMethod()
        {
            // Act
            var methodInfo = MemberAccessHelper.GetMethod<TestClassAlpha>(x => x.MyIntMethod());

            // Assert
            ((object)methodInfo).Should().NotBeNull();
            methodInfo.Name.Should().Be("MyIntMethod");
        }

        [Test]
        public void CanGetExpressionDebugInfo()
        {
            // Arrange
            var expression = Expression.Constant(0);

            // Act
            var debug = MemberAccessHelper.GetDebugExpression(expression);

            // Assert
            Console.WriteLine(debug);
        }

        class TestClassNamedIndexer
        {
            [IndexerName("SomeCrazyValue")]
            public object this[int index]
            {
                get { return null; }
                set { }
            }
        }

        class TestClassAlpha
        {
            public object MyField;

            public string MyFirstStringProperty { get; set; }
            public int MyFirstIntProperty { get; set; }

            public object this[int index]
            {
                get { return null; }
                set {  }
            }

            public string MyStringMethod()
            {
                return null;
            }

            public int MyIntMethod()
            {
                return 0;
            }
        }

        class TestClassWithComplexProperty
        {
            public TestClassAlpha TestClassAlphaProperty { get; set; }
        }
    }
}