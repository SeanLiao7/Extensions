using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using HiLoExtensions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Shouldly;

namespace HiLoExtensionsTests
{
    [TestFixture]
    public class ExtensionsTests
    {
        private const int Max = 11;
        private const int Min = 1;

        [Test]
        public void Copy_AnonymousObject_ReferenceShouldNotBeEqual( )
        {
            var expected = CreateAnonymousObject( );
            var acutal = expected.Copy( );
            var isReferenceEqual = acutal == expected;
            isReferenceEqual.ShouldBeFalse( );
        }

        [Test]
        public void Copy_AnonymousObject_ShouldBeEqual( )
        {
            var expected = CreateAnonymousObject( );
            var acutal = expected.Copy( );
            acutal.ShouldBeEquivalentTo( expected );
        }

        [Test]
        public void Copy_DummyObject_ReferenceShouldNotBeEqual( )
        {
            var expected = CreateDummyObject( );
            var acutal = expected.Copy( );
            var isReferenceEqual = acutal == expected;
            isReferenceEqual.ShouldBeFalse( );
        }

        [Test]
        public void Copy_DummyObject_ShouldBeEqual( )
        {
            var expected = CreateDummyObject( );
            var actual = expected.Copy( );
            actual.ShouldBeEquivalentTo( expected );
        }

        [Test]
        public void Copy_DummyStruct_ShouldBeEqual( )
        {
            var expected = CreateDummyStruct( );
            var actual = expected.Copy( );
            actual.ShouldBeEquivalentTo( expected );
        }

        [Test]
        [Description( "FluentAssertions fail to compare List in ValueTuple" )]
        public void Copy_ValueTuple_ShouldBeEqual( )
        {
            var expected = (Age: 27, Name: "Ted", Grade: 5.2d);
            var actual = expected.Copy( );
            actual.ShouldBeEquivalentTo( expected );
        }

        private static object CreateAnonymousObject( )
        {
            var fixture = new Fixture( );
            RegisterTestType( fixture );

            var age = fixture.Create<int>( );
            var name = fixture.Create<string>( );
            var list = fixture.Create<List<string>>( );
            var money = fixture.Create<double>( );
            var table = fixture.Create<Dictionary<int, string>>( );
            var complexTable = fixture.Create<Dictionary<string, List<string>>>( );
            var expected = new
            {
                List = list,
                Age = age,
                Name = name,
                Money = money,
                Table = table,
                ComplexTable = complexTable
            };
            return expected;
        }

        private static DummyObject CreateDummyObject( )
        {
            var fixture = new Fixture( );
            RegisterTestType( fixture );
            var expected = fixture.Create<DummyObject>( );
            return expected;
        }

        private static DummyStruct CreateDummyStruct( )
        {
            var fixture = new Fixture( );
            var customization = new SupportMutableValueTypesCustomization( );
            customization.Customize( fixture );
            RegisterTestType( fixture );
            var expected = fixture.Create<DummyStruct>( );
            return expected;
        }

        private static void RegisterTestType( IFixture fixture )
        {
            var random = new Random( );
            fixture.Register( ( ) =>
                {
                    var count = random.Next( Min, Max );
                    return fixture.CreateMany<string>( count ).ToList( );
                }
            );
            fixture.Register( ( ) =>
                {
                    var count = random.Next( Min, Max );
                    var table = new Dictionary<string, List<string>>( );
                    for ( var i = 0; i < count; i++ )
                    {
                        var key = fixture.Create<string>( );
                        var value = fixture.Create<List<string>>( );
                        table[ key ] = value;
                    }
                    return table;
                }
            );
        }

        private struct DummyStruct
        {
            public int Age { get; set; }
            public Dictionary<string, List<string>> ComplexTable { get; set; }
            public List<string> List { get; set; }
            public decimal Money { get; set; }
            public string Name { get; set; }
            public Dictionary<int, string> Table { get; set; }
        }

        private class DummyObject
        {
            public int Age { get; set; }
            public Dictionary<string, List<string>> ComplexTable { get; set; }
            public List<string> List { get; set; }
            public decimal Money { get; set; }
            public string Name { get; set; }
            public Dictionary<int, string> Table { get; set; }
        }
    }
}