using Classwork_11_10_2026.DTO;

namespace Classwork_11_10_2026.Services;
public interface IUserService
{
    Task<List<UserDto>> GetAllUsersAsync();
    Task<UserDto> GetUserByIdAsync(Guid id);
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
    Task<bool> DeleteUserAsync(Guid id);
}
