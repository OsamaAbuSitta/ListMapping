using NUnit.Framework;
using System.Collections.Generic;

namespace ListMapping.Tests
{
    [TestFixture]
    public class MissingParameterlessConstructor
    {
        public class Source
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class DestinationNoDefaultCtor
        {
            public int Id { get; }
            public string Name { get; }

            // No parameterless constructor
            public DestinationNoDefaultCtor(int id, string name)
            {
                Id = id;
                Name = name;
            }
        }

        [Test]
        public void MapList_NoParameterlessCtorWithAllProperties_ShouldMapp()
        {
            // Arrange
            var sourceList = new List<Source>
            {
                new Source { Id = 1, Name = "Source1" }
            };
            var destinationList = new List<DestinationNoDefaultCtor>();

            // Act
            destinationList.MapList(sourceList);

            // Assert
            Assert.AreEqual(1, destinationList.Count);
            Assert.AreEqual("Source1", destinationList.First(d => d.Id == 1).Name);
        }
    }
}