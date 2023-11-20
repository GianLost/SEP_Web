using Microsoft.AspNetCore.Mvc;
using SEP_Web.Models;

namespace SEP_Web.Helper.Validation;
public interface IUsersValidation
{
    Task<bool> VerifyIfFieldExistsInBothUsersTable(string fieldName, object value);
    Task<List<(string FieldName, string Message)>> CheckForDuplicateDatatableFields(Users user);
    void LoginFieldsValidation(string fields, string errorMessage, Controller controller);
    bool ValidatePassword(string pass, string confirmPass, Controller controller);
    bool IsFieldChanged(Users existingUser, string fieldName, object newValue);
}
