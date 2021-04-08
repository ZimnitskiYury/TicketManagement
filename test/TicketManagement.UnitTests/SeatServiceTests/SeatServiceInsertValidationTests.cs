using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Interfaces.RepositoryExtension;
using TicketManagement.EventAPI.Services;

namespace TicketManagement.UnitTests.SeatServiceTests
{
    [TestFixture]
    public class SeatServiceInsertValidationTests
    {
        private static List<Seat> _seats = new List<Seat>
            {
            new Seat { Id=1, AreaId = 1, Row = 1, Number = 1, },
            new Seat { Id=2, AreaId = 1, Row = 1, Number = 2, },
            new Seat { Id=3, AreaId = 1, Row = 1, Number = 3, },
            new Seat { Id=4, AreaId = 2, Row = 1, Number = 1, },
            new Seat { Id=5, AreaId = 2, Row = 1, Number = 2, },
            new Seat { Id=6, AreaId = 2, Row = 1, Number = 3, },
            new Seat { Id=7, AreaId = 2, Row = 2, Number = 1, },
            new Seat { Id=8, AreaId = 2, Row = 2, Number = 2, },
            new Seat { Id=9, AreaId = 2, Row = 2, Number = 3, },
            new Seat { Id=10, AreaId = 3, Row = 1, Number = 1, },
            new Seat { Id=11, AreaId = 3, Row = 1, Number = 2, },
            };

        [Test]
        public void IsValid_WhenInsertSeatSuccess_ShouldReturnIdOfSeat()
        {
            // Arrange
            var seatTest = new Seat
            {
                AreaId = 2,
                Row = 3,
                Number = 3,
            };

            var mockRepository = new Mock<ISeatRepositoryExtension>();
            mockRepository.Setup(repo => repo.FilterByRowAndNumberInArea(seatTest)).Returns(FilterByRowAndNumberInAreaTests(seatTest));
            var extendedMockRepository = mockRepository.As<IRepository<Seat>>();
            extendedMockRepository.Setup(repo => repo.Insert(seatTest)).Returns(CreateTests(seatTest));
            var seatService = new SeatService(extendedMockRepository.Object);

            // Act
            var result = seatService.Insert(seatTest);

            // Assert
            Assert.AreEqual(result, _seats.Last().Id);
        }

        [Test]
        public void IsValid_WhenInsertSeatFailed_ShouldThrowException()
        {
            // Arrange
            var seatTest = new Seat
            {
                AreaId = 1,
                Row = 1,
                Number = 1,
            };

            var mockRepository = new Mock<ISeatRepositoryExtension>();
            mockRepository.Setup(repo => repo.FilterByRowAndNumberInArea(seatTest)).Returns(FilterByRowAndNumberInAreaTests(seatTest));
            var extendedMockRepository = mockRepository.As<IRepository<Seat>>();
            extendedMockRepository.Setup(repo => repo.Insert(seatTest)).Returns(CreateTests(seatTest));
            var seatService = new SeatService(extendedMockRepository.Object);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => seatService.Insert(seatTest));

            // Assert
            Assert.AreEqual("Row and number should be unique for area", ex.Message);
        }

        private static List<Seat> FilterByRowAndNumberInAreaTests(Seat entity)
        {
            return _seats.Where(x => (x.Row == entity.Row) && (x.Number == entity.Number) && (x.AreaId == entity.AreaId)).ToList();
        }

        private static int CreateTests(Seat entity)
        {
            entity.Id = _seats.Count;
            _seats.Add(entity);
            return _seats.Last().Id;
        }
    }
}
