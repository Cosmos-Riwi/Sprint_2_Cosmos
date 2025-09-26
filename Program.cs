﻿using Sprint2.Controllers;
using Sprint2.Models;
using Sprint2.Utils;
using Sprint2.Data;
using Sprint2.Repositories;

// Configurar inyección de dependencias
var dbContext = new MysqlDbContext();
var userRepository = new UserRepository(dbContext);
var controller = new UserController(userRepository);

void ShowMenu()
{
  Console.WriteLine("\nGestión de Usuarios");
  Console.WriteLine("1) Listar todos");
  Console.WriteLine("2) Ver por Id");
  Console.WriteLine("3) Ver por Email");
  Console.WriteLine("4) Listar por ciudad");
  Console.WriteLine("5) Listar por país");
  Console.WriteLine("6) Mayores de edad mínima");
  Console.WriteLine("7) Por género");
  Console.WriteLine("8) Nombres y correos");
  Console.WriteLine("9) Contar total");
  Console.WriteLine("10) Contar por ciudad");
  Console.WriteLine("11) Contar por país");
  Console.WriteLine("12) Sin teléfono");
  Console.WriteLine("13) Sin dirección");
  Console.WriteLine("14) Últimos registrados");
  Console.WriteLine("15) Ordenados por apellido");
  Console.WriteLine("16) Paginación de usuarios");
  Console.WriteLine("17) Crear usuario");
  Console.WriteLine("18) Actualizar usuario");
  Console.WriteLine("19) Actualizar contraseña");
  Console.WriteLine("20) Eliminar por Id");
  Console.WriteLine("21) Eliminar por Email");
  Console.WriteLine("0) Salir");
  Console.Write("Seleccione opción: ");
}

string Read(string label)
{
  Console.Write(label);
  return Console.ReadLine() ?? string.Empty;
}

// Método para validar con reintentos
T ValidateWithRetry<T>(string prompt, Func<string, (bool IsValid, T Value, string ErrorMessage)> validator, string fieldName)
{
  while (true)
  {
    string input = Read(prompt);
    var (isValid, value, errorMessage) = validator(input);
    
    if (isValid)
    {
      return value;
    }
    
    Console.WriteLine($"Error: {errorMessage}");
    Console.WriteLine($"Por favor, ingrese un {fieldName} válido:");
  }
}

