using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AspNetAssignments.Data;
using AspNetAssignments.DTOs.User;
using AspNetAssignments.Models.User;
using AspNetAssignments.Services.Abstract;

namespace AspNetAssignments.Services;

/// <summary>
/// Сервіс для управління даними користувачів та збереження аватарок
/// </summary>
public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private const string AvatarSubFolder = "avatars";

    public UserService(AppDbContext context, IMapper mapper, IFileService fileService)
    {
        _context = context;
        _mapper = mapper;
        _fileService = fileService;
    }

    /// <summary>
    /// Отримує перелік усіх не видалених користувачів
    /// </summary>
    public async Task<List<UserDTO>> GetAllUsersAsync()
    {
        var users = await _context.Users
            .AsNoTracking()
            .Where(u => !u.IsDeleted)
            .ToListAsync();

        return _mapper.Map<List<UserDTO>>(users);
    }

    /// <summary>
    /// Знаходить користувача за його ідентифікатором
    /// </summary>
    public async Task<UserDTO?> GetUserByIdAsync(Guid id)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

        return user == null ? null : _mapper.Map<UserDTO>(user);
    }

    /// <summary>
    /// Створює нового користувача та зберігає його аватар за наявності
    /// </summary>
    public async Task<UserDTO> CreateUserAsync(CreateUserDTO createUserDto)
    {
        var user = _mapper.Map<User>(createUserDto);

        // Збереження аватара на сервері
        if (createUserDto.Avatar != null && createUserDto.Avatar.Length > 0)
        {
            var fileResult = await _fileService.SaveFileAsync(createUserDto.Avatar, AvatarSubFolder);
            user.AvatarURL = fileResult.RelativePath;
        }

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return _mapper.Map<UserDTO>(user);
    }

    /// <summary>
    /// Оновлює інформацію профілю користувача та замінює аватар
    /// </summary>
    public async Task<UserDTO?> UpdateUserAsync(Guid id, UpdateUserDTO updateUserDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        if (user == null) return null;

        user.Name = updateUserDto.Name;
        user.LastName = updateUserDto.LastName;
        user.Email = updateUserDto.Email;
        user.Role = updateUserDto.Role;
        user.Age = updateUserDto.Age;

        if (updateUserDto.Avatar != null && updateUserDto.Avatar.Length > 0)
        {
            if (!string.IsNullOrEmpty(user.AvatarURL))
            {
                _fileService.DeleteFile(user.AvatarURL);
            }
            var fileResult = await _fileService.SaveFileAsync(updateUserDto.Avatar, AvatarSubFolder);
            user.AvatarURL = fileResult.RelativePath;
        }

        await _context.SaveChangesAsync();
        return _mapper.Map<UserDTO>(user);
    }

    /// <summary>
    /// Виконує м'яке видалення користувача за ідентифікатором
    /// </summary>
    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        if (user == null) return false;

        user.IsDeleted = true;
        await _context.SaveChangesAsync();
        return true;
    }
}
