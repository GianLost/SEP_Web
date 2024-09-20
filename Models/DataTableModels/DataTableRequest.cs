namespace SEP_Web.Models.DataTableModels;

public class DataTableRequest
{
    public int Draw { get; set; } // Um contador para rastrear as solicitações do DataTables
    public int Start { get; set; } // Índice de início para paginação
    public int Length { get; set; } // Número de registros por página
    public DataTableColumn[] Columns { get; set; } // Colunas da tabela
    public DataTableOrder[] Order { get; set; } // Informações de ordenação
    public DataTableSearch Search { get; set; } // Informação de pesquisa
}

public class DataTableColumn
{
    public string Data { get; set; } // Nome da coluna
    public string Name { get; set; } // Nome legível da coluna
    public bool Searchable { get; set; } // Se a coluna é pesquisável
    public bool Orderable { get; set; } // Se a coluna é ordenável
    public DataTableSearch Search { get; set; } // Termo de pesquisa para a coluna
}

public class DataTableOrder
{
    public int Column { get; set; } // Índice da coluna que está sendo ordenada
    public string Dir { get; set; } // Direção da ordenação ("asc" ou "desc")
}

public class DataTableSearch
{
    public string Value { get; set; } // Termo de pesquisa
    public bool Regex { get; set; } // Se a pesquisa é por expressão regular (geralmente não é)
}