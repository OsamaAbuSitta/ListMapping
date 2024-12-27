using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListMapping.Tests
{
    public class SourceWithDuplicateId
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class DestinationWithDuplicateId
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [TestFixture]
    public class ExtendedMapperTests
    {
        #region Test Classes
        private class Source
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private class Destination
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private class SourceNoId
        {
            public string SomeProperty { get; set; }
        }

        private class DestinationNoId
        {
            public string SomeProperty { get; set; }
        }
        #endregion

        #region Setup

        [SetUp]
        public void Setup()
        {

            Mapper.DefaultMapperFactory = new DefaultMapperFactory();

        }
        #endregion

        #region Basic String-based Tests

        [Test]
        public void MapList_StringSelector_ShouldUpdateAndRemoveAndAdd()
        {
            var sourceList = new List<Source>
                {
                    new Source { Id = 1, Name = "Source1" },
                    new Source { Id = 2, Name = "Source2" },
                    new Source { Id = 3, Name = "Source3" }
                };

            var destinationList = new List<Destination>
                {
                    new Destination { Id = 2, Name = "OldDestination2" },
                    new Destination { Id = 3, Name = "OldDestination3" },
                    new Destination { Id = 4, Name = "OldDestination4" }
                };

            // Arrange & Act
            destinationList.MapList(sourceList, "Id", "Id");

            // Assert
            // 1) Updated existing Id = 2 and Id = 3
            Assert.AreEqual("Source2", destinationList.First(d => d.Id == 2).Name);
            Assert.AreEqual("Source3", destinationList.First(d => d.Id == 3).Name);

            // 2) Removed old Id = 4
            Assert.IsFalse(destinationList.Any(d => d.Id == 4), "Item with Id=4 should be removed.");

            // 3) Added new Id = 1
            Assert.IsTrue(destinationList.Any(d => d.Id == 1), "Item with Id=1 should be added.");
            Assert.AreEqual("Source1", destinationList.First(d => d.Id == 1).Name);

            // Final count should be 3
            Assert.AreEqual(3, destinationList.Count);
        }

        [Test]
        public void MapList_StringSelector_ShouldThrowForMissingIdInSource()
        {
            // Arrange
            var invalidSourceList = new List<SourceNoId> { new SourceNoId { SomeProperty = "Hello" } };
         
            var destinationList = new List<Destination>
                {
                    new Destination { Id = 2, Name = "OldDestination2" },
                    new Destination { Id = 3, Name = "OldDestination3" },
                    new Destination { Id = 4, Name = "OldDestination4" }
                };
            // Act & Assert
            Assert.Throws<NotSupportedException>(() =>
                destinationList.MapList(invalidSourceList, "Id", "Id"));
        }

        [Test]
        public void MapList_StringSelector_ShouldThrowForMissingIdInDestination()
        {
            // Arrange
            var invalidDestinationList = new List<DestinationNoId> { new DestinationNoId { SomeProperty = "Hello" } };
            var sourceList = new List<Source>
                {
                    new Source { Id = 1, Name = "Source1" },
                    new Source { Id = 2, Name = "Source2" },
                    new Source { Id = 3, Name = "Source3" }
                };

            // Act & Assert
            Assert.Throws<NotSupportedException>(() =>
                invalidDestinationList.MapList(sourceList, "Id", "Id"));
        }

        #endregion

        #region Basic Lambda-based Tests

        [Test]
        public void MapList_LambdaSelector_ShouldUpdateAndRemoveAndAdd()
        {
            // Arrange
            var sourceList = new List<Source>
                {
                    new Source { Id = 1, Name = "Source1" },
                    new Source { Id = 2, Name = "Source2" },
                    new Source { Id = 3, Name = "Source3" }
                };

            var destinationList = new List<Destination>
                {
                    new Destination { Id = 2, Name = "OldDestination2" },
                    new Destination { Id = 3, Name = "OldDestination3" },
                    new Destination { Id = 4, Name = "OldDestination4" }
                };

            // Arrange & Act
            destinationList.MapList(
                sourceList,
                s => s.Id,
                d => d.Id
            );

            // Assert (similar checks as the string-based test)
            Assert.AreEqual("Source2", destinationList.First(d => d.Id == 2).Name);
            Assert.AreEqual("Source3", destinationList.First(d => d.Id == 3).Name);
            Assert.IsFalse(destinationList.Any(d => d.Id == 4), "Should remove item with Id=4.");
            Assert.IsTrue(destinationList.Any(d => d.Id == 1), "Should add item with Id=1.");
            Assert.AreEqual(3, destinationList.Count);
        }

        #endregion

        #region Edge Cases

        [Test]
        public void MapList_ShouldHandleEmptySourceList()
        {
            // Arrange
            var emptySource = new List<Source>();
            var destinationList = new List<Destination>
                {
                    new Destination { Id = 2, Name = "OldDestination2" },
                    new Destination { Id = 3, Name = "OldDestination3" },
                    new Destination { Id = 4, Name = "OldDestination4" }
                };
            // Act
            destinationList.MapList(emptySource, "Id", "Id");

            // Assert
            // Should remove all from destination because none match in the source
            Assert.IsEmpty(destinationList, "All items should be removed if the source list is empty.");
        }

        [Test]
        public void MapList_ShouldHandleEmptyDestinationList()
        {
            // Arrange
            var emptyDestination = new List<Destination>();
            var sourceList = new List<Source>
                {
                    new Source { Id = 1, Name = "Source1" },
                    new Source { Id = 2, Name = "Source2" },
                    new Source { Id = 3, Name = "Source3" }
                };
            // Act
            emptyDestination.MapList(sourceList, "Id", "Id");

            // Assert
            // Should add all items from source
            Assert.AreEqual(sourceList.Count, emptyDestination.Count, "All source items should be added.");
        }

        [Test]
        public void MapList_ShouldThrowIfNullLists()
        {
            var sourceList = new List<Source>
             {
                 new Source { Id = 1, Name = "Source1" },
                 new Source { Id = 2, Name = "Source2" },
                 new Source { Id = 3, Name = "Source3" }
             };

                    var destinationList = new List<Destination>
             {
                 new Destination { Id = 2, Name = "OldDestination2" },
                 new Destination { Id = 3, Name = "OldDestination3" },
                 new Destination { Id = 4, Name = "OldDestination4" }
             };
            Assert.Throws<ArgumentNullException>(() =>
                ((List<Destination>)null).MapList(sourceList, "Id", "Id"),
                "Should throw if destination list is null.");

            Assert.Throws<ArgumentNullException>(() =>
                destinationList.MapList((List<Source>)null, "Id", "Id"),
                "Should throw if source list is null.");
        }

        [Test]
        public void MapList_WithDuplicateIds_ShouldOnlyKeepOneDestination()
        {
            // Arrange
            var duplicateSources = new List<SourceWithDuplicateId>
        {
            new SourceWithDuplicateId { Id = 1, Name = "Duplicate-Source1" },
            new SourceWithDuplicateId { Id = 1, Name = "Duplicate-Source2" } // same Id as above
        };

            var duplicateDestinations = new List<DestinationWithDuplicateId>
        {
            new DestinationWithDuplicateId { Id = 1, Name = "ExistingDestination" }
        };

            // Act
            duplicateDestinations.MapList(duplicateSources, "Id", "Id");

            // Assert
            // The final list will end up having one Id=1. 
            // Depending on your design, it will likely be the last updated. 
            // Mapster will adapt the second source onto the existing destination.

            Assert.AreEqual(1, duplicateDestinations.Count, "Should only have one destination with Id=1");
            Assert.AreEqual("Duplicate-Source2", duplicateDestinations[0].Name,
                "Should have been updated by the last matching source item.");
        }

        [Test]
        public void MapList_LambdaSelector_WithCustomProperty()
        {
            // Arrange
            var sourceListCustom = new List<SourceCustom>
        {
            new SourceCustom { CustomId = 100, Name = "CustomSource1" }
        };

            var destinationListCustom = new List<DestinationCustom>();

            // Act
            destinationListCustom.MapList(sourceListCustom, s => s.CustomId, d => d.CustomId);

            // Assert
            Assert.AreEqual(1, destinationListCustom.Count);
            Assert.AreEqual(100, destinationListCustom[0].CustomId);
            Assert.AreEqual("CustomSource1", destinationListCustom[0].Name);
        }

        #endregion
    }

    #region Additional Classes for Custom Property Testing
    public class SourceCustom
    {
        public int CustomId { get; set; }
        public string Name { get; set; }
    }

    public class DestinationCustom
    {
        public int CustomId { get; set; }
        public string Name { get; set; }
    }
    #endregion

}
