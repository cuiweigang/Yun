﻿using System;
using MongoDB.Configuration;
using NUnit.Framework;

namespace MongoDB.UnitTests.Configuration
{
    [TestFixture]
    public class MongoConfigurationBuilderTests
    {
        private class Person
        {
            public Guid Id { get; set; }

	    public int Age { get; set; }

            public string Name { get; set; }
        }

        [Test]
        public void Test()
        {
            var configure = new MongoConfigurationBuilder();

            configure.ConnectionString(cs =>
            {
                cs.Pooled = true;
            });

            configure.Mapping(mapping => 
            {
                mapping.DefaultProfile(p =>
                {
                    p.AliasesAreCamelCased();
                    p.CollectionNamesAreCamelCasedAndPlural();
                });

                mapping.Map<Person>(m =>
                {
                    m.CollectionName("people");
                    m.Member(x => x.Age).Alias("age");
                    m.Member(x => x.Name).Alias("name").DefaultValue("something").Ignore();
                });
            });
        }
    }
}
