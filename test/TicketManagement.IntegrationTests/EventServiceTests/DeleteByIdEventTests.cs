using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using Microsoft.SqlServer.Dac;
using NUnit.Framework;
using TicketManagement.DataAccess.Repository.Sql;
using TicketManagement.EventAPI.Services;

namespace TicketManagement.IntegrationTests.EventServiceTests
{
    [TestFixture]
    public class DeleteByIdEventTests
    {
        private static readonly ConnectionSettingsTest _settingsTest = new ConnectionSettingsTest();
        private static readonly DbConnection _connection = _settingsTest.Connection;
        private static readonly EventDataRepository _eventRepository = new EventDataRepository(_connection);
        private static readonly LayoutRepository _layoutRepository = new LayoutRepository(_connection);
        private static readonly SeatRepository _seatRepository = new SeatRepository(_connection);
        private static readonly EventService _eventService = new EventService(_eventRepository, _layoutRepository, _seatRepository);
        private static int _eventId=1;

        [Test]
        public static void IsValid_WhenDeleteByIdEventSuccess_ShouldReturn0AffectedRows()
        {
            // Arrange
            var testId = _eventId + 15;

            // Act
            int rows = _eventService.DeleteById(testId);

            // Assert
            Assert.AreEqual(0, rows);
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
        public void IsValid_WhenDeleteByIdEventSuccess_ShouldReturn1AffectedRows()
        {
            // Act
            int rows = _eventService.DeleteById(_eventId);

            // Assert
            Assert.AreEqual(1, rows);
        }
    }
}
