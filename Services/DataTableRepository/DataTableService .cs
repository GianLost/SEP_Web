using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using SEP_Web.Interfaces.DataTableInterfaces;
using SEP_Web.Models.DataTableModels;

namespace SEP_Web.Services.DataTableRepository;

public class DataTableService : IDataTableService
{
    public async Task<DataTableResponse<T>> GetPaginatedResponseAsync<T>(
            IQueryable<T> query,
            DataTableRequest request,
            Func<IQueryable<T>, IQueryable<T>> customFilter = null)
    {
        // Aplicar filtros personalizados, se fornecidos
        if (customFilter != null)
        {
            query = customFilter(query);
        }

        // Filtragem genérica baseada em pesquisa
        if (!string.IsNullOrEmpty(request.Search?.Value))
        {
            var searchValue = request.Search.Value.ToLower();

            // Exemplo de filtro genérico dinâmico em várias colunas
            var searchableColumns = string.Join(" || ", request.Columns.Where(c => c.Searchable)
                .Select(c => $"{c.Data}.ToLower().Contains(@0)"));

            if (!string.IsNullOrEmpty(searchableColumns))
            {
                // Usando Linq.Dynamic.Core para suportar filtros dinâmicos
                query = query.Where(searchableColumns, searchValue);
            }
        }

        // Total de registros antes da paginação
        var totalRecords = await query.CountAsync();

        // Ordenação (usando parâmetros do DataTables)
        if (request.Order != null)
        {
            var column = request.Columns[request.Order[0].Column].Data;
            var direction = request.Order[0].Dir;

            // Usando Linq.Dynamic.Core para ordenação dinâmica
            query = query.OrderBy($"{column} {direction}");
        }

        // Aplicar paginação
        var data = await query.Skip(request.Start).Take(request.Length).ToListAsync();

        // Retornar a resposta no formato esperado pelo DataTables
        return new DataTableResponse<T>
        {
            Draw = request.Draw,
            RecordsTotal = totalRecords,
            RecordsFiltered = totalRecords, // Se houver filtros, mudar isso para o total filtrado
            Data = data
        };
    }
}