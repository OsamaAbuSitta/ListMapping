using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListMapping.Tests
{
    [TestFixture]
    public class RaceConditionsInMultiThreadedScenarios
    {

        [SetUp]
        public void Setup()
        {

            Mapper.DefaultMapperFactory = new DefaultMapperFactory();
        }

        [Test]
        public void MapList_MultiThreadedMapping_RaceConditionExample()
        {
            // Arrange
            // For demonstration: 10 source items with Ids 1 through 10
            var sourceList = Enumerable.Range(1, 10)
                .Select(i => new Source { Id = i, Name = $"Source{i}" })
                .ToList();

            // Destination starts with two items (Ids 2 & 4) to illustrate changes
            var destinationList = new List<Destination>
            {
                new Destination { Id = 2, Name = "OldDestination2" },
                new Destination { Id = 4, Name = "OldDestination4" }
            };

            // Act
            // We'll run 100 parallel iterations. Some use the string-based approach,
            // others use the lambda-based approach.
            Parallel.ForEach(Enumerable.Range(1, 100), i =>
            {
                if (i % 2 == 0)
                {
                    // Even iterations: string-based property selection
                    destinationList.MapList(sourceList, "Id", "Id");
                }
                else
                {
                    // Odd iterations: lambda-based property selection
                    destinationList.MapList(sourceList, s => s.Id, d => d.Id);
                }
            });

            // Assert
            // The primary goal is to see if it runs without exceptions or race-condition crashes.
            // You can add additional assertions here, e.g. checking final list count, etc.
            Assert.Pass("Completed without throwing exceptions. Check logs or final list state if needed.");
        }
    }

    // Example classes for demonstration
    public class Source
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Destination
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}