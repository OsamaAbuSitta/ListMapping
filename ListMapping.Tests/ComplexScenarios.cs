namespace ListMapping.Tests
{
    [TestFixture]
    public class ComplexScenarios
    {
        [SetUp]
        public void Setup()
        {

            Mapper.DefaultMapperFactory = new DefaultMapperFactory();
        }

        #region Property Type Mismatch

        public class SourceStringId
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public class DestinationIntId
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        [Test]
        public void MapList_PropertyTypeMismatch_ShouldFailOrMissMap()
        {
            // Arrange
            var sourceList = new List<SourceStringId>
                {
                    new SourceStringId { Id = "1", Name = "Source1" },
                    new SourceStringId { Id = "2", Name = "Source2" }
                };

            var destinationList = new List<DestinationIntId>
                {
                    new DestinationIntId { Id = 2, Name = "OldDestination2" }
                };

            // Act & Assert
            // The comparison sourceIdAccessor(s).Equals(destinationIdAccessor(d)) might fail
            // or simply never match because "2" != 2, depending on how Mapster handles equality.
            // This often results in no updates happening or an exception.
            destinationList.MapList(sourceList);
            Assert.AreEqual(2, destinationList.Count);
            Assert.AreEqual("Source1", destinationList.First(d => d.Id == 1).Name);
            Assert.AreEqual("Source2", destinationList.First(d => d.Id == 2).Name);
        }

        #endregion Property Type Mismatch

        #region Circular or Recursive References

        public class Node
        {
            public int Id { get; set; }
            public Node Parent { get; set; }
            public List<Node> Children { get; set; } = new();
        }

        //[Test]
        //public void MapList_CircularReferences_ShouldPotentiallyInfiniteLoop()
        //{
        //    // Arrange
        //    var child = new Node { Id = 2 };
        //    var parent = new Node { Id = 1, Children = new List<Node> { child } };
        //    child.Parent = parent; // circular reference

        //    var sourceList = new List<Node> { parent };
        //    var destinationList = new List<Node>();

        //    // Act & Assert
        //    // By default, naive mapping might attempt to follow the circle:
        //    // parent -> child -> parent -> child -> ...
        //    // This can cause an infinite loop or stack overflow
        //    Assert.Throws<StackOverflowException>(() =>
        //    {
        //        destinationList.MapList(sourceList, "Id", "Id");
        //    });
        //}

        #endregion Circular or Recursive References

        #region Read-Only or Private Set Properties

        public class SourceWithReadOnly
        {
            public int Id { get; set; }
            public string Immutable { get; set; }
        }

        public class DestinationWithReadOnly
        {
            public int Id { get; set; }

            // Read-only property (no setter)
            public string Immutable { get; } = "InitialValue";
        }

        [Test]
        public void MapList_ReadOnlyProperties_ShouldFailOrRemainUnchanged()
        {
            // Arrange
            var sourceList = new List<SourceWithReadOnly>
                    {
                        new SourceWithReadOnly { Id = 1, Immutable = "NewValue" }
                    };
            var destinationList = new List<DestinationWithReadOnly>
                    {
                        new DestinationWithReadOnly { Id = 1, /* Immutable remains "InitialValue" */ }
                    };

            // Act
            // May not throw an exception, but "Immutable" won't be updated
            // unless a custom config or constructor-based approach is used.
            destinationList.MapList(sourceList, "Id", "Id");

            // Assert
            Assert.AreEqual("InitialValue", destinationList[0].Immutable,
                "Immutable property was not updated because it's read-only.");
        }
    }

    #endregion Read-Only or Private Set Properties
}