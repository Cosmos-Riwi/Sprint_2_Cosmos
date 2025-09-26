using Sprint2.Models;
using Sprint2.Repositories;

namespace Sprint2.Controllers;

public class UserController
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // 🔹 READ (todos)
    public async Task<List<User>> IndexAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    // 2. Ver detalle por Id
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    // 3. Ver detalle por Email
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _userRepository.GetByEmailAsync(email);
    }

    // 4. Listar por ciudad
    public async Task<List<User>> GetByCityAsync(string city)
    {
        return await _userRepository.GetByCityAsync(city);
    }

    // 5. Listar por país
    public async Task<List<User>> GetByCountryAsync(string country)
    {
        return await _userRepository.GetByCountryAsync(country);
    }

    // 6. Mayores de edad específica
    public async Task<List<User>> GetByMinimumAgeAsync(int minimumAge)
    {
        return await _userRepository.GetByMinimumAgeAsync(minimumAge);
    }

    // 7. Por género
    public async Task<List<User>> GetByGenderAsync(string gender)
    {
        return await _userRepository.GetByGenderAsync(gender);
    }

    // 8. Solo nombres y correos
    public async Task<List<NameEmail>> GetNamesAndEmailsAsync()
    {
        return await _userRepository.GetNamesAndEmailsAsync();
    }

    // 9. Contar total de usuarios
    public async Task<int> CountAllAsync()
    {
        return await _userRepository.CountAllAsync();
    }

    // 10. Contar por ciudad
    public async Task<List<(string City, int Count)>> CountByCityAsync()
    {
        return await _userRepository.CountByCityAsync();
    }

    // 11. Contar por país
    public async Task<List<(string Country, int Count)>> CountByCountryAsync()
    {
        return await _userRepository.CountByCountryAsync();
    }

    // 12. Sin teléfono
    public async Task<List<User>> WithoutPhoneAsync()
    {
        return await _userRepository.WithoutPhoneAsync();
    }

    // 13. Sin dirección
    public async Task<List<User>> WithoutAddressAsync()
    {
        return await _userRepository.WithoutAddressAsync();
    }

    // 14. Últimos registrados
    public async Task<List<User>> LatestRegisteredAsync(int limit = 10)
    {
        return await _userRepository.LatestRegisteredAsync(limit);
    }

    // 15. Ordenados por apellido
    public async Task<List<User>> OrderedByLastNameAsync()
    {
        return await _userRepository.OrderedByLastNameAsync();
    }

    // 16. Paginación
    public async Task<List<User>> GetPagedAsync(int page, int pageSize)
    {
        return await _userRepository.GetPagedAsync(page, pageSize);
    }

    // Inserción con unicidad
    public async Task<(bool Ok, string Message, User? Created)> CreateAsync(string firstName, string lastName, string username, string email)
    {
        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Username = username,
            Email = email
        };

        return await _userRepository.CreateAsync(user);
    }

    // Actualización general
    public async Task<(bool Ok, string Message)> UpdateAsync(User updated)
    {
        return await _userRepository.UpdateAsync(updated);
    }

    // Actualizar contraseña (con confirmación)
    public async Task<(bool Ok, string Message)> UpdatePasswordAsync(int userId, string newPassword, string confirmPassword)
    {
        if (newPassword != confirmPassword)
            return (false, "La confirmación de contraseña no coincide.");

        return await _userRepository.UpdatePasswordAsync(userId, newPassword);
    }

    // Eliminación (por Id)
    public async Task<(bool Ok, string Message)> DeleteByIdAsync(int id)
    {
        return await _userRepository.DeleteByIdAsync(id);
    }

    // Eliminación (por Email)
    public async Task<(bool Ok, string Message)> DeleteByEmailAsync(string email)
    {
        return await _userRepository.DeleteByEmailAsync(email);
    }

    // Métodos síncronos para compatibilidad con Program.cs
    public List<User> Index() => IndexAsync().Result;
    public User? GetById(int id) => GetByIdAsync(id).Result;
    public User? GetByEmail(string email) => GetByEmailAsync(email).Result;
    public List<User> GetByCity(string city) => GetByCityAsync(city).Result;
    public List<User> GetByCountry(string country) => GetByCountryAsync(country).Result;
    public List<User> GetByMinimumAge(int minimumAge) => GetByMinimumAgeAsync(minimumAge).Result;
    public List<User> GetByGender(string gender) => GetByGenderAsync(gender).Result;
    public List<NameEmail> GetNamesAndEmails() => GetNamesAndEmailsAsync().Result;
    public int CountAll() 
    {
        try
        {
            return CountAllAsync().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en CountAll: {ex.Message}");
            return 0;
        }
    }
    public List<(string City, int Count)> CountByCity() => CountByCityAsync().Result;
    public List<(string Country, int Count)> CountByCountry() => CountByCountryAsync().Result;
    public List<User> WithoutPhone() => WithoutPhoneAsync().Result;
    public List<User> WithoutAddress() => WithoutAddressAsync().Result;
    public List<User> LatestRegistered(int limit = 10) => LatestRegisteredAsync(limit).Result;
    public List<User> OrderedByLastName() => OrderedByLastNameAsync().Result;
    public List<User> GetPaged(int page, int pageSize) => GetPagedAsync(page, pageSize).Result;
    public (bool Ok, string Message, User? Created) Create(string firstName, string lastName, string username, string email) => CreateAsync(firstName, lastName, username, email).Result;
    public (bool Ok, string Message) Update(User updated) => UpdateAsync(updated).Result;
    public (bool Ok, string Message) UpdatePassword(int userId, string newPassword, string confirmPassword) => UpdatePasswordAsync(userId, newPassword, confirmPassword).Result;
    public (bool Ok, string Message) DeleteById(int id, Func<string> confirm) => DeleteByIdAsync(id).Result;
    public (bool Ok, string Message) DeleteByEmail(string email, Func<string> confirm) => DeleteByEmailAsync(email).Result;
}