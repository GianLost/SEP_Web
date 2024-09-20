using SEP_Web.Models.DataTableModels;

namespace SEP_Web.Interfaces.DataTableInterfaces;
public interface IDataTableService
{
    Task<DataTableResponse<T>> GetPaginatedResponseAsync<T>(
        IQueryable<T> query,
        DataTableRequest request,
        Func<IQueryable<T>, IQueryable<T>> customFilter = null);
}