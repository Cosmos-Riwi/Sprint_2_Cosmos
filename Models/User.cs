namespace Sprint2.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("users")]
public class User
{
    [Column("id")]
    public int Id { get; set; }  

    [Column("first_name")]
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 50 caracteres")]
    public string FirstName { get; set; } = string.Empty;

    [Column("last_name")]
    [Required(ErrorMessage = "El apellido es obligatorio")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "El apellido debe tener entre 2 y 50 caracteres")]
    public string LastName { get; set; } = string.Empty;

    [Column("email")]
    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
    public string Email { get; set; } = string.Empty;

    [Column("username")]
    [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "El nombre de usuario debe tener entre 3 y 30 caracteres")]
    public string Username { get; set; } = string.Empty;

    [Column("phone")]
    [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
    public string? Phone { get; set; }

    [Column("cellphone")]
    [StringLength(20, ErrorMessage = "El celular no puede exceder 20 caracteres")]
    public string? Cellphone { get; set; }

    [Column("address")]
    [StringLength(200, ErrorMessage = "La dirección no puede exceder 200 caracteres")]
    public string? Address { get; set; }

    [Column("city")]
    [StringLength(50, ErrorMessage = "La ciudad no puede exceder 50 caracteres")]
    public string? City { get; set; }

    [Column("state")]
    [StringLength(50, ErrorMessage = "El estado no puede exceder 50 caracteres")]
    public string? State { get; set; }

    [Column("zipcode")]
    [StringLength(10, ErrorMessage = "El código postal no puede exceder 10 caracteres")]
    public string? Zipcode { get; set; }

    [Column("country")]
    [StringLength(50, ErrorMessage = "El país no puede exceder 50 caracteres")]
    public string? Country { get; set; }

    [Column("gender")]
    [RegularExpression("^(male|female|m|f)$", ErrorMessage = "El género debe ser 'male', 'female', 'm' o 'f'")]
    public string? Gender { get; set; }

    [Column("age")]
    [Range(0, 120, ErrorMessage = "La edad debe estar entre 0 y 120 años")]
    public int? Age { get; set; }

    [Column("password")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 100 caracteres")]
    public string? Password { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [NotMapped]
    public string Name => ($"{FirstName} {LastName}").Trim();
}