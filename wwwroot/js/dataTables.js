function initializeDataTable(tableId, ajaxUrl, columns, hasActions = false, customButtonRender = null) {
    var finalColumns = columns;

    // Se a tabela tiver colunas de ações, adicione a coluna de botões no final
    if (hasActions) {
        finalColumns.push({
            data: null,
            className: 'dt-center',
            orderable: false, // Desativa a ordenação na coluna de botões
            render: function (data, type, row) {
                if (customButtonRender) {
                    return customButtonRender(row);  // Usa renderização personalizada se fornecida
                } else {
                    return `
                        <button type="button" title="Editar" class="btn btn-sm btn-outline-primary edit-item" data-id="${row.id}">
                            <i class="bi bi-building-up"></i>
                        </button>
                        <button type="button" title="Excluir" class="btn btn-sm btn-outline-danger delete-item" data-id="${row.id}">
                            <i class="bi bi-building-x"></i>
                        </button>
                    `;
                }
            }
        });
    }

    $(tableId).DataTable({
        ordering: true,
        paging: true,
        searching: true,
        processing: true,
        serverSide: true,
        ajax: {
            url: ajaxUrl,
            type: "POST",
            dataSrc: function (json) {
                //console.log("Dados retornados pelo servidor:", json);
                return json.data; // Certifique-se de que 'data' é a propriedade correta
            }
        },
        columns: finalColumns,
        language: {
            emptyTable: "Nenhum registro encontrado na tabela",
            info: "Mostrar _START_ até _END_ de _TOTAL_ registros",
            infoEmpty: "Mostrar 0 até 0 de 0 Registros",
            infoFiltered: "(Filtrar de um total de _MAX_ registros)",
            infoPostFix: "",
            infoThousands: ".",
            lengthMenu: "Mostrar _MENU_ registros por página",
            loadingRecords: "Carregando...",
            processing: "Carregando...",
            zeroRecords: "Nenhum registro encontrado",
            search: "Pesquisar",
            paginate: {
                next: "Proximo",
                previous: "Anterior",
                first: "Primeiro",
                last: "Ultimo"
            },
            aria: {
                sortAscending: ": Ordenar colunas de forma ascendente",
                sortDescending: ": Ordenar colunas de forma descendente"
            }
        }
    });
}

function renderMasp(data, type, row) {
    return data.toString().padStart(8, '0');
}

function formatDateToBR(dateString) {
    if (!dateString) return ''; // Retorna vazio se a data for nula

    // Ajusta a string da data para UTC removendo o horário
    const dateParts = dateString.split('-'); // Supondo formato ISO yyyy-mm-dd
    const adjustedDate = new Date(Date.UTC(dateParts[0], dateParts[1] - 1, dateParts[2])); // mês - 1 pois é zero-indexado

    const day = adjustedDate.getUTCDate().toString().padStart(2, '0');
    const month = (adjustedDate.getUTCMonth() + 1).toString().padStart(2, '0');
    const year = adjustedDate.getUTCFullYear();

    return `${day}/${month}/${year}`;
}

// Função de renderização personalizada para os botões da tabela de administradores
function renderAdministratorButtons(row) {
    return `
        <a title="Editar administrador ..." role="button" href="/UserAdministrator/Edit/${row.id}" class="btn btn-sm btn-outline-primary" data-id="${row.id}">
            <i title="Editar administrador ..." class="bi bi-person-vcard"></i>
        </a>
        <button title="Excluir administrador ..." type="button" data-bs-toggle="modal" asp-route-id="${row.id}" class="btn btn-sm btn-outline-danger delete-item" data-id="${row.id}">
            <i title="Excluir administrador ..." class="bi bi-person-dash"></i>
        </button>
    `;
}

// Função de renderização personalizada para os botões da tabela de avaliadores
function renderEvaluatorButtons(row) {
    let buttons = `
        <a title="Editar avaliador ..." role="button" href="/UserEvaluator/Edit/${row.id}" class="btn btn-sm btn-outline-primary" data-id="${row.id}">
            <i title="Editar avaliador ..." class="bi bi-person-vcard"></i>
        </a>
    `;

    // Verificação com base no tipo de usuário logado (currentUserType)
    if (currentUserType === '1') {
        // Verifica se o usuário logado é Admin (ajuste o valor 1 conforme seu enum)
        buttons += `
            <button title="Excluir avaliador ..." type="button" data-bs-toggle="modal" data-id="${row.id}" class="btn btn-sm btn-outline-danger delete-item">
                <i title="Excluir avaliador ..." class="bi bi-person-dash"></i>
            </button>
        `;
    }

    return buttons;
}

