using WebApi.DTOs.User;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(MapToDto);
    }

    public async Task<UserDto?> GetUserByIdAsync(long id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user == null ? null : MapToDto(user);
    }

    public async Task<UserDto?> GetUserByUsernameAsync(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        return user == null ? null : MapToDto(user);
    }

    public async Task<UserDto> UpdateUserAsync(long id, User user)
    {
        var existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser == null)
            throw new KeyNotFoundException($"User with ID {id} not found");

        existingUser.Username = user.Username;
        existingUser.Role = user.Role;
        existingUser.Status = user.Status;

        await _userRepository.UpdateAsync(existingUser);
        return MapToDto(existingUser);
    }

    public async Task DeleteUserAsync(long id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {id} not found");

        await _userRepository.DeleteAsync(user);
    }

    private static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role,
            Status = user.Status,
            CreatedAt = user.CreatedAt,
            ModifiedAt = user.ModifiedAt
        };
    }
}
