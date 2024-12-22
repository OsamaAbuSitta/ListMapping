using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListMapping.Tests
{
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
    }

    public class SourceComplex
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
    }

    public class DestinationComplex
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
    }

    [TestFixture]
    public class MapperNestedTests
    {
        [Test]
        public void MapList_WithComplexNestedObject_ShouldUpdateNestedProperties()
        {
            // Arrange
            var sourceList = new List<SourceComplex>
        {
            new SourceComplex
            {
                Id = 1,
                Name = "Source1",
                Address = new Address { Street = "123 Main St", City = "Metropolis" }
            },
            new SourceComplex
            {
                Id = 2,
                Name = "Source2",
                Address = new Address { Street = "456 Side Rd", City = "Gotham" }
            }
        };

            var destinationList = new List<DestinationComplex>
        {
            new DestinationComplex
            {
                Id = 1,
                Name = "OldDestination1",
                Address = new Address { Street = "Old Street", City = "Old City" }
            },
            new DestinationComplex
            {
                Id = 3,
                Name = "OldDestination3",
                Address = new Address { Street = "789 Old Lane", City = "Atlantis" }
            }
        };

            // Act
            // Using string-based property names for demonstration ("Id" in both classes)
            destinationList.MapList(sourceList, "Id", "Id");

            // Assert

            // 1. The existing item (Id=1) should be updated
            var updated = destinationList.FirstOrDefault(d => d.Id == 1);
            Assert.NotNull(updated, "Should still contain item with Id=1.");
            Assert.AreEqual("Source1", updated.Name, "Name should be updated.");
            Assert.AreEqual("123 Main St", updated.Address.Street, "Street should be updated.");
            Assert.AreEqual("Metropolis", updated.Address.City, "City should be updated.");

            // 2. The item with Id=3 from destination should be removed (not in the source)
            Assert.IsFalse(destinationList.Any(d => d.Id == 3), "Item with Id=3 should be removed.");

            // 3. A new item with Id=2 should be added
            var newlyAdded = destinationList.FirstOrDefault(d => d.Id == 2);
            Assert.NotNull(newlyAdded, "Should add new destination item with Id=2.");
            Assert.AreEqual("Source2", newlyAdded.Name);
            Assert.AreEqual("456 Side Rd", newlyAdded.Address.Street);
            Assert.AreEqual("Gotham", newlyAdded.Address.City);

            // Final count should be 2 (Id=1 updated, Id=2 added)
            Assert.AreEqual(2, destinationList.Count);
        }
    }

}
