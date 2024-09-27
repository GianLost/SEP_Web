// Função genérica para configurar os eventos dos botões de ação (editar/excluir)
function setupTableActions(tableId, editUrl, deleteUrl, editModalId, deleteModalId, enableEdit = true, enableDelete = true) {

    // Verifica se o botão de edição deve ser configurado
    if (enableEdit) {
        // Evento para o botão de edição
        $(tableId).on('click', '.edit-item', function () {
            var itemId = $(this).data('id');

            $.ajax({
                url: editUrl,
                type: 'GET',
                data: { id: itemId },
                success: function (response) {
                    $('#dynamic-modal').html(response);
                    $('#' + editModalId + itemId).modal('show');
                }
            });
        });
    }

    // Verifica se o botão de exclusão deve ser configurado
    if (enableDelete) {
        // Evento para o botão de exclusão
        $(tableId).on('click', '.delete-item', function () {
            var itemId = $(this).data('id');

            $.ajax({
                url: deleteUrl,
                type: 'GET',
                data: { id: itemId },
                success: function (response) {
                    $('#dynamic-modal').html(response);
                    $('#' + deleteModalId + itemId).modal('show');
                }
            });
        });
    }
}

// Configurar ações para as tabelas de estruturas (com edição e exclusão)
setupTableActions('#instituition-table', '/Instituition/EditModal', '/Instituition/DeleteModal', 'edit-instituitionModal', 'deleteInstituition');
setupTableActions('#division-table', '/Division/EditModal', '/Division/DeleteModal', 'edit-divisionModal', 'deleteDivision');
setupTableActions('#section-table', '/Section/EditModal', '/Section/DeleteModal', 'edit-sectionModal', 'deleteSection');
setupTableActions('#sector-table', '/Sector/EditModal', '/Sector/DeleteModal', 'edit-sectorModal', 'deleteSector');
setupTableActions('#license-table', '/License/EditModal', '/License/DeleteModal', 'edit-licenseModal', 'deleteLicense');

// Configurar ações para as tabelas de usuários (apenas exclusão)
setupTableActions('#administrator-table', null, '/UserAdministrator/DeleteModal', null, 'deleteAdministrator', false, true);
// Configurar ações para as tabelas de usuários (apenas exclusão)
setupTableActions('#eval-table', null, '/UserEvaluator/DeleteModal', null, 'deleteEval', false, true);
// Configurar ações para as tabelas de usuários (apenas exclusão)
setupTableActions('#servant-table', null, '/CivilServant/DeleteModal', null, 'deleteServant', false, true);