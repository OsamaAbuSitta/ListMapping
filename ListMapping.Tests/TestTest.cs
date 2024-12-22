using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ListMapping.Tests.ComplexScenarios;

namespace ListMapping.Tests
{
    [TestFixture]
    public class TestTest
    {

        [Test]
        public void MapList_Should_UpdateExistingItems()
        {
            // Arrange
            var sourceList = new List<SourceStringId>
                {
                    new SourceStringId { Id = "1", Name = "Source1" },
                    new SourceStringId { Id = "2", Name = "Source2" }
                };

            var destinationList = new List<SourceStringId>
                {
                    new SourceStringId { Id = "2", Name = "OldDestination2" }
                };

            // Act & Assert
            destinationList.MapList(sourceList, "Id", "Id");


            Assert.That(() =>
                destinationList.Any(x => x.Id == "1") && 
                destinationList.Any(x => x.Id == "2")
            );

        }


        [Test]
        public void MapList_Should_UpdateExistingItems_Int_Id()
        {
            // Arrange
            var sourceList = new List<DestinationIntId>
                {
                    new DestinationIntId { Id = 1, Name = "Source1" },
                    new DestinationIntId { Id = 2, Name = "Source2" }
                };

            var destinationList = new List<DestinationIntId>
                {
                    new DestinationIntId { Id = 2, Name = "OldDestination2" }
                };

            // Act & Assert
            destinationList.MapList(sourceList, "Id", "Id");


            Assert.That(() =>
                destinationList.Any(x => x.Id == 1) &&
                destinationList.Any(x => x.Id == 2)
            );

        }
    }
}