// Função de renderização personalizada para os botões da tabela de avaliadores
function renderServantButtons(row) {

    let buttons = `
        <a title="Editar servidor ..." role="button" href="/CivilServant/Edit/${row.id}" class="btn btn-sm btn-outline-primary" data-id="${row.id}">
            <i title="Editar servidor ..." class="bi bi-person-vcard"></i>
        </a>
        <a title="Boletim ..." role="button" target="_blank" href="/Reports/PrintAssessmentsToPDF/${row.id}" class="btn btn-sm btn-outline-secondary">
            <i title="Boletim ..." class="bi bi bi-printer-fill"></i>
        </a>
    `;

    // Verificação com base no tipo de usuário logado (currentUserType)
    if (currentUserType === '1') {
        // Verifica se o usuário logado é Admin (ajuste o valor 1 conforme seu enum)

        buttons += `
            <button title="Excluir servidor ..." type="button" data-bs-toggle="modal" asp-route-id="${row.id}" class="btn btn-sm btn-outline-danger delete-item" data-id="${row.id}">
                <i title="Excluir servidor ..." class="bi bi-person-dash"></i>
            </button>
        `;
    }

    return buttons;
}

// Função de renderização personalizada para os botões da tabela de avaliadores
function renderLicenseButtons(row) {
    return `
        <button type="button" title="Editar" class="btn btn-sm btn-outline-primary edit-item" data-id="${row.id}"><i title="Editar licença ..." class="bi bi-file-earmark-medical"></i></button>

        <button type="button" title="Excluir" class="btn btn-sm btn-outline-danger delete-item" data-id="${row.id}"><i title="Excluir licença ..." class="bi bi-trash"></i></button>
    `;
}

// Função de renderização personalizada para os botões da tabela de avaliadores
function renderServantLicenseButtons(row) {
    return `
        <button type="button" title="Editar licença ..." class="btn btn-sm btn-outline-primary edit-item" data-id="${row.id}"><i title="Editar ..."  class="bi bi-file-earmark-medical"></i></button>

        <button type="button" title="Excluir" class="btn btn-sm btn-outline-danger delete-item" data-id="${row.id}"><i title="Excluir ..." class="bi bi-trash"></i></button>
    `;
}

function renderUserStats(data, type, row) {
    switch (row.userStats) {
        case 0: // UnderLicense
            return `<span title="Inativo" class="material-symbols-outlined text-danger fs-5 m-1 p-1">radio_button_checked</span>`;
        case 1: // Active
            return `<span title="Ativo" class="material-symbols-outlined text-success fs-5 m-1 p-1">radio_button_checked</span>`;
        case 2: // Inactive
            // UnderLicense
            return `<span title="sob licença" id="statsEvaluatorInactive" class="material-symbols-outlined text-warning fs-5 m-1 p-1">radio_button_checked</span>`;
        default:
            return `<span title="Desconhecido" class="material-symbols-outlined text-muted fs-5 m-1 p-1">help_outline</span>`;
    }
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

// Atualize a definição de colunas para incluir a função de renderização
initializeDataTable("#administrator-table", '/UserAdministrator/Index', [
    { data: "userStats", name: "UserStats", render: renderUserStats, searchable: false },
    { data: "masp", name: "Masp", render: renderMasp  },
    { data: "name", name: "Name" },
    { data: "login", name: "Login" },
    { data: "email", name: "Email" },
    { data: "phone", name: "Phone" }
], true, renderAdministratorButtons);

initializeDataTable("#eval-table", '/UserEvaluator/Index', [
    { data: "userStats", name: "UserStats", render: renderUserStats, searchable: false },
    { data: "masp", name: "Masp", render: renderMasp  },
    { data: "name", name: "Name" },
    { data: "login", name: "Login" },
    { data: "email", name: "Email" },
    { data: "phone", name: "Phone" }
], true, renderEvaluatorButtons);

// Atualize a definição de colunas para incluir a função de renderização
initializeDataTable("#servant-table", '/CivilServant/Index', [
    { data: "userStats", name: "UserStats", render: renderUserStats, searchable: false },
    { data: "masp", name: "Masp", render: renderMasp },
    { data: "name", name: "Name" },
    { data: "login", name: "Login" },
    { data: "email", name: "Email" },
    { data: "phone", name: "Phone" }
], true, renderServantButtons);

// Atualize a definição de colunas para incluir a função de renderização LICENÇAS
initializeDataTable("#license-table", '/License/Index', [
    { data: "name", name: "Name" }
], true, renderLicenseButtons);

// Atualize a definição de colunas para incluir a função de renderização SERVIDORES SOB LICENÇA
initializeDataTable("#servantLicense-table", '/ServantLicense/Index', [
    { data: "masp", name: "Masp", render: renderMasp  },
    { data: "name", name: "Name" },
    { data: "licenseName", name: "LicenseName" },
    {
        data: "startDate",
        name: "StartDate",
        render: function (data, type, row) {
            return formatDateToBR(data);  // Aplica a formatação da data
        }
    },
    {
        data: "endDate",
        name: "EndDate",
        render: function (data, type, row) {
            return formatDateToBR(data);  // Aplica a formatação da data
        }
    }
], true, renderServantLicenseButtons);