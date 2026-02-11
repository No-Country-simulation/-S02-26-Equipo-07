using WebApi.DTOs.User;
using WebApi.Models;

namespace WebApi.Services;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(long id);
    Task<UserDto?> GetUserByUsernameAsync(string username);
    Task<UserDto> UpdateUserAsync(long id, User user);
    Task DeleteUserAsync(long id);
}
