using AutoMapper;
using HW_17_10_to_24_10_2026.Data;
using HW_17_10_to_24_10_2026.DTO;
using HW_17_10_to_24_10_2026.DTO.Pagination;
using HW_17_10_to_24_10_2026.Models;
using Microsoft.EntityFrameworkCore;

namespace HW_17_10_to_24_10_2026.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _appdbContext;
    private readonly IMapper _mapper;

    public UserService(AppDbContext appDbContext, IMapper mapper)
    {
        _appdbContext = appDbContext;
        _mapper = mapper;
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        var user = _mapper.Map<User>(createUserDto);
        await _appdbContext.Users.AddAsync(user);
        await _appdbContext.SaveChangesAsync();
        return _mapper.Map<UserDto>(user);
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var user = await _appdbContext.Users.FindAsync(id);
        if (user == null) return false;

        user.IsDeleted = true;
        await _appdbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users = await _appdbContext.Users.ToListAsync();
        return users.Select(u => _mapper.Map<UserDto>(u)).ToList();
    }

    public async Task<PageResponse<UserDto>> GetPagedAsync(UserQueryParams queryParams)
    {
        var query = _appdbContext.Users.AsNoTracking().AsQueryable();

        // Searching
        if (!string.IsNullOrWhiteSpace(queryParams.Search))
        {
            var search = queryParams.Search.Trim().ToLower();
            query = query.Where(u =>
                u.FirstName.ToLower().Contains(search) ||
                u.LastName.ToLower().Contains(search) ||
                u.Email.ToLower().Contains(search));
        }

        // Filtering
        if (!string.IsNullOrWhiteSpace(queryParams.Role))
        {
            var role = queryParams.Role.Trim().ToLower();
            query = query.Where(u => u.Role.ToLower() == role);
        }

        // Sorting
        query = queryParams.SortBy?.ToLower() switch
        {
            "firstname" => queryParams.IsSortDescending ? query.OrderByDescending(u => u.FirstName) : query.OrderBy(u => u.FirstName),
            "lastname" => queryParams.IsSortDescending ? query.OrderByDescending(u => u.LastName) : query.OrderBy(u => u.LastName),
            "email" => queryParams.IsSortDescending ? query.OrderByDescending(u => u.Email) : query.OrderBy(u => u.Email),
            "role" => queryParams.IsSortDescending ? query.OrderByDescending(u => u.Role) : query.OrderBy(u => u.Role),
            "createdat" => queryParams.IsSortDescending ? query.OrderByDescending(u => u.CreatedAt) : query.OrderBy(u => u.CreatedAt),
            _ => queryParams.IsSortDescending ? query.OrderByDescending(u => u.CreatedAt) : query.OrderBy(u => u.CreatedAt)
        };

        // Pagination
        var totalCount = await query.CountAsync();
        var items = await query.Skip((queryParams.PageNumber - 1) * queryParams.PageSize)
            .Take(queryParams.PageSize)
            .ToListAsync();

        var mappedItems = items.Select(u => _mapper.Map<UserDto>(u)).ToList();

        return new PageResponse<UserDto>(mappedItems, totalCount, queryParams.PageNumber, queryParams.PageSize);
    }

    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        var user = await _appdbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) throw new InvalidOperationException("User not found");
        return _mapper.Map<UserDto>(user);
    }
}
