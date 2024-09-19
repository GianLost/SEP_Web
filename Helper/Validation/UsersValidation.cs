using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SEP_Web.Database;
using SEP_Web.Helper.Messages;
using SEP_Web.Models.UsersModels;

namespace SEP_Web.Helper.Validation;
public class UsersValidation : IUsersValidation
{
    private readonly SEP_WebContext _database;

    public UsersValidation(SEP_WebContext database)
    {
        _database = database ?? throw new ArgumentNullException(nameof(database));
    }

    public async Task<bool> CheckIfFieldExistsInAnyUserTable(string fieldName, object value)
    {

        return await _database.Administrators.AnyAsync(u => EF.Property<object>(u, fieldName) == value) ||
        await _database.Evaluators.AnyAsync(u => EF.Property<object>(u, fieldName) == value) ||
        await _database.Servants.AnyAsync(u => EF.Property<object>(u, fieldName) == value);

    }

    public async Task<List<(string FieldName, string Message)>> CheckForDuplicateUserFields(Users user)
    {
        List<(string FieldName, string Message)> duplicateErrors = new();

        List<(string FieldName, object Value)> fieldsToValidate = new()
        {
            ("Masp", user.Masp),
            ("Name", user.Name),
            ("Login", user.Login),
            ("Email", user.Email),
            ("Phone", user.Phone),
        };

        foreach (var (fieldName, value) in fieldsToValidate)
            if (await CheckIfFieldExistsInAnyUserTable(fieldName, value)) duplicateErrors.Add((fieldName, $"O {fieldName.ToLower()} informado já está em uso."));

        return duplicateErrors;
    }

    public virtual bool ValidatePassword(string pass, string confirmPass, Controller controller)
    {
        bool passwordsMatch = pass == confirmPass;

        if (!passwordsMatch)
            controller.TempData["ErrorPass"] = FeedbackMessages.UnMatchedPassords;

        return passwordsMatch;
    }

    public void LoginFieldsValidation(string fields, string errorMessage, Controller controller)
    {
        controller.TempData[fields] = errorMessage;
    }

    public bool IsFieldChanged(Users existingUser, string fieldName, object newValue)
    {
        if (existingUser == null)
            throw new ArgumentNullException(nameof(existingUser));

        var property = existingUser.GetType().GetProperty(fieldName)
            ?? throw new ArgumentException($"O campo '{fieldName}' não pôde ser mapeado.");

        var existingValue = property.GetValue(existingUser);

        return !Equals(existingValue, newValue);
    }
}