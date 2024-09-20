// Função genérica para configurar os eventos dos botões de ação (editar/excluir)

function setupTableActions(tableId, editUrl, deleteUrl, editModalId, deleteModalId) {
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

// Configurar ações para as tabelas
setupTableActions('#instituition-table', '/Instituition/EditModal', '/Instituition/DeleteModal', 'edit-instituitionModal', 'deleteInstituition');

setupTableActions('#division-table', '/Division/EditModal', '/Division/DeleteModal', 'edit-divisionModal', 'deleteDivision');

setupTableActions('#section-table', '/Section/EditModal', '/Section/DeleteModal', 'edit-sectionModal', 'deleteSection');

setupTableActions('#sector-table', '/Sector/EditModal', '/Sector/DeleteModal', 'edit-sectorModal', 'deleteSector');