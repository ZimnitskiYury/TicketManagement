using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Interfaces.RepositoryExtension;
using TicketManagement.EventAPI.Services;

namespace TicketManagement.UnitTests.EventServiceTests
{
    [TestFixture]
    public class EventServiceInsertValidationTests
    {
        private static List<EventData> _events = new List<EventData>
        {
            new EventData
            {
            Id = 1,
            Name = "First Event Name",
            Description = "First Event Description",
            LayoutId = 1,
            StartDateTime = new DateTime(2020, 10, 13, 15, 00, 00),
            EndDateTime = new DateTime(2020, 10, 13, 19, 00, 00),
            },
            new EventData
            {
                Id=2,
                Name="Second Event Name",
                Description="Second Event Description",
                LayoutId=1,
                StartDateTime=new DateTime(2020, 11, 13, 15, 00, 00),
                EndDateTime=new DateTime(2020, 11, 14, 03, 00, 00),
            },
            new EventData
            {
                Id=3,
                Name="Third Event Name",
                Description="Third Event Description",
                LayoutId=2,
                StartDateTime=new DateTime(2020, 09, 22, 18, 00, 00),
                EndDateTime=new DateTime(2020, 09, 22, 23, 00, 00),
            },
            new EventData
            {
                Id = 4,
                Name = "Fourth Event Name",
                Description = "Fourth Event Description",
                LayoutId = 1,
                StartDateTime = new DateTime(2020, 09, 22, 15, 00, 00),
                EndDateTime = new DateTime(2020, 09, 22, 17, 00, 00),
            },
            new EventData
            {
                Id = 5,
                Name = "Fifth Event Name",
                Description = "Fifth Event Description",
                LayoutId = 4,
                StartDateTime = new DateTime(2020, 09, 23, 15, 00, 00),
                EndDateTime = new DateTime(2020, 10, 23, 19, 00, 00),
            },
            new EventData
            {
                Id = 6,
                Name = "Sixth Event Name",
                Description = "Sixth Event Description",
                LayoutId = 1,
                StartDateTime = new DateTime(2020, 09, 30, 18, 00, 00),
                EndDateTime = new DateTime(2020, 10, 01, 07, 00, 00),
            },
            new EventData
            {
                Id = 7,
                Name = "Seventh Event Name",
                Description = "Seventh Event Description",
                LayoutId = 6,
                StartDateTime = new DateTime(2020, 10, 15, 15, 00, 00),
                EndDateTime = new DateTime(2020, 10, 18, 19, 00, 00),
            },
            new EventData
            {
                Id = 8,
                Name = "Eighth Event Name",
                Description = "Eighth Event Description",
                LayoutId = 1,
                StartDateTime = new DateTime(2020, 10, 28, 15, 00, 00),
                EndDateTime = new DateTime(2020, 10, 28, 19, 00, 00),
            },
            new EventData
            {
                Id = 9,
                Name = "New Year Party Event",
                Description = "New Year Party",
                LayoutId = 1,
                StartDateTime = new DateTime(2020, 12, 31, 15, 00, 00),
                EndDateTime = new DateTime(2021, 01, 01, 09, 00, 00),
            },
            new EventData
            {
                Id = 10,
                Name = "Tenth Evenet Name",
                Description = "Tenth Event Description",
                LayoutId = 3,
                StartDateTime = new DateTime(2021, 10, 13, 15, 00, 00),
                EndDateTime = new DateTime(2021, 10, 13, 19, 00, 00),
            },
        };

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

        private static List<Area> _areas = new List<Area>
            {
            new Area { Id=1, Description="TestArea1", LayoutId=1, CoordX=0, CoordY=1, },
            new Area { Id=2, Description="TestArea2", LayoutId=1, CoordX=1, CoordY=1, },
            new Area { Id=3, Description="TestArea3", LayoutId=1, CoordX=2, CoordY=1, },
            new Area { Id=4, Description="TestArea4", LayoutId=1, CoordX=3, CoordY=1, },
            new Area { Id=5, Description="TestArea5", LayoutId=1, CoordX=4, CoordY=1, },
            new Area { Id=6, Description="TestArea6", LayoutId=2, CoordX=3, CoordY=1, },
            new Area { Id=7, Description="TestArea7", LayoutId=2, CoordX=3, CoordY=2, },
            new Area { Id=8, Description="TestArea8", LayoutId=2, CoordX=3, CoordY=3, },
            new Area { Id=9, Description="TestArea9", LayoutId=3, CoordX=1, CoordY=1, },
            new Area { Id=10, Description="TestArea10", LayoutId=3, CoordX=2, CoordY=2, },
            };

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

        private static LayoutData _layout;

        [Test]
        public void IsValid_WhenInsertEventSuccess_ShouldReturnId()
        {
            // Arrange
            var eventTest = new EventData
            {
                Name = "Test Event Name",
                Description = "Test Event Description",
                LayoutId = 1,
                StartDateTime = new DateTime(2029, 08, 13, 13, 00, 00),
                EndDateTime = new DateTime(2030, 12, 13, 22, 00, 00),
            };

            var layoutMockRepo = new Mock<IRepository<LayoutData>>();
            layoutMockRepo.Setup(repo => repo.GetById(eventTest.LayoutId)).Returns(GetByIdTests(eventTest.LayoutId));

            var seatMockRepo = new Mock<IRepository<Seat>>();
            var extendedSeatMockRepo = seatMockRepo.As<ISeatRepositoryExtension>();
            extendedSeatMockRepo.Setup(repo => repo.FilterSeatsByLayoutId(eventTest.LayoutId)).Returns(FilterSeatsByLayoutIdTests(eventTest.LayoutId));

            var mockRepo = new Mock<IEventDataRepositoryExtension>();
            mockRepo.Setup(repo => repo.FilterByVenueId(_layout.VenueId)).Returns(FilterByVenueIdTests(_layout.VenueId));

            var extendedMockRepo = mockRepo.As<IRepository<EventData>>();
            extendedMockRepo.Setup(repo => repo.Insert(eventTest)).Returns(CreateTests(eventTest));

            var eventService = new EventService(extendedMockRepo.Object,
                                                layoutMockRepo.Object,
                                                (IRepository<Seat>)extendedSeatMockRepo.Object);

            // Act
            var result = eventService.Insert(eventTest);

            // Assert
            Assert.AreEqual(result, _events.Last().Id);
        }

