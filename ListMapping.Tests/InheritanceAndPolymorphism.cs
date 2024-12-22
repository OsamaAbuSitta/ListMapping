using System.Reflection;

namespace ListMapping.Tests
{

    [TestFixture]
    public class InheritanceAndPolymorphism
    {
        public class BaseSource
        {
            public int Id { get; set; }
            public string BaseName { get; set; }
        }

        public class DerivedSource : BaseSource
        {
            public string ExtraInfo { get; set; }
        }

        public class BaseDestination
        {
            public int Id { get; set; }
            public string BaseName { get; set; }
        }

        public class DerivedDestination : BaseDestination
        {
            public string ExtraInfo { get; set; }
        }

        [Test]
        public void MapList_Polymorphism_ShouldMissDerivedPropsIfWrongTypes()
        {
            // Arrange
            var derivedSource = new List<DerivedSource>
    {
        new DerivedSource { Id = 1, BaseName = "Base1", ExtraInfo = "Extra1" }
    };

            // Destination is of base type only
            var baseDestinations = new List<BaseDestination>();

            // Act
            baseDestinations.MapList(derivedSource, "Id", "Id");

            // Assert
            // The "ExtraInfo" won't map because BaseDestination doesn't have it.
            Assert.IsTrue(baseDestinations.Any(d => d.Id == 1));
            // No property "ExtraInfo" in baseDestinations -> data is lost.
        }
    }
}