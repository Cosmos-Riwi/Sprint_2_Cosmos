using Microsoft.EntityFrameworkCore;
using Sprint2.Data;
using Sprint2.Models;

namespace Sprint2.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MysqlDbContext _context;

    public UserRepository(MysqlDbContext context)
    {
        _context = context;
    }

    // READ operations
    public async Task<List<User>> GetAllAsync()
    {
        return await _context.users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<List<User>> GetByCityAsync(string city)
    {
        return await _context.users.Where(u => u.City == city).ToListAsync();
    }

    public async Task<List<User>> GetByCountryAsync(string country)
    {
        return await _context.users.Where(u => u.Country == country).ToListAsync();
    }

    public async Task<List<User>> GetByMinimumAgeAsync(int minimumAge)
    {
        return await _context.users
            .Where(u => u.Age != null && u.Age >= minimumAge)
            .ToListAsync();
    }

    public async Task<List<User>> GetByGenderAsync(string gender)
    {
        return await _context.users.Where(u => u.Gender == gender).ToListAsync();
    }

    public async Task<List<NameEmail>> GetNamesAndEmailsAsync()
    {
        return await _context.users
            .Select(u => new NameEmail { Name = u.FirstName + " " + u.LastName, Email = u.Email })
            .ToListAsync();
    }

    public async Task<int> CountAllAsync()
    {
        return await _context.users.CountAsync();
    }

    public async Task<List<(string City, int Count)>> CountByCityAsync()
    {
        return await _context.users
            .GroupBy(u => u.City)
            .Select(g => new ValueTuple<string, int>(g.Key ?? string.Empty, g.Count()))
            .ToListAsync();
    }

    public async Task<List<(string Country, int Count)>> CountByCountryAsync()
    {
        return await _context.users
            .GroupBy(u => u.Country)
            .Select(g => new ValueTuple<string, int>(g.Key ?? string.Empty, g.Count()))
            .ToListAsync();
    }

    public async Task<List<User>> WithoutPhoneAsync()
    {
        return await _context.users
            .Where(u => string.IsNullOrEmpty(u.Phone))
            .ToListAsync();
    }

    public async Task<List<User>> WithoutAddressAsync()
    {
        return await _context.users
            .Where(u => string.IsNullOrEmpty(u.Address))
            .ToListAsync();
    }

    public async Task<List<User>> LatestRegisteredAsync(int limit = 10)
    {
        return await _context.users
            .OrderByDescending(u => u.CreatedAt)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<List<User>> OrderedByLastNameAsync()
    {
        return await _context.users
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .ToListAsync();
    }

    public async Task<List<User>> GetPagedAsync(int page, int pageSize)
    {
        return await _context.users
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    // WRITE operations with transactions
    public async Task<(bool Ok, string Message, User? Created)> CreateAsync(User user)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Validar unicidad
            if (await ExistsByUsernameAsync(user.Username))
            {
                return (false, "El nombre de usuario ya existe.", null);
            }
            if (await ExistsByEmailAsync(user.Email))
            {
                return (false, "El correo electrónico ya existe.", null);
            }

            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            _context.users.Add(user);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return (true, "Usuario creado correctamente.", user);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return (false, $"Error al crear usuario: {ex.Message}", null);
        }
    }

    public async Task<(bool Ok, string Message)> UpdateAsync(User user)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var current = await _context.users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (current == null)
            {
                return (false, "Usuario no encontrado.");
            }

            // Validar unicidad si cambian username o email
            if (!string.Equals(current.Username, user.Username, StringComparison.OrdinalIgnoreCase)
                && await ExistsByUsernameAsync(user.Username, user.Id))
            {
                return (false, "El nombre de usuario ya existe.");
            }
            if (!string.Equals(current.Email, user.Email, StringComparison.OrdinalIgnoreCase)
                && await ExistsByEmailAsync(user.Email, user.Id))
            {
                return (false, "El correo electrónico ya existe.");
            }

            // Actualizar campos
            current.FirstName = user.FirstName;
            current.LastName = user.LastName;
            current.Username = user.Username;
            current.Email = user.Email;
            current.Phone = user.Phone;
            current.Cellphone = user.Cellphone;
            current.Address = user.Address;
            current.City = user.City;
            current.State = user.State;
            current.Zipcode = user.Zipcode;
            current.Country = user.Country;
            current.Gender = user.Gender;
            current.Age = user.Age;
            current.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return (true, "Datos actualizados correctamente.");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return (false, $"Error al actualizar usuario: {ex.Message}");
        }
    }

    public async Task<(bool Ok, string Message)> UpdatePasswordAsync(int userId, string newPassword)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var current = await _context.users.FirstOrDefaultAsync(u => u.Id == userId);
            if (current == null)
            {
                return (false, "Usuario no encontrado.");
            }

            current.Password = newPassword;
            current.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return (true, "Contraseña actualizada correctamente.");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return (false, $"Error al actualizar contraseña: {ex.Message}");
        }
    }

    public async Task<(bool Ok, string Message)> DeleteByIdAsync(int id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var current = await _context.users.FirstOrDefaultAsync(u => u.Id == id);
            if (current == null)
            {
                return (false, "Usuario no encontrado.");
            }

            _context.users.Remove(current);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return (true, "Usuario eliminado correctamente.");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return (false, $"Error al eliminar usuario: {ex.Message}");
        }
    }

    public async Task<(bool Ok, string Message)> DeleteByEmailAsync(string email)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var current = await _context.users.FirstOrDefaultAsync(u => u.Email == email);
            if (current == null)
            {
                return (false, "Usuario no encontrado.");
            }

            _context.users.Remove(current);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return (true, "Usuario eliminado correctamente.");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return (false, $"Error al eliminar usuario: {ex.Message}");
        }
    }

    // VALIDATION operations
    public async Task<bool> ExistsByUsernameAsync(string username, int? excludeId = null)
    {
        var query = _context.users.Where(u => u.Username == username);
        if (excludeId.HasValue)
        {
            query = query.Where(u => u.Id != excludeId.Value);
        }
        return await query.AnyAsync();
    }

    public async Task<bool> ExistsByEmailAsync(string email, int? excludeId = null)
    {
        var query = _context.users.Where(u => u.Email == email);
        if (excludeId.HasValue)
        {
            query = query.Where(u => u.Id != excludeId.Value);
        }
        return await query.AnyAsync();
    }
}