        [Test]
        public void IsValid_WhenInsertEventFailed_ShouldThrowExceptionNotValidDate()
        {
            // Arrange
            var eventTest = new EventData
            {
                Name = "Test Event Name",
                Description = "Test Event Description",
                LayoutId = 2,
                StartDateTime = new DateTime(2025, 10, 13, 15, 00, 00),
                EndDateTime = new DateTime(2024, 10, 13, 19, 00, 00),
            };
            var mockRepo = new Mock<IRepository<EventData>>();
            var layoutMockRepo = new Mock<IRepository<LayoutData>>();
            var seatMockRepo = new Mock<IRepository<Seat>>();
            var eventService = new EventService(mockRepo.Object, layoutMockRepo.Object, seatMockRepo.Object);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => eventService.Insert(eventTest));

            // Assert
            Assert.AreEqual("Not Valid dates", ex.Message);
        }

        [Test]
        public void IsValid_WhenInsertEventFailed_ShouldThrowExceptionEventHasNoSeats()
        {
            // Arrange
            var eventTest = new EventData
            {
                Name = "Test Event Name",
                Description = "Test Event Description",
                LayoutId = 4,
                StartDateTime = new DateTime(2025, 10, 13, 13, 00, 00),
                EndDateTime = new DateTime(2028, 10, 13, 22, 00, 00),
            };

            var layoutMockRepo = new Mock<IRepository<LayoutData>>();
            layoutMockRepo.Setup(repo => repo.GetById(eventTest.LayoutId)).Returns(GetByIdTests(eventTest.LayoutId));

            var mockRepo = new Mock<IRepository<EventData>>();
            var extendedMockRepo = mockRepo.As<IEventDataRepositoryExtension>();
            extendedMockRepo.Setup(repo => repo.FilterByVenueId(_layout.VenueId)).Returns(FilterByVenueIdTests(_layout.VenueId));

            var seatMockRepo = new Mock<IRepository<Seat>>();
            var extendedSeatMockRepo = seatMockRepo.As<ISeatRepositoryExtension>();
            extendedSeatMockRepo.Setup(repo => repo.FilterSeatsByLayoutId(eventTest.LayoutId)).Returns(FilterSeatsByLayoutIdTests(eventTest.LayoutId));

            var eventService = new EventService((IRepository<EventData>)extendedMockRepo.Object,
                                                layoutMockRepo.Object,
                                                (IRepository<Seat>)extendedSeatMockRepo.Object);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => eventService.Insert(eventTest));

            // Assert
            Assert.AreEqual("Event must have seats", ex.Message);
        }

        [Test]
        public void IsValid_WhenInsertEventFailed_ShouldThrowExceptionEventsAtSameTime()
        {
            // Arrange
            var eventTest = new EventData
            {
                Name = "Test Event Name",
                Description = "Test Event Description",
                LayoutId = 2,
                StartDateTime = new DateTime(2021, 10, 13, 13, 00, 00),
                EndDateTime = new DateTime(2021, 10, 13, 22, 00, 00),
            };
            var layoutMockRepo = new Mock<IRepository<LayoutData>>();
            layoutMockRepo.Setup(repo => repo.GetById(eventTest.LayoutId)).Returns(GetByIdTests(eventTest.LayoutId));

            var mockRepo = new Mock<IRepository<EventData>>();
            var extendedMockRepo = mockRepo.As<IEventDataRepositoryExtension>();
            extendedMockRepo.Setup(repo => repo.FilterByVenueId(_layout.VenueId)).Returns(FilterByVenueIdTests(_layout.VenueId));

            var seatMockRepo = new Mock<IRepository<Seat>>();
            var eventService = new EventService((IRepository<EventData>)extendedMockRepo.Object,
                                                layoutMockRepo.Object,
                                                seatMockRepo.Object);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => eventService.Insert(eventTest));

            // Assert
            Assert.AreEqual("Event cannot occur for the same venue in the same time", ex.Message);
        }

        private static int CreateTests(EventData entity)
        {
            entity.Id = _events.Count;
            _events.Add(entity);
            return entity.Id;
        }

        private static List<Seat> FilterSeatsByLayoutIdTests(int id)
        {
            var seats = from seat in _seats
                        join area in _areas on seat.AreaId equals area.Id
                        where area.LayoutId == id
                        select seat;
            return seats.ToList();
        }

        private static LayoutData GetByIdTests(int id)
        {
            return _layout = _layouts.First(x => x.Id == id);
        }

        private static List<EventData> FilterByVenueIdTests(int id)
        {
            var eves = from ev in _events
                       join lay in _layouts on ev.LayoutId equals lay.Id
                       where lay.VenueId == id
                       select ev;
            return eves.ToList();
        }
    }
}
