using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using Microsoft.SqlServer.Dac;
using NUnit.Framework;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Repository.Sql;
using TicketManagement.EventAPI.Services;

namespace TicketManagement.IntegrationTests.EventServiceTests
{
    [TestFixture]
    public class InsertEventTests
    {
        private static readonly ConnectionSettingsTest _settingsTest = new ConnectionSettingsTest();
        private static readonly DbConnection _connection = _settingsTest.Connection;
        private static readonly EventDataRepository _eventRepository = new EventDataRepository(_connection);
        private static readonly LayoutRepository _layoutRepository = new LayoutRepository(_connection);
        private static readonly SeatRepository _seatRepository = new SeatRepository(_connection);
        private static readonly EventService _eventService = new EventService(_eventRepository, _layoutRepository, _seatRepository);

        [Test]
        public static void IsValid_WhenInsertEventFailed_ShouldThrowExceptionAboutEventInSameTime()
        {
            // Arrange
            var eventTest = new EventData
            {
                Name = "14th Event Name",
                Description = "First Event Description",
                LayoutId = 1,
                Category = Category.Cinema,
                StartDateTime = new DateTime(2021, 08, 13, 15, 00, 00),
                EndDateTime = new DateTime(2021, 08, 16, 19, 00, 00),
            };

            // Act
            var ex = Assert.Throws<ArgumentException>(() => _eventService.Insert(eventTest));

            // Assert
            Assert.AreEqual("Event cannot occur for the same venue in the same time", ex.Message);
        }

        [Test]
        public static void IsValid_WhenInsertEventFailed_ShouldThrowExceptionAboutNotValidDates()
        {
            // Arrange
            var eventTest = new EventData
            {
                Name = "14th Event Name",
                Description = "First Event Description",
                LayoutId = 1,
                StartDateTime = new DateTime(2019, 12, 29, 15, 00, 00),
                EndDateTime = new DateTime(2021, 01, 01, 19, 00, 00),
            };

            // Act
            var ex = Assert.Throws<ArgumentException>(() => _eventService.Insert(eventTest));

            // Assert
            Assert.AreEqual("Not Valid dates", ex.Message);
        }

        [Test]
        public static void IsValid_WhenInsertEventFailed_ShouldThrowExceptionAboutEventWithoutSeats()
        {
            // Arrange
            var eventTest = new EventData
            {
                Name = "14th Event Name",
                Description = "First Event Description",
                LayoutId = 3,
                StartDateTime = new DateTime(2028, 12, 29, 15, 00, 00),
                EndDateTime = new DateTime(2029, 01, 01, 19, 00, 00),
            };

            // Act
            var ex = Assert.Throws<ArgumentException>(() => _eventService.Insert(eventTest));

            // Assert
            Assert.AreEqual("Event must have seats", ex.Message);
        }

        [SetUp]
        public void SetUp()
        {
            var dacpac = DacPackage.Load(@"..\..\..\TicketManagement.DatabaseTest.dacpac");
            var dacpacService = new DacServices(@"Data Source =.\SQLEXPRESS;Initial Catalog = TicketManagement.DatabaseTest; Integrated Security = True");
            var dacOptions = new DacDeployOptions { CreateNewDatabase = true };
            dacpacService.Deploy(dacpac, "TicketManagement.DatabaseTest", true, dacOptions);
            using (var connection = _settingsTest.Connection)
            {
                string scriptPost = File.ReadAllText(@"..\..\..\Script.PostDeployment.sql");


                var command = new SqlCommand(scriptPost, (SqlConnection)connection);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        [Test]
        public void IsValid_WhenInsertEventSuccess_ShouldReturnId()
        {
            // Arrange
            var eventTest = new EventData
            {
                Name = "14th Event Name",
                Description = "14th Event Description",
                LayoutId = 1,
                StartDateTime = new DateTime(2023, 10, 18, 15, 00, 00),
                EndDateTime = new DateTime(2023, 10, 19, 19, 00, 00),
                Category = Category.Concert,
            };

            // Act
            var id = _eventService.Insert(eventTest);

            // Assert
            Assert.IsNotNull(id);
        }
    }
}
