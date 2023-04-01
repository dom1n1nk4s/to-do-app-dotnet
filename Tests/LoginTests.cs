using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using to_do_app_dotnet.Controllers;
using to_do_app_dotnet.DTOs.User;
using to_do_app_dotnet.Models;

namespace Tests;

public class LoginTests
{
    [Fact]
    public async void WhenCorrectLoginInfoPassed_ReturnAuthToken()
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
        var signInResult = SignInResult.Success;
        var jwtKey = "sample-key-aaaaa";

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

        var signInManagerMock = new Mock<SignInManager<User>>(
            userManagerMock.Object,
            Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<User>>(),
            null,
            null,
            null,
            null);
        var configurationMock = new Mock<IConfiguration>();
        userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
        .Returns(Task.FromResult<User?>(userModel));
        signInManagerMock.Setup(x => x.PasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), false, false))
        .Returns(Task.FromResult(signInResult));
        configurationMock.Setup(x => x[It.IsAny<string>()]).Returns(jwtKey);

        var controller = new UserController(userManagerMock.Object, signInManagerMock.Object, configurationMock.Object);

        //Act
        var resultTask = () => controller.Login(userDTO);

        //Assert
        await resultTask.Should().NotThrowAsync();
        var result = await resultTask();
        result.Should().BeOfType<Microsoft.AspNetCore.Mvc.OkObjectResult>();
        ((Microsoft.AspNetCore.Mvc.OkObjectResult)result).Value.Should().BeOfType<string>();
    }

    [Fact]
    public async void WhenIncorrectLoginInfoPassed_ReturnBadRequest()
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
        var signInResult = SignInResult.Failed;

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

        var signInManagerMock = new Mock<SignInManager<User>>(
            userManagerMock.Object,
            Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<User>>(),
            null,
            null,
            null,
            null);
        var configurationMock = new Mock<IConfiguration>();
        userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
        .Returns(Task.FromResult<User?>(userModel));
        signInManagerMock.Setup(x => x.PasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), false, false))
        .Returns(Task.FromResult(signInResult));

        var controller = new UserController(userManagerMock.Object, signInManagerMock.Object, configurationMock.Object);

        //Act
        var resultTask = () => controller.Login(userDTO);

        //Assert
        await resultTask.Should().NotThrowAsync();
        var result = await resultTask();
        result.Should().BeOfType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>();
    }
}