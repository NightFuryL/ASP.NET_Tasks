using AspNetAssignments.DTOs.User;

namespace AspNetAssignments.Services.Abstract;

/// <summary>
/// Контракт сервісу для управління користувачами та профілями
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Отримує список усіх активних користувачів
    /// </summary>
    Task<List<UserDTO>> GetAllUsersAsync();

    /// <summary>
    /// Знаходить користувача за ідентифікатором
    /// </summary>
    Task<UserDTO?> GetUserByIdAsync(Guid id);

    /// <summary>
    /// Реєструє нового користувача із можливістю завантаження аватара
    /// </summary>
    Task<UserDTO> CreateUserAsync(CreateUserDTO createUserDto);

    /// <summary>
    /// Оновлює дані користувача та його аватар
    /// </summary>
    Task<UserDTO?> UpdateUserAsync(Guid id, UpdateUserDTO updateUserDto);

    /// <summary>
    /// Виконує м'яке видалення користувача
    /// </summary>
    Task<bool> DeleteUserAsync(Guid id);
}
