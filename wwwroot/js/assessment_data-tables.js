$(document).ready(function () {
    $('#assessment-table').DataTable({
        ordering: true,          // Habilitar a ordenação
        paging: true,            // Habilitar a paginação
        searching: true,        // Desabilitar a pesquisa (habilite se necessário)
        processing: true,        // Exibir mensagem de processamento enquanto os dados são carregados
        serverSide: true,        // Habilitar o modo server-side
        ajax: {
            url: '/Assessments/Index',  // URL da ação no servidor que processa a requisição
            type: 'POST',               // Tipo de requisição (POST no seu caso)
            dataType: 'json',           // O formato dos dados esperados
            data: function (d) {        // Adicionar dados extras à requisição, se necessário
                return $.extend({}, d, {
                    // Adicione outros parâmetros aqui se precisar enviar mais dados ao servidor
                });
            },
            error: function (xhr, error, thrown) {
                console.error('Erro ao carregar dados:', xhr.responseText);  // Exibir erro, caso ocorra
            }
        },
        columns: [
            { data: "statusTitle", render: renderAssessmentStatus },  // Coluna customizada para o status
            { data: "phase" },                                        // Coluna da etapa
            { data: "masp" },                                         // Coluna do MASP
            { data: "servantName" },                                  // Coluna do nome do servidor
            { data: "startDate" },                                    // Coluna da data de início da etapa
            { data: "endDate" },                                      // Coluna da data de avaliação
            { data: "canAssess", render: renderAssessmentButtonActions } // Coluna de botões de ações
        ],
        language: {  // Tradução das mensagens de DataTables
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
        aria: {  // Acessibilidade
            sortAscending: ": Ordenar colunas de forma ascendente",
            sortDescending: ": Ordenar colunas de forma descendente"
        },
        rowCallback: function (row, data) {
            // Aplica uma classe CSS customizada na linha com base no RowColor da resposta
            $(row).addClass(data.rowColor);
        }
    });
});

// Função para renderizar o status com ícone e estilo
function renderAssessmentStatus(data, type, row) {
    return `<span title="${row.statusTitle}" class="material-symbols-outlined ${row.statusClass} fs-5 m-1 p-1">
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