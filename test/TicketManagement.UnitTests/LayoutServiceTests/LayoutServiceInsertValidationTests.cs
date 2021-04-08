using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Interfaces.RepositoryExtension;
using TicketManagement.VenueAPI.Services;

namespace TicketManagement.UnitTests.LayoutServiceTests
{
    [TestFixture]
    public class LayoutServiceInsertValidationTests
    {
        private static List<LayoutData> _layouts = new List<LayoutData>
            {
            new LayoutData { Id = 1, Description = "First Layout", VenueId = 1, },
            new LayoutData { Id = 2, Description = "Second Layout", VenueId = 1, },
            new LayoutData { Id = 3, Description = "Third Layout", VenueId = 1, },
            new LayoutData { Id = 4, Description = "Fourth Layout", VenueId = 2, },
            new LayoutData { Id = 5, Description = "Fifth Layout", VenueId = 2, },
            new LayoutData { Id = 6, Description = "Sixth Layout", VenueId = 3, },
            new LayoutData { Id = 7, Description = "Seventh Layout", VenueId = 3, },
            new LayoutData { Id = 8, Description = "Eights Layout", VenueId = 3, },
            new LayoutData { Id = 9, Description = "Ninth Layout", VenueId = 4, },
            new LayoutData { Id = 10, Description = "Tenth Layout", VenueId = 4, },
            new LayoutData { Id = 11, Description = "Eleventh Layout", VenueId = 4, },
            new LayoutData { Id = 12, Description = "Twelveth Layout", VenueId = 4, },
            };

        [Test]
        public void IsValid_WhenInsertLayoutSuccess_ShouldReturnIdOfLayout()
        {
            // Arrange
            var layoutTest = new LayoutData
            {
                Description = "Test Layout",
                VenueId = 2,
            };

            var mockRepository = new Mock<ILayoutRepositoryExtension>();
            mockRepository.Setup(repo => repo.FilterByNameInVenue(layoutTest)).Returns(FilterByNameInVenueTests(layoutTest));
            var extendedMockRepository = mockRepository.As<IRepository<LayoutData>>();
            extendedMockRepository.Setup(repo => repo.Insert(layoutTest)).Returns(CreateTests(layoutTest));
            var layoutService = new LayoutService(extendedMockRepository.Object);

            // Act
            var result = layoutService.Insert(layoutTest);

            // Assert
            Assert.AreEqual(result, _layouts.Last().Id);
        }

        [Test]
        public void IsValid_WhenInsertSeatFailed_ShouldThrowException()
        {
            // Arrange
            var layoutTest = new LayoutData
            {
                Description = "Fifth Layout",
                VenueId = 2,
            };

            var mockRepository = new Mock<ILayoutRepositoryExtension>();
            mockRepository.Setup(repo => repo.FilterByNameInVenue(layoutTest)).Returns(FilterByNameInVenueTests(layoutTest));
            var extendedMockRepository = mockRepository.As<IRepository<LayoutData>>();
            extendedMockRepository.Setup(repo => repo.Insert(layoutTest)).Returns(CreateTests(layoutTest));
            var layoutService = new LayoutService(extendedMockRepository.Object);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => layoutService.Insert(layoutTest));

            // Assert
            Assert.AreEqual("Description not unique", ex.Message);
        }

        private static List<LayoutData> FilterByNameInVenueTests(LayoutData entity)
        {
            return _layouts.Where(x => (x.Description == entity.Description) && (x.VenueId == entity.VenueId)).ToList();
        }

        private static int CreateTests(LayoutData entity)
        {
            entity.Id = _layouts.Count;
            _layouts.Add(entity);
            return _layouts.Last().Id;
        }
    }
}
