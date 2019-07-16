using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrackerLukaN.Controllers;
using TimeTrackerLukaN.Data;
using TimeTrackerLukaN.Domain;
using TimeTrackerLukaN.Models;
using Xunit;

namespace TimeTrackerLukaN.Tests.UnitTests
{
    public class UsersControllerTests
    {
        private readonly UsersController _controller;
        public UsersControllerTests()
        {
            //opcije za in memory bazu
            var options = new DbContextOptionsBuilder<TimeTrackerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var dbContext = new TimeTrackerDbContext(options);

            dbContext.Users.Add(new User { Id = 1, Name = "Test User 1", HourRate = 15 });
            dbContext.Users.Add(new User { Id = 2, Name = "Test User 2", HourRate = 25 });
            dbContext.Users.Add(new User { Id = 3, Name = "Test User 3", HourRate = 35 });
            dbContext.SaveChanges();

            var logger = new FakeLogger<UsersController>();

            _controller = new UsersController(dbContext, logger);
        }
        [Fact(Skip = "Doesn't work with EF Core 3 Preview 6")]
        public void GetById_IdDoesNotExist_ReturnsNotFound()
        {
            // Act
            var result = _controller.GetById(0);
            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetById_IdExists_ReturnsExpectedResult()
        {
            const string expectedName = "Test User 1";
            // Act
            var result = await _controller.GetById(1);
            // Assert
            Assert.IsType<ActionResult<UserModel>>(result);
            Assert.NotNull(result.Value);
            Assert.Equal(expectedName, result.Value.Name);
        }

        [Fact]
        public async Task GetPage_FirstPage_ReturnsExpectedResult()
        {
            const int expectedCount = 3;
            const int totalCount = 3;
            // Act
            var result = await _controller.GetPage(1, 10);
            // Assert
            Assert.IsType<ActionResult<PagedList<UserModel>>>(result);
            Assert.NotNull(result.Value);
            Assert.Equal(expectedCount, result.Value.Items.Count());
            Assert.Equal(totalCount, result.Value.TotalCount);
        }

        [Fact]
        public async Task GetPage_SecondPage_ReturnsExpectedResult()
        {            
            const int totalCount = 3;
            // Act
            var result = await _controller.GetPage(2, 10);
            // Assert
            Assert.IsType<ActionResult<PagedList<UserModel>>>(result);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Items);
            Assert.Equal(totalCount, result.Value.TotalCount);
        }

        [Fact]
        public async Task Delete_IdExists_ReturnsOkResult()
        {
            var result = await _controller.Delete(1);

            Assert.IsType<OkResult>(result);
        }
    }
}
