using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Interfaces.RepositoryExtension;
using TicketManagement.VenueAPI.Services;

namespace TicketManagement.UnitTests.VenueServiceTests
{
    [TestFixture]
    public class VenueServiceInsertValidationTests
    {
        private static List<Venue> _venues = new List<Venue>
            {
            new Venue { Id=0, Description = "Test Venue 1", Address = "First Venue address", Phone = "3754444855", },
            new Venue { Id=1, Description = "Test Venue 2", Address = "Second Venue address", Phone = "375454455", },
            new Venue { Id=2, Description = "Test Event 3", Address = "Third Venue address", Phone = "3754788855", },
            new Venue { Id=3, Description = "Test Event 4", Address = "Fourth Venue address", Phone = "37855525", },
            new Venue { Id=4, Description = "Test Event 5", Address = "Fifth Venue address", Phone = "754678697", },
            new Venue { Id=5, Description = "Test Event 6", Address = "Sixth Venue address", Phone = "3765786855", },
            new Venue { Id=6, Description = "Test Event 7", Address = "Seventh Venue address", Phone = "8786666444855", },
            new Venue { Id=7, Description = "Test Event 8", Address = "Eighth Venue address", Phone = "37777444855", },
            new Venue { Id=8, Description = "Test Event 9", Address = "Ninth Venue address", Phone = "33335422855", },
            new Venue { Id=9, Description = "Test Event 10", Address = "Tenth Venue address", Phone = "3712214855", },
            new Venue { Id=10, Description = "Test Event 11", Address = "Eleventh Venue address", Phone = "33255855", },
            };

        [Test]
        public void IsValid_WhenInsertVenueSuccess_ShouldReturnIdOfEvent()
        {
            // Arrange
            var venueTest = new Venue
            {
                Description = "Test Venue",
                Address = "Test Venue Address",
                Phone = "37544876921",
            };
            var mockLayoutRepository = new Mock<IRepository<LayoutData>>();
            var mockAreaRepository = new Mock<IRepository<Area>>();
            var mockSeatRepository = new Mock<IRepository<Seat>>();
            var mockRepository = new Mock<IVenueRepositoryExtension>();
            mockRepository.Setup(repo => repo.FilterByName(venueTest)).Returns(FilterByNameTests(venueTest));
            var extendedMockRepository = mockRepository.As<IRepository<Venue>>();
            extendedMockRepository.Setup(repo => repo.Insert(venueTest)).Returns(CreateTests(venueTest));
            var venueService = new VenueService(extendedMockRepository.Object, mockLayoutRepository.Object, mockSeatRepository.Object, mockAreaRepository.Object);

            // Act
            var result = venueService.Insert(venueTest);

            // Assert
            Assert.AreEqual(result, _venues.Last().Id);
        }

        [Test]
        public void IsValid_WhenInsertVenueFailed_ShouldThrowException()
        {
            // Arrange
            var venueTest = new Venue
            {
                Description = "Test Venue 1",
                Address = "Test Venue Address",
                Phone = "37544876921",
            };
            var mockLayoutRepository = new Mock<IRepository<LayoutData>>();
            var mockAreaRepository = new Mock<IRepository<Area>>();
            var mockSeatRepository = new Mock<IRepository<Seat>>();
            var mockRepository = new Mock<IVenueRepositoryExtension>();
            mockRepository.Setup(repo => repo.FilterByName(venueTest)).Returns(FilterByNameTests(venueTest));
            var extendedMockRepository = mockRepository.As<IRepository<Venue>>();
            extendedMockRepository.Setup(repo => repo.Insert(venueTest)).Returns(CreateTests(venueTest));
            var venueService = new VenueService(extendedMockRepository.Object, mockLayoutRepository.Object, mockSeatRepository.Object, mockAreaRepository.Object);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => venueService.Insert(venueTest));

            // Assert
            Assert.AreEqual("Not Unique name of venue", ex.Message);
        }

        private static List<Venue> FilterByNameTests(Venue entity)
        {
            return _venues.Where(s => s.Description == entity.Description).ToList();
        }

        private static int CreateTests(Venue entity)
        {
            entity.Id = _venues.Count;
            _venues.Add(entity);
            return _venues.Last().Id;
        }
    }
}
