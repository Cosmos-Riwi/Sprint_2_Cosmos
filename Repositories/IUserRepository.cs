using Sprint2.Models;

namespace Sprint2.Repositories;

public interface IUserRepository
{
    // READ operations
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<List<User>> GetByCityAsync(string city);
    Task<List<User>> GetByCountryAsync(string country);
    Task<List<User>> GetByMinimumAgeAsync(int minimumAge);
    Task<List<User>> GetByGenderAsync(string gender);
    Task<List<NameEmail>> GetNamesAndEmailsAsync();
    Task<int> CountAllAsync();
    Task<List<(string City, int Count)>> CountByCityAsync();
    Task<List<(string Country, int Count)>> CountByCountryAsync();
    Task<List<User>> WithoutPhoneAsync();
    Task<List<User>> WithoutAddressAsync();
    Task<List<User>> LatestRegisteredAsync(int limit = 10);
    Task<List<User>> OrderedByLastNameAsync();
    Task<List<User>> GetPagedAsync(int page, int pageSize);

    // WRITE operations
    Task<(bool Ok, string Message, User? Created)> CreateAsync(User user);
    Task<(bool Ok, string Message)> UpdateAsync(User user);
    Task<(bool Ok, string Message)> UpdatePasswordAsync(int userId, string newPassword);
    Task<(bool Ok, string Message)> DeleteByIdAsync(int id);
    Task<(bool Ok, string Message)> DeleteByEmailAsync(string email);
    
    // VALIDATION operations
    Task<bool> ExistsByUsernameAsync(string username, int? excludeId = null);
    Task<bool> ExistsByEmailAsync(string email, int? excludeId = null);
}