while (true)
{
  ShowMenu();
  var input = Console.ReadLine();
  if (input == "0" || input?.ToLower() == "salir") break;

  try
  {
    switch (input)
    {
      case "1":
        foreach (var u in controller.Index()) Console.WriteLine($"{u.Id} - {u.Name} - {u.Email}");
        break;
      case "2":
        {
          int id = ValidateWithRetry("Id: ", input => ValidationHelper.ValidatePositiveInt(input, "ID"), "ID");
          
          var user = controller.GetById(id);
          if (user == null)
          {
            Console.WriteLine("Error: Usuario no encontrado");
          }
          else
          {
            Console.WriteLine($"Exito: {user.Id} - {user.Name} - {user.Email}");
          }
        }
        break;
      case "3":
        {
          string email = ValidateWithRetry("Email: ", input => ValidationHelper.ValidateEmail(input), "email");
          
          var user = controller.GetByEmail(email);
          if (user == null)
          {
            Console.WriteLine("Error: Usuario no encontrado");
          }
          else
          {
            Console.WriteLine($"Exito: {user.Id} - {user.Name} - {user.Email}");
          }
        }
        break;
      case "4":
        {
          string city = ValidateWithRetry("Ciudad: ", input => ValidationHelper.ValidateRequiredText(input, "Ciudad"), "ciudad");
          
          var users = controller.GetByCity(city);
          if (users.Count == 0)
          {
            Console.WriteLine("Error: No se encontraron usuarios en esa ciudad");
          }
          else
          {
            Console.WriteLine($"Exito: Usuarios en {city}:");
            foreach (var u in users) Console.WriteLine($"  {u.Id} - {u.Name} - {u.Email}");
          }
        }
        break;
      case "5":
        {
          string country = ValidateWithRetry("País: ", input => ValidationHelper.ValidateRequiredText(input, "País"), "país");
          
          var users = controller.GetByCountry(country);
          if (users.Count == 0)
          {
            Console.WriteLine("Error: No se encontraron usuarios en ese país");
          }
          else
          {
            Console.WriteLine($"Exito: Usuarios en {country}:");
            foreach (var u in users) Console.WriteLine($"  {u.Id} - {u.Name} - {u.Email}");
          }
        }
        break;
      case "6":
        {
          int minAge = ValidateWithRetry("Edad mínima: ", input => ValidationHelper.ValidateIntRange(input, "Edad mínima", 0, 120), "edad mínima");
          
          var users = controller.GetByMinimumAge(minAge);
          if (users.Count == 0)
          {
            Console.WriteLine($"Error: No se encontraron usuarios mayores de {minAge} años");
          }
          else
          {
            Console.WriteLine($"Exito: Usuarios mayores de {minAge} años:");
            foreach (var u in users) Console.WriteLine($"  {u.Id} - {u.Name} - {u.Email} - {u.Age} años");
          }
        }
        break;
      case "7":
        {
          string gender = ValidateWithRetry("Género (male/female/m/f): ", input => ValidationHelper.ValidateGender(input), "género");
          
          var users = controller.GetByGender(gender);
          if (users.Count == 0)
          {
            Console.WriteLine($"Error: No se encontraron usuarios con género {gender}");
          }
          else
          {
            Console.WriteLine($"Exito: Usuarios con género {gender}:");
            foreach (var u in users) Console.WriteLine($"  {u.Id} - {u.Name} - {u.Email} - {u.Gender}");
          }
        }
        break;
      case "8":
        {
          var namesAndEmails = controller.GetNamesAndEmails();
          if (namesAndEmails.Count == 0)
          {
            Console.WriteLine("Error: No hay usuarios registrados");
          }
          else
          {
            Console.WriteLine($"Exito: Nombres y correos ({namesAndEmails.Count} usuarios):");
            foreach (var ne in namesAndEmails) 
            {
              Console.WriteLine($"  {ne.Name} - {ne.Email}");
            }
          }
        }
        break;
      case "9":
        {
          var total = controller.CountAll();
          Console.WriteLine($"Exito: Total de usuarios: {total}");
        }
        break;
      case "10":
        {
          var cityCounts = controller.CountByCity();
          if (cityCounts.Count == 0)
          {
            Console.WriteLine("Error: No hay datos de ciudades");
          }
          else
          {
            Console.WriteLine($"Exito: Conteo por ciudad ({cityCounts.Count} ciudades):");
            foreach (var t in cityCounts) 
            {
              Console.WriteLine($"  {t.City}: {t.Count} usuarios");
            }
          }
        }
        break;
      case "11":
        {
          var countryCounts = controller.CountByCountry();
          if (countryCounts.Count == 0)
          {
            Console.WriteLine("Error: No hay datos de países");
          }
          else
          {
            Console.WriteLine($"Exito: Conteo por país ({countryCounts.Count} países):");
            foreach (var t in countryCounts) 
            {
              Console.WriteLine($"  {t.Country}: {t.Count} usuarios");
            }
          }
        }
        break;
      case "12":
        {
          var usersWithoutPhone = controller.WithoutPhone();
          if (usersWithoutPhone.Count == 0)
          {
            Console.WriteLine("Exito: Todos los usuarios tienen teléfono registrado");
          }
          else
          {
            Console.WriteLine($"Advertencia:  Usuarios sin teléfono ({usersWithoutPhone.Count} usuarios):");
            foreach (var u in usersWithoutPhone) 
            {
              Console.WriteLine($"  {u.Id} - {u.Name} - {u.Email}");
            }
          }
        }
        break;
      case "13":
        {
          var usersWithoutAddress = controller.WithoutAddress();
          if (usersWithoutAddress.Count == 0)
          {
            Console.WriteLine("Exito: Todos los usuarios tienen dirección registrada");
          }
          else
          {
            Console.WriteLine($"Advertencia:  Usuarios sin dirección ({usersWithoutAddress.Count} usuarios):");
            foreach (var u in usersWithoutAddress) 
            {
              Console.WriteLine($"  {u.Id} - {u.Name} - {u.Email}");
            }
          }
        }
        break;
      case "14":
        {
          int limit = ValidateWithRetry("Cantidad: ", input => ValidationHelper.ValidateIntRange(input, "Cantidad", 1, 100), "cantidad");
          
          var users = controller.LatestRegistered(limit);
          if (users.Count == 0)
          {
            Console.WriteLine("Error: No hay usuarios registrados");
          }
          else
          {
            Console.WriteLine($"Exito: Últimos {users.Count} usuarios registrados:");
            foreach (var u in users) Console.WriteLine($"  {u.Id} - {u.Name} - {u.Email} - {u.CreatedAt:yyyy-MM-dd HH:mm}");
          }
        }
        break;
      case "15":
        {
          var orderedUsers = controller.OrderedByLastName();
          if (orderedUsers.Count == 0)
          {
            Console.WriteLine("Error: No hay usuarios registrados");
          }
          else
          {
            Console.WriteLine($"Exito: Usuarios ordenados por apellido ({orderedUsers.Count} usuarios):");
            foreach (var u in orderedUsers) 
            {
              Console.WriteLine($"  {u.Id} - {u.LastName}, {u.FirstName} - {u.Email}");
            }
          }
        }
        break;
      case "16":
        {
          int page = ValidateWithRetry("Página (1-∞): ", input => ValidationHelper.ValidatePositiveInt(input, "Página"), "página");
          int pageSize = ValidateWithRetry("Tamaño de página (1-50): ", input => ValidationHelper.ValidateIntRange(input, "Tamaño de página", 1, 50), "tamaño de página");

          var pagedUsers = controller.GetPaged(page, pageSize);
          if (pagedUsers.Count == 0)
          {
            Console.WriteLine("Error: No hay usuarios en esta página");
          }
          else
          {
            Console.WriteLine($"Exito: Página {page} de usuarios (mostrando {pagedUsers.Count} de {pageSize} por página):");
            foreach (var u in pagedUsers) 
            {
              Console.WriteLine($"  {u.Id} - {u.Name} - {u.Email}");
            }
          }
        }
        break;
      case "17":
        {
          string firstName = ValidateWithRetry("Nombre: ", input => ValidationHelper.ValidateRequiredText(input, "Nombre"), "nombre");
          string lastName = ValidateWithRetry("Apellido: ", input => ValidationHelper.ValidateRequiredText(input, "Apellido"), "apellido");
          string username = ValidateWithRetry("Usuario: ", input => ValidationHelper.ValidateRequiredText(input, "Usuario"), "usuario");
          string email = ValidateWithRetry("Email: ", input => ValidationHelper.ValidateEmail(input), "email");

          var result = controller.Create(firstName, lastName, username, email);
          if (result.Ok)
          {
            Console.WriteLine($"Exito: {result.Message}");
          }
          else
          {
            Console.WriteLine($"Error: {result.Message}");
          }
        }
        break;
      case "18":
        {
          // Validar ID
          string idInput = Read("Id a actualizar: ");
          var (isValidId, id, idError) = ValidationHelper.ValidatePositiveInt(idInput, "ID");
          if (!isValidId)
          {
            Console.WriteLine($"Error: {idError}");
            break;
          }

          // Verificar que el usuario existe
          var userToUpdate = controller.GetById(id);
          if (userToUpdate == null)
          {
            Console.WriteLine("Error: Usuario no encontrado");
            break;
          }

          Console.WriteLine($"Info: Actualizando usuario: {userToUpdate.Name} ({userToUpdate.Email})");
          Console.WriteLine("(Presiona Enter para mantener el valor actual)");

          // Validar y actualizar nombre
          string firstNameInput = Read($"Nombre ({userToUpdate.FirstName}): ");
          if (!string.IsNullOrWhiteSpace(firstNameInput))
          {
            var (isValidFirstName, firstName, firstNameError) = ValidationHelper.ValidateRequiredText(firstNameInput, "Nombre");
            if (!isValidFirstName)
            {
              Console.WriteLine($"Error: {firstNameError}");
              break;
            }
            userToUpdate.FirstName = firstName;
          }

          // Validar y actualizar apellido
          string lastNameInput = Read($"Apellido ({userToUpdate.LastName}): ");
          if (!string.IsNullOrWhiteSpace(lastNameInput))
          {
            var (isValidLastName, lastName, lastNameError) = ValidationHelper.ValidateRequiredText(lastNameInput, "Apellido");
            if (!isValidLastName)
            {
              Console.WriteLine($"Error: {lastNameError}");
              break;
            }
            userToUpdate.LastName = lastName;
          }

          // Validar y actualizar usuario
          string usernameInput = Read($"Usuario ({userToUpdate.Username}): ");
          if (!string.IsNullOrWhiteSpace(usernameInput))
          {
            var (isValidUsername, username, usernameError) = ValidationHelper.ValidateRequiredText(usernameInput, "Usuario");
            if (!isValidUsername)
            {
              Console.WriteLine($"Error: {usernameError}");
              break;
            }
            userToUpdate.Username = username;
          }

          // Validar y actualizar email
          string emailInput = Read($"Email ({userToUpdate.Email}): ");
          if (!string.IsNullOrWhiteSpace(emailInput))
          {
            var (isValidEmail, email, emailError) = ValidationHelper.ValidateEmail(emailInput);
            if (!isValidEmail)
            {
              Console.WriteLine($"Error: {emailError}");
              break;
            }
            userToUpdate.Email = email;
          }

          // Actualizar campos opcionales
          string phoneInput = Read($"Teléfono ({userToUpdate.Phone}): ");
          if (!string.IsNullOrWhiteSpace(phoneInput))
          {
            var (isValidPhone, phone, phoneError) = ValidationHelper.ValidatePhone(phoneInput);
            if (!isValidPhone)
            {
              Console.WriteLine($"Error: {phoneError}");
              break;
            }
            userToUpdate.Phone = phone;
          }

          string cellphoneInput = Read($"Celular ({userToUpdate.Cellphone}): ");
          if (!string.IsNullOrWhiteSpace(cellphoneInput))
          {
            var (isValidCellphone, cellphone, cellphoneError) = ValidationHelper.ValidatePhone(cellphoneInput);
            if (!isValidCellphone)
            {
              Console.WriteLine($"Error: {cellphoneError}");
              break;
            }
            userToUpdate.Cellphone = cellphone;
          }

          string addressInput = Read($"Dirección ({userToUpdate.Address}): ");
          if (!string.IsNullOrWhiteSpace(addressInput))
          {
            userToUpdate.Address = addressInput.Trim();
          }

          string cityInput = Read($"Ciudad ({userToUpdate.City}): ");
          if (!string.IsNullOrWhiteSpace(cityInput))
          {
            userToUpdate.City = cityInput.Trim();
          }

          string stateInput = Read($"Estado ({userToUpdate.State}): ");
          if (!string.IsNullOrWhiteSpace(stateInput))
          {
            userToUpdate.State = stateInput.Trim();
          }

          string zipcodeInput = Read($"Zipcode ({userToUpdate.Zipcode}): ");
          if (!string.IsNullOrWhiteSpace(zipcodeInput))
          {
            userToUpdate.Zipcode = zipcodeInput.Trim();
          }

          string countryInput = Read($"País ({userToUpdate.Country}): ");
          if (!string.IsNullOrWhiteSpace(countryInput))
          {
            userToUpdate.Country = countryInput.Trim();
          }

          string genderInput = Read($"Género ({userToUpdate.Gender}): ");
          if (!string.IsNullOrWhiteSpace(genderInput))
          {
            var (isValidGender, gender, genderError) = ValidationHelper.ValidateGender(genderInput);
            if (!isValidGender)
            {
              Console.WriteLine($"Error: {genderError}");
              break;
            }
            userToUpdate.Gender = gender;
          }

          string ageInput = Read($"Edad ({userToUpdate.Age}): ");
          if (!string.IsNullOrWhiteSpace(ageInput))
          {
            var (isValidAge, age, ageError) = ValidationHelper.ValidateIntRange(ageInput, "Edad", 0, 120);
            if (!isValidAge)
            {
              Console.WriteLine($"Error: {ageError}");
              break;
            }
            userToUpdate.Age = age;
          }

          // Confirmar actualización
          Console.WriteLine($"Advertencia:  ¿Está seguro de actualizar este usuario? (S/N): ");
          string confirmInput = Read("");
          var (isValidConfirm, confirmError) = ValidationHelper.ValidateConfirmation(confirmInput);
          if (!isValidConfirm)
          {
            Console.WriteLine($"Error: {confirmError}");
            break;
          }

          var result = controller.Update(userToUpdate);
          if (result.Ok)
          {
            Console.WriteLine($"Exito: {result.Message}");
          }
          else
          {
            Console.WriteLine($"Error: {result.Message}");
          }
        }
        break;
      case "19":
        {
          string idInput = Read("Id: ");
          var (isValidId, id, idError) = ValidationHelper.ValidatePositiveInt(idInput, "ID");
          if (!isValidId)
          {
            Console.WriteLine($"Error: {idError}");
            break;
          }

          string newPassword = Read("Nueva contraseña: ");
          string confirmPassword = Read("Confirmar contraseña: ");
          
          var (isValidPassword, passwordError) = ValidationHelper.ValidatePassword(newPassword, confirmPassword);
          if (!isValidPassword)
          {
            Console.WriteLine($"Error: {passwordError}");
            break;
          }

          var result = controller.UpdatePassword(id, newPassword, confirmPassword);
          if (result.Ok)
          {
            Console.WriteLine($"Exito: {result.Message}");
          }
          else
          {
            Console.WriteLine($"Error: {result.Message}");
          }
        }
        break;
      case "20":
        {
          string idInput = Read("Id a eliminar: ");
          var (isValidId, id, idError) = ValidationHelper.ValidatePositiveInt(idInput, "ID");
          if (!isValidId)
          {
            Console.WriteLine($"Error: {idError}");
            break;
          }

          // Verificar que el usuario existe antes de pedir confirmación
          var userToDelete = controller.GetById(id);
          if (userToDelete == null)
          {
            Console.WriteLine("Error: Usuario no encontrado");
            break;
          }

          Console.WriteLine($"Advertencia:  Usuario a eliminar: {userToDelete.Name} ({userToDelete.Email})");
          string confirmInput = Read("¿Está seguro de eliminar este usuario? (S/N): ");
          var (isValidConfirm, confirmError) = ValidationHelper.ValidateConfirmation(confirmInput);
          if (!isValidConfirm)
          {
            Console.WriteLine($"Error: {confirmError}");
            break;
          }

          var result = controller.DeleteById(id, () => "S");
          if (result.Ok)
          {
            Console.WriteLine($"Exito: {result.Message}");
          }
          else
          {
            Console.WriteLine($"Error: {result.Message}");
          }
        }
        break;
      case "21":
        {
          string emailInput = Read("Email a eliminar: ");
          var (isValidEmail, email, emailError) = ValidationHelper.ValidateEmail(emailInput);
          if (!isValidEmail)
          {
            Console.WriteLine($"Error: {emailError}");
            break;
          }

          // Verificar que el usuario existe antes de pedir confirmación
          var userToDelete = controller.GetByEmail(email);
          if (userToDelete == null)
          {
            Console.WriteLine("Error: Usuario no encontrado");
            break;
          }

          Console.WriteLine($"Advertencia:  Usuario a eliminar: {userToDelete.Name} ({userToDelete.Email})");
          string confirmInput = Read("¿Está seguro de eliminar este usuario? (S/N): ");
          var (isValidConfirm, confirmError) = ValidationHelper.ValidateConfirmation(confirmInput);
          if (!isValidConfirm)
          {
            Console.WriteLine($"Error: {confirmError}");
            break;
          }

          var result = controller.DeleteByEmail(email, () => "S");
          if (result.Ok)
          {
            Console.WriteLine($"Exito: {result.Message}");
          }
          else
          {
            Console.WriteLine($"Error: {result.Message}");
          }
        }
        break;
      default:
        Console.WriteLine("Opción inválida");
        break;
    }
  }
  catch (Exception ex)
  {
    Console.WriteLine($"Error: {ex.Message}");
  }
}