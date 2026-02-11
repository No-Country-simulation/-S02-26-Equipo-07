namespace WebApi.DTOs.User;

public class UserDto
{
    public long Id { get; set; }
    public string Username { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}
