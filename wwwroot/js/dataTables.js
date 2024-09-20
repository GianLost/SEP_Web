function initializeDataTable(tableId, ajaxUrl, columns, hasActions = false) {
    var finalColumns = columns;

    // Se a tabela tiver colunas de ações, adicione a coluna de botões no final
    if (hasActions) {
        finalColumns.push({
            data: null,
            className: 'dt-center',
            orderable: false, // Desativa a ordenação na coluna de botões
            render: function (data, type, row) {
                return `
                    <button type="button" title="Editar" class="btn btn-sm btn-outline-primary edit-item" data-id="${row.id}">
                        <i class="bi bi-building-up"></i>
                    </button>
                    <button type="button" title="Excluir" class="btn btn-sm btn-outline-danger delete-item" data-id="${row.id}">
                        <i class="bi bi-building-x"></i>
                    </button>
                `;
            }
        });
    }

    $(tableId).DataTable({
        "ordering": true,
        "paging": true,
        "searching": false,
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": ajaxUrl,
            "type": "POST"
        },
        "columns": finalColumns,
        "language": {
            "emptyTable": "Nenhum registro encontrado na tabela",
            "info": "Mostrar _START_ até _END_ de _TOTAL_ registros",
            "infoEmpty": "Mostrar 0 até 0 de 0 Registros",
            "infoFiltered": "(Filtrar de um total de _MAX_ registros)",
            "loadingRecords": "Carregando...",
            "processing": "Carregando...",
            "zeroRecords": "Nenhum registro encontrado",
            "search": "Pesquisar",
            "paginate": {
                "next": "Proximo",
                "previous": "Anterior",
                "first": "Primeiro",
                "last": "Ultimo"
            },
            "aria": {
                "sortAscending": ": Ordenar colunas de forma ascendente",
                "sortDescending": ": Ordenar colunas de forma descendente"
            }
        }
    });
}

// Inicialização do DataTables para diferentes tabelas

initializeDataTable("#instituition-table", '/Instituition/Index', [
    { data: "name", name: "Name" }
], true);

initializeDataTable("#division-table", '/Division/Index', [
    { data: "name", name: "Name" },
    { data: "instituitionName", name: "InstituitionName" }
], true);

initializeDataTable("#section-table", '/Section/Index', [
    { data: "name", name: "Name" },
    { data: "divisionName", name: "DivisionName" }
], true);

initializeDataTable("#sector-table", '/Sector/Index', [
    { data: "name", name: "Name" },
    { data: "sectionName", name: "SectionName" }
], true);