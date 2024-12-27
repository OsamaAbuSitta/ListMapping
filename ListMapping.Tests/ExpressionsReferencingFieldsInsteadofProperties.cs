using System.Diagnostics;
using System.Threading;

namespace ListMapping.Tests
{
    public class ExpressionsReferencingFieldsInsteadOfProperties
    {
        public class SourceWithField
        {
            public int IdField; // Not a property
            public string Name { get; set; }
        }

        public class DestinationWithField
        {
            public int IdField; // Not a property
            public string Name { get; set; }
        }

        [Test]
        public void MapList_FieldExpression_NotSupportedOrIgnored()
        {
            // Arrange
            var sourceList = new List<SourceWithField>
            {
                new SourceWithField { IdField = 1, Name = "Source1" }
            };
            var destinationList = new List<DestinationWithField>();

            // Act & Assert
            // This often throws or is ignored, as reflection is trying to find a Property named "IdField".
            Assert.Throws<ArgumentException>(() =>
            {
                destinationList.MapList(
                    sourceList,
                    s => s.IdField,         // Expression referencing a field, not a property
                    d => d.IdField
                );
            });
        }


    }
}