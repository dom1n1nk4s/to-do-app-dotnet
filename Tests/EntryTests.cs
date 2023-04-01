using System.Security.Claims;
using System.Security.Principal;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq.EntityFrameworkCore;
using to_do_app_dotnet.Controllers;
using to_do_app_dotnet.DTOs.Entry;
using to_do_app_dotnet.Models;

namespace Tests
{
    public class EntryTests
    {
        [Fact]
        public async Task GetEntries_WhenUserLoggedIn_ReturnListOfUsersEntries()
        {
            //Arrange
            var userName = "username";
            var userIdentityMock = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userName),
                }, "mock"));

            var userEntry = new Entry
            {
                Id = Guid.NewGuid(),
                Title = "title",
                UserName = userName,
            };
            var entries = new List<Entry>()
            {
                userEntry,
                new Entry
                {
                    Id = Guid.NewGuid(),
                    Title = "title",
                    UserName = "notTheUsersUserName",
                }
            };
            var identityContextMock = new Mock<IIdentityContext>();
            identityContextMock.SetupGet(x => x.Entries).ReturnsDbSet(entries);

            var controller = new EntryController(identityContextMock.Object);
            controller.ControllerContext.HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext() { User = userIdentityMock };

            //Act
            var resultTask = () => controller.GetEntries();

            await resultTask.Should().NotThrowAsync();

            var result = await resultTask();
            //Assert
            result.Should().BeOfType<Microsoft.AspNetCore.Mvc.OkObjectResult>();
            var resultValue = ((Microsoft.AspNetCore.Mvc.OkObjectResult)result).Value;

            resultValue.Should().BeOfType<List<Entry>>();
            ((List<Entry>)resultValue!).Should().BeEquivalentTo(new List<Entry>() { userEntry });
        }

        [Fact]
        public async Task CreateEntry_WhenEntryDataIsValid_AddAndReturnOk()
        {
            //Arrange
            var userName = "username";
            var userIdentityMock = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userName),
                }, "mock"));

            var entryDTO = new EntryDTO
            {
                Id = Guid.NewGuid(),
                Title = "title",
            };
            var entries = new List<Entry>();

            var identityContextMock = new Mock<IIdentityContext>();
            identityContextMock.SetupGet(x => x.Entries).ReturnsDbSet(entries);

            var controller = new EntryController(identityContextMock.Object);
            controller.ControllerContext.HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext() { User = userIdentityMock };
            //Act
            var resultTask = () => controller.CreateEntry(entryDTO);

            await resultTask.Should().NotThrowAsync();

            var result = await resultTask();
            //Assert
            result.Should().BeOfType<Microsoft.AspNetCore.Mvc.OkObjectResult>();
            var resultValue = ((Microsoft.AspNetCore.Mvc.OkObjectResult)result).Value;
            identityContextMock.Verify(x => x.SaveChangesAsync(default), Times.AtLeastOnce());
            resultValue.Should().BeOfType<Entry>();
        }

        [Fact]
        public async Task DeleteEntry_WhenEntryExists_ReturnOk()
        {
            //Arrange
            var userName = "username";
            var userIdentityMock = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userName),
                }, "mock"));

            var userEntry = new Entry
            {
                Id = Guid.NewGuid(),
                Title = "title",
                UserName = userName,
            };
            var entries = new List<Entry>()
            {
                userEntry,
                new Entry
                {
                    Id = Guid.NewGuid(),
                    Title = "title",
                    UserName = "notTheUsersUserName",
                }
            };
            var identityContextMock = new Mock<IIdentityContext>();
            identityContextMock.SetupGet(x => x.Entries).ReturnsDbSet(entries);

            var controller = new EntryController(identityContextMock.Object);
            controller.ControllerContext.HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext() { User = userIdentityMock };

            //Act
            var resultTask = () => controller.DeleteEntry(userEntry.Id);

            await resultTask.Should().NotThrowAsync();

            var result = await resultTask();
            //Assert
            result.Should().BeOfType<Microsoft.AspNetCore.Mvc.OkResult>();
            identityContextMock.Verify(x => x.SaveChangesAsync(default), Times.AtLeastOnce());
        }

        [Fact]
        public async Task DeleteEntry_WhenEntryDoesNotExist_ReturnBadRequest()
        {
            //Arrange
            var userName = "username";
            var userIdentityMock = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userName),
                }, "mock"));

            var notTheUsersEntry = new Entry
            {
                Id = Guid.NewGuid(),
                Title = "title",
                UserName = "notTheUsersUserName",
            };
            var entries = new List<Entry>()
            {
                notTheUsersEntry,
                new Entry
                {
                    Id = Guid.NewGuid(),
                    Title = "title",
                    UserName = "notTheUsersUserName",
                }
            };
            var identityContextMock = new Mock<IIdentityContext>();
            identityContextMock.SetupGet(x => x.Entries).ReturnsDbSet(entries);

            var controller = new EntryController(identityContextMock.Object);
            controller.ControllerContext.HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext() { User = userIdentityMock };

            //Act
            var resultTask = () => controller.DeleteEntry(notTheUsersEntry.Id);

            await resultTask.Should().NotThrowAsync();

            var result = await resultTask();
            //Assert
            result.Should().BeOfType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>();
            identityContextMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
        }
    }
}