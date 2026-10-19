using HW_17_10_to_24_10_2026.DTO;
using HW_17_10_to_24_10_2026.DTO.Pagination;

namespace HW_17_10_to_24_10_2026.Services;

public interface IUserService
{
    Task<List<UserDto>> GetAllUsersAsync();
    Task<PageResponse<UserDto>> GetPagedAsync(UserQueryParams queryParams);
    Task<UserDto> GetUserByIdAsync(Guid id);
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
    Task<bool> DeleteUserAsync(Guid id);
}
