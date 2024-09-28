$(document).ready(function () {
    $('#assessment-table').DataTable({
        ordering: true,  
        paging: true,    
        searching: true, 
        processing: true,
        serverSide: true,
        ajax: {
            url: '/Assessments/Index',
            type: 'POST',             
            dataType: 'json',         
            dataSrc: function (json) {
                //console.log("Dados retornados pelo servidor:", json);
                return json.data;
            },
            error: function (xhr, error, thrown) {
                //console.error('Erro ao carregar dados:', xhr.responseText);  // Exibir erro, caso ocorra
            }
        },
        columns: [
            { data: "statusTitle", render: renderAssessmentStatus },
            { data: "phase" },                                      
            { data: "masp" },                                       
            { data: "servantName" },                                
            { data: "startDate" },                                  
            { data: "endDate" },                                    
            {
                data: "canAssess",
                render: renderAssessmentButtonActions,
                orderable: false,
                className: 'dt-center'
            }
        ],
        language: {
            emptyTable: "Nenhum registro encontrado na tabela",
            info: "Mostrar _START_ até _END_ de _TOTAL_ registros",
            infoEmpty: "Mostrar 0 até 0 de 0 Registros",
            infoFiltered: "(Filtrar de um total de _MAX_ registros)",
            lengthMenu: "Mostrar _MENU_ registros por página",
            loadingRecords: "Carregando...",
            processing: "Carregando...",
            search: "Pesquisar :",
            zeroRecords: "Nenhum registro encontrado",
            paginate: {
                next: "Próximo",
                previous: "Anterior",
                first: "Primeiro",
                last: "Último"
            }
        },
        aria: {
            sortAscending: ": Ordenar colunas de forma ascendente",
            sortDescending: ": Ordenar colunas de forma descendente"
        },
        rowCallback: function (row, data) {
            $(row).addClass(data.rowColor);
        }
    });
});

// Função para renderizar o status com ícone e estilo
function renderAssessmentStatus(data, type, row) {
    return `<span value"${row.statusTitle}" title="${row.statusTitle}" class="material-symbols-outlined ${row.statusClass} fs-5 m-1 p-1">
                radio_button_checked
            </span>`;
}

// Função para renderizar os botões de ações na coluna de avaliação
function renderAssessmentButtonActions(data, type, row) {
    if (row.canAssess) {
        // Se o usuário pode avaliar, exibe um botão de avaliação
        return `<a title="Avaliar ..." role="button" href="/Assessments/ToAssess/${row.assessmentId}" class="btn btn-sm btn-outline-primary">
                    <i title="Avaliar ..." class="bi bi-journal-check"></i>
                </a>`;
    } else {
        // Caso contrário, exibe um botão desabilitado
        return `<a title="Etapa concluída ..." class="btn btn-sm btn-outline-secondary" disabled>
                    <i title="Etapa concluída ..." class="bi bi-journal-check"></i>
                </a>`;
    }
}