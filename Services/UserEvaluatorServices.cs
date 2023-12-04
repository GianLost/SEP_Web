using Microsoft.EntityFrameworkCore;
using SEP_Web.Database;
using SEP_Web.Helper.Messages;
using SEP_Web.Models;

namespace SEP_Web.Services;

public class UserEvaluatorServices : IUserEvaluatorServices
{
    private readonly SEP_WebContext _database;

    public UserEvaluatorServices(SEP_WebContext database)
    {
        _database = database;
    }
    
    public void DeleteEvaluator(int id)
    {
        throw new NotImplementedException();
    }

    public Task<UserEvaluator> EvaluatorsEdit(UserEvaluator user)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<UserEvaluator>> EvaluatorsList()
    {
        ICollection<UserEvaluator> administrators = await _database.Evaluators.ToListAsync();

        if (administrators?.Count == 0)
            throw new ArgumentNullException(nameof(administrators), ExceptionMessages.ErrorArgumentNullException);

        return administrators;
    }

    public Task<UserEvaluator> RegisterEvaluator(UserEvaluator user)
    {
        throw new NotImplementedException();
    }

    public UserEvaluator SearchForId(int id)
    {
        throw new NotImplementedException();
    }
}