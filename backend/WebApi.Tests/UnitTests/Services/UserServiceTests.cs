using FluentAssertions;
using Moq;
using WebApi.DTOs.User;
using WebApi.Models;
using WebApi.Repositories;
using WebApi.Services;
using Xunit;

namespace WebApi.Tests.UnitTests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object);
    }

    #region GetAllUsersAsync

    [Fact]
    public async Task GetAllUsersAsync_ConUsuariosExistentes_RetornaLista()
    {
        var users = new List<User>
        {
            new User { Id = 1, Username = "user1", Role = "admin", Status = "habilitado", CreatedAt = DateTime.Now, ModifiedAt = DateTime.Now },
            new User { Id = 2, Username = "user2", Role = "user", Status = "habilitado", CreatedAt = DateTime.Now, ModifiedAt = DateTime.Now }
        };

        _userRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(users);

        var result = await _userService.GetAllUsersAsync();

        result.Should().NotBeNull();
        result.Count().Should().Be(2);
    }

    [Fact]
    public async Task GetAllUsersAsync_SinUsuarios_RetornaListaVacia()
    {
        var users = new List<User>();

        _userRepositoryMock
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(users);

        var result = await _userService.GetAllUsersAsync();

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    #endregion

    #region GetUserByIdAsync

    [Fact]
    public async Task GetUserByIdAsync_UsuarioExistente_RetornaUsuario()
    {
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Role = "admin",
            Status = "habilitado",
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        };

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(user);

        var result = await _userService.GetUserByIdAsync(1);

        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Username.Should().Be("testuser");
    }

    [Fact]
    public async Task GetUserByIdAsync_UsuarioNoExiste_RetornaNull()
    {
        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(999))
            .ReturnsAsync((User?)null);

        var result = await _userService.GetUserByIdAsync(999);

        result.Should().BeNull();
    }

    #endregion

    #region GetUserByUsernameAsync

    [Fact]
    public async Task GetUserByUsernameAsync_UsuarioExistente_RetornaUsuario()
    {
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Role = "admin",
            Status = "habilitado",
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        };

        _userRepositoryMock
            .Setup(x => x.GetByUsernameAsync("testuser"))
            .ReturnsAsync(user);

        var result = await _userService.GetUserByUsernameAsync("testuser");

        result.Should().NotBeNull();
        result!.Username.Should().Be("testuser");
    }

    [Fact]
    public async Task GetUserByUsernameAsync_UsuarioNoExiste_RetornaNull()
    {
        _userRepositoryMock
            .Setup(x => x.GetByUsernameAsync("nonexistent"))
            .ReturnsAsync((User?)null);

        var result = await _userService.GetUserByUsernameAsync("nonexistent");

        result.Should().BeNull();
    }

    #endregion

    #region UpdateUserAsync

    [Fact]
    public async Task UpdateUserAsync_UsuarioExistente_RetornaUsuarioActualizado()
    {
        var existingUser = new User
        {
            Id = 1,
            Username = "oldname",
            Role = "user",
            Status = "habilitado",
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        };

        var updatedUser = new User
        {
            Id = 1,
            Username = "newname",
            Role = "admin",
            Status = "habilitado"
        };

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(existingUser);

        _userRepositoryMock
            .Setup(x => x.UsernameExistsAsync("newname"))
            .ReturnsAsync(false);

        _userRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        var result = await _userService.UpdateUserAsync(1, updatedUser);

        result.Should().NotBeNull();
        result.Username.Should().Be("newname");
        result.Role.Should().Be("admin");
    }

    [Fact]
    public async Task UpdateUserAsync_UsuarioNoExiste_LanzaExcepcion()
    {
        var user = new User
        {
            Id = 999,
            Username = "testuser",
            Role = "user",
            Status = "habilitado"
        };

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(999))
            .ReturnsAsync((User?)null);

        var act = () => _userService.UpdateUserAsync(999, user);
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("User with ID 999 not found");
    }

    [Fact]
    public async Task UpdateUserAsync_UsernameVacio_LanzaExcepcion()
    {
        var existingUser = new User
        {
            Id = 1,
            Username = "testuser",
            Role = "user",
            Status = "habilitado",
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        };

        var userWithEmptyUsername = new User
        {
            Id = 1,
            Username = "",
            Role = "user",
            Status = "habilitado"
        };

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(existingUser);

        var act = () => _userService.UpdateUserAsync(1, userWithEmptyUsername);
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Username cannot be empty");
    }

    [Fact]
    public async Task UpdateUserAsync_UsernameDuplicado_LanzaExcepcion()
    {
        var existingUser = new User
        {
            Id = 1,
            Username = "user1",
            Role = "user",
            Status = "habilitado",
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        };

        var updatedUser = new User
        {
            Id = 1,
            Username = "existinguser",
            Role = "user",
            Status = "habilitado"
        };

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(existingUser);

        _userRepositoryMock
            .Setup(x => x.UsernameExistsAsync("existinguser"))
            .ReturnsAsync(true);

        var act = () => _userService.UpdateUserAsync(1, updatedUser);
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Username already exists");
    }

    #endregion

    #region DeleteUserAsync

    [Fact]
    public async Task DeleteUserAsync_UsuarioExistente_NoLanzaExcepcion()
    {
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Role = "user",
            Status = "habilitado",
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        };

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(x => x.DeleteAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        var act = () => _userService.DeleteUserAsync(1);
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task DeleteUserAsync_UsuarioNoExiste_LanzaExcepcion()
    {
        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(999))
            .ReturnsAsync((User?)null);

        var act = () => _userService.DeleteUserAsync(999);
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("User with ID 999 not found");
    }

    #endregion
}
