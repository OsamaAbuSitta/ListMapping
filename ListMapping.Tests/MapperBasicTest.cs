namespace ListMapping.Tests
{
    [TestFixture]
    public class MapListTests
    {
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

        private List<Source> _sourceList;
        private List<Destination> _destinationList;

        [SetUp]
        public void Setup()
        {
            Mapper.DefaultMapperFactory = new DefaultMapperFactory();
            _sourceList = new List<Source>
                    {
                        new Source { Id = 1, Name = "Source1" },
                        new Source { Id = 2, Name = "Source2" },
                        new Source { Id = 3, Name = "Source3" }
                    };

            _destinationList = new List<Destination>
                    {
                        new Destination { Id = 2, Name = "OldDestination2" },
                        new Destination { Id = 3, Name = "OldDestination3" },
                        new Destination { Id = 4, Name = "OldDestination4" }
                    };
        }

        [Test]
        public void MapList_Should_UpdateExistingItems()
        {
            // Arrange
            var mapper = new Mapper();

            // Act
            mapper.MapList(_sourceList, _destinationList);

            // Assert
            Assert.AreEqual(3, _destinationList.Count);
            Assert.AreEqual("Source2", _destinationList.First(d => d.Id == 2).Name);
            Assert.AreEqual("Source3", _destinationList.First(d => d.Id == 3).Name);
        }

        [Test]
        public void MapList_Should_RemoveNonMatchingItems()
        {
            // Arrange
            var mapper = new Mapper();

            // Act
            mapper.MapList(_sourceList, _destinationList);

            // Assert
            Assert.IsFalse(_destinationList.Any(d => d.Id == 4), "Item with Id=4 should have been removed.");
        }

        [Test]
        public void MapList_Should_AddNewItems()
        {
            // Arrange
            var mapper = new Mapper();

            // Act
            mapper.MapList(_sourceList, _destinationList);

            // Assert
            Assert.IsTrue(_destinationList.Any(d => d.Id == 1), "Item with Id=1 should have been added.");
            Assert.AreEqual("Source1", _destinationList.First(d => d.Id == 1).Name);
        }

        [Test]
        public void MapList_Should_ThrowException_WhenSourceMissingId()
        {
            // Arrange
            var mapper = new Mapper();
            var invalidSourceList = new List<object> { new { Name = "Invalid" } };
            var validDestinationList = new List<Destination>();

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                mapper.MapList((IList<dynamic>)invalidSourceList, validDestinationList)
            );
        }

        [Test]
        public void MapList_Should_ThrowException_WhenDestinationMissingId()
        {
            // Arrange
            var mapper = new Mapper();
            var validSourceList = new List<Source>();
            var invalidDestinationList = new List<object> { new { Name = "Invalid" } };

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                mapper.MapList(validSourceList, (IList<dynamic>)invalidDestinationList)
            );
        }

        [Test]
        public void MapList_Should_HandleEmptyLists()
        {
            // Arrange
            var mapper = new Mapper();
            var emptySourceList = new List<Source>();
            var emptyDestinationList = new List<Destination>();

            // Act
            mapper.MapList(emptySourceList, emptyDestinationList);

            // Assert
            Assert.IsEmpty(emptyDestinationList);
        }

        [Test]
        public void MapList_Should_HandleNullLists()
        {
            // Arrange
            var mapper = new Mapper();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => mapper.MapList<Source, Destination>(null, _destinationList));
            Assert.Throws<ArgumentNullException>(() => mapper.MapList<Source, Destination>(_sourceList, null));
        }
    }
}