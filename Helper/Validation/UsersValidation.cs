using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SEP_Web.Database;
using SEP_Web.Helper.Messages;
using SEP_Web.Models;
using SEP_Web.Services;

namespace SEP_Web.Helper.Validation;
public class UsersValidation : IUsersValidation
{
    private readonly SEP_WebContext _database;
    private readonly IUserAdministratorServices _administratorServices;

    public UsersValidation(SEP_WebContext database, IUserAdministratorServices administratorServices)
    {
        _database = database;
        _administratorServices = administratorServices;
    }

    public async Task<bool> VerifyIfFieldExistsInBothUsersTable(string fieldName, object value)
    {
        bool Administrators = await _database.Administrators.AnyAsync(u => EF.Property<object>(u, fieldName) == value);
        bool Evaluators = await _database.Evaluators.AnyAsync(u => EF.Property<object>(u, fieldName) == value);
        bool Servants = await _database.Servants.AnyAsync(u => EF.Property<object>(u, fieldName) == value);

        return Administrators || Evaluators || Servants;
    }

    public async Task<List<(string FieldName, string Message)>> CheckForDuplicateDatatableFields(Users user)
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
            if (await VerifyIfFieldExistsInBothUsersTable(fieldName, value)) duplicateErrors.Add((fieldName, $"O {fieldName.ToLower()} informado já está em uso."));
        
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
        var existingValue = existingUser.GetType().GetProperty(fieldName).GetValue(existingUser);
        return !Equals(existingValue, newValue);
    }
}
