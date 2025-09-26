using System.Text.RegularExpressions;

namespace Sprint2.Utils;

public static class ValidationHelper
{
    // Validación de enteros positivos
    public static (bool IsValid, int Value, string ErrorMessage) ValidatePositiveInt(string? input, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(input))
            return (false, 0, $"{fieldName} es obligatorio");

        if (!int.TryParse(input, out int value))
            return (false, 0, $"{fieldName} debe ser un número válido");

        if (value <= 0)
            return (false, 0, $"{fieldName} debe ser un número positivo");

        return (true, value, string.Empty);
    }

    // Validación de enteros con rango
    public static (bool IsValid, int Value, string ErrorMessage) ValidateIntRange(string? input, string fieldName, int min, int max)
    {
        if (string.IsNullOrWhiteSpace(input))
            return (false, 0, $"{fieldName} es obligatorio");

        if (!int.TryParse(input, out int value))
            return (false, 0, $"{fieldName} debe ser un número válido");

        if (value < min || value > max)
            return (false, 0, $"{fieldName} debe estar entre {min} y {max}");

        return (true, value, string.Empty);
    }

    // Validación de texto no vacío
    public static (bool IsValid, string Value, string ErrorMessage) ValidateRequiredText(string? input, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(input))
            return (false, string.Empty, $"{fieldName} es obligatorio");

        if (input.Trim().Length < 2)
            return (false, string.Empty, $"{fieldName} debe tener al menos 2 caracteres");

        return (true, input.Trim(), string.Empty);
    }

    // Validación de email
    public static (bool IsValid, string Value, string ErrorMessage) ValidateEmail(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return (false, string.Empty, "Email es obligatorio");

        string email = input.Trim();
        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        
        if (!Regex.IsMatch(email, emailPattern))
            return (false, string.Empty, "Formato de email inválido");

        return (true, email, string.Empty);
    }

    // Validación de género
    public static (bool IsValid, string Value, string ErrorMessage) ValidateGender(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return (false, string.Empty, "Género es obligatorio");

        string gender = input.Trim().ToLower();
        if (gender != "male" && gender != "female" && gender != "m" && gender != "f")
            return (false, string.Empty, "Género debe ser 'male', 'female', 'm' o 'f'");

        return (true, gender, string.Empty);
    }

    // Validación de teléfono (opcional)
    public static (bool IsValid, string Value, string ErrorMessage) ValidatePhone(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return (true, string.Empty, string.Empty); // Teléfono es opcional

        string phone = input.Trim();
        if (phone.Length < 7)
            return (false, string.Empty, "Teléfono debe tener al menos 7 dígitos");

        return (true, phone, string.Empty);
    }

    // Validación de confirmación
    public static (bool IsValid, string ErrorMessage) ValidateConfirmation(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return (false, "Debe confirmar la acción");

        string response = input.Trim().ToLower();
        if (response != "s" && response != "si" && response != "y" && response != "yes")
            return (false, "Confirmación inválida. Use 'S' para confirmar");

        return (true, string.Empty);
    }

    // Validación de contraseña
    public static (bool IsValid, string ErrorMessage) ValidatePassword(string? password, string? confirmPassword)
    {
        if (string.IsNullOrWhiteSpace(password))
            return (false, "Contraseña es obligatoria");

        if (password.Length < 6)
            return (false, "Contraseña debe tener al menos 6 caracteres");

        if (password != confirmPassword)
            return (false, "Las contraseñas no coinciden");

        return (true, string.Empty);
    }
}