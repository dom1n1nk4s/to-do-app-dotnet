using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using to_do_app_dotnet.Controllers;
using to_do_app_dotnet.DTOs.User;
using to_do_app_dotnet.Models;

namespace Tests
{
    public class RegistrationTests
    {
        [Fact]
        public async void WhenValidRegistrationInfoPassed_ReturnOk()
        {
            //Arrange
            var userName = "qwerty";
            var userDTO = new UserDTO
            {
                Name = userName,
                Password = "azerty",
            };
            var userModel = new User
            {
                UserName = userName,
            };

            var userManagerMock = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(),
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null);

            userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(userModel));

            var controller = new UserController(userManagerMock.Object, null!, null!);

            //Act
            var resultTask = () => controller.Register(userDTO);

            //Assert
            await resultTask.Should().NotThrowAsync();
            var result = await resultTask();
            result.Should().BeOfType<Microsoft.AspNetCore.Mvc.OkObjectResult>();
        }

        [Fact]
        public async void WhenInvalidRegistrationInfoPassed_ReturnBadRequest()
        {
            //Arrange
            var userName = "qwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwertyqwerty";

            var userDTO = new UserDTO
            {
                Name = userName,
                Password = "azerty",
            };
            var userModel = new User
            {
                UserName = userName,
            };

            var userManagerMock = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(),
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null);
            userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
            .Returns(Task.FromResult<User?>(userModel));

            var controller = new UserController(userManagerMock.Object, null!, null!);
            controller.ModelState.AddModelError(string.Empty, string.Empty);

            //Act
            var resultTask = () => controller.Register(userDTO);

            //Assert
            await resultTask.Should().NotThrowAsync();
            var result = await resultTask();
            result.Should().BeOfType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>();
        }
    }
}