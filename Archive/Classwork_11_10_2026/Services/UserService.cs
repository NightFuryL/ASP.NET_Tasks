using AutoMapper;
using Classwork_11_10_2026.Data;
using Classwork_11_10_2026.DTO;
using Classwork_11_10_2026.Models;
using Microsoft.EntityFrameworkCore;

namespace Classwork_11_10_2026.Services;

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

    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        var user = await _appdbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) throw new InvalidOperationException("User not found");
        return _mapper.Map<UserDto>(user);
    }
}
