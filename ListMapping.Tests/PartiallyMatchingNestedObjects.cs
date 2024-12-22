using System.Linq.Expressions;

namespace ListMapping.Tests
{
    [TestFixture]
    public class PartiallyMatchingNestedObjects
    {
        public class ExtendedAddress : Address
        {
            public string ZipCode { get; set; }
        }

        public class SourceComplexExtended
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Address Address { get; set; } // Missing ZipCode
        }

        public class DestinationComplexExtended
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public ExtendedAddress Address { get; set; } // Has ZipCode
        }

        [Test]
        public void MapList_PartialNestedProperties_MaySkipSomeData()
        {
            // Arrange
            var sourceList = new List<SourceComplexExtended>
                {
                    new SourceComplexExtended
                    {
                        Id = 1,
                        Name = "Source",
                        Address = new Address { Street = "123", City = "Metropolis" }
                    }
                };
            var destinationList = new List<DestinationComplexExtended>
                {
                    new DestinationComplexExtended
                    {
                        Id = 1,
                        Name = "Destination",
                        Address = new ExtendedAddress { Street = "Old", City = "OldCity", ZipCode = "00000" }
                    }
                };

            // Act
            destinationList.MapList(sourceList, "Id", "Id");

            // Assert
            // The Street and City will get updated from source, but ZipCode remains unchanged
            // because the Source doesn't have ZipCode at all.
            var updated = destinationList.First();
            Assert.AreEqual("123", updated.Address.Street);
            Assert.AreEqual("Metropolis", updated.Address.City);
            Assert.AreEqual("00000", updated.Address.ZipCode,
                "ZipCode remains from the original destination because source doesn't provide it.");
        }
    }
}