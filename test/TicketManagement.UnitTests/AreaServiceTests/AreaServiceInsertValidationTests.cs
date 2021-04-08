using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TicketManagement.DataAccess.Entities;
using TicketManagement.DataAccess.Interfaces;
using TicketManagement.DataAccess.Interfaces.RepositoryExtension;
using TicketManagement.EventAPI.Services;

namespace TicketManagement.UnitTests.AreaServiceTests
{
    [TestFixture]
    public class AreaServiceInsertValidationTests
    {
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

        [Test]
        public void IsValid_WhenInsertAreaSuccess_ShouldReturnIdOfArea()
        {
            // Arrange
            var areaTest = new Area
            {
                Description = "TestArea15",
                LayoutId = 1,
                CoordX = 0,
                CoordY = 1,
            };

            var mockRepository = new Mock<IAreaRepositoryExtension>();
            mockRepository.Setup(repo => repo.FilterByNameInLayout(areaTest)).Returns(FilterByNameInLayoutTests(areaTest));
            var extendedMockRepository = mockRepository.As<IRepository<Area>>();
            extendedMockRepository.Setup(repo => repo.Insert(areaTest)).Returns(CreateTests(areaTest));
            var areaService = new AreaService(extendedMockRepository.Object);

            // Act
            var result = areaService.Insert(areaTest);

            // Assert
            Assert.AreEqual(result, _areas.Last().Id);
        }

        [Test]
        public void IsValid_WhenInsertAreaFailed_ShouldThrowException()
        {
            // Arrange
            var areaTest = new Area
            {
                Description = "TestArea3",
                LayoutId = 1,
                CoordX = 0,
                CoordY = 1,
            };

            var mockRepository = new Mock<IAreaRepositoryExtension>();
            mockRepository.Setup(repo => repo.FilterByNameInLayout(areaTest)).Returns(FilterByNameInLayoutTests(areaTest));
            var extendedMockRepository = mockRepository.As<IRepository<Area>>();
            extendedMockRepository.Setup(repo => repo.Insert(areaTest)).Returns(CreateTests(areaTest));
            var areaService = new AreaService(extendedMockRepository.Object);

            // Act
            var ex = Assert.Throws<ArgumentException>(() => areaService.Insert(areaTest));

            // Assert
            Assert.AreEqual("Description not unique", ex.Message);
        }

        private static List<Area> FilterByNameInLayoutTests(Area entity)
        {
            return _areas.Where(x => (x.Description == entity.Description) && (x.LayoutId == entity.LayoutId)).ToList();
        }

        private static int CreateTests(Area entity)
        {
            entity.Id = _areas.Count;
            _areas.Add(entity);
            return _areas.Last().Id;
        }
    }
}