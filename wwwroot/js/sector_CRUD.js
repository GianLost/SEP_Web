/* JavaScript responsável pelo POST do formulário de cadastro de setores */

function RegisterSector() {
    let properties = {
        Name: $("#sector-name").val(),
        SectionId: $("#sector-sectionid").val(),
    };
    $.post("/Sector/Register", properties)

        .done(function (output) {
            if (output.stats == "OK") {
                $(location).attr('href', '/Sector/Index');

            } else if (output.stats == "ERROR") {
                if (properties.SectionId <= 0) {
                    $(".select-validation-error").html('Selecione uma seção !');
                } else {
                    $(".select-validation-error").empty();
                }
                if (properties.Name <= 0) {
                    $(".field-validation-error").html('Informe uma descrição para o setor !');
                } else {
                    $(".field-validation-error").empty();
                }
            }
        })

        .fail(function () {
            alert("Ocorreu um erro!");
        });
}

$(document).ready(function () {
    $("#sector-form").submit(function (e) {
        e.preventDefault();
        RegisterSector();
    });
});


/* JavaScript responsável pelo POST do formulário de edição de setores */

$(document).ready(function () {

    $(document).on("click", "[data-toggle='modal'][data-target^='#edit-sectorModal']", function () {
        let url = '@Url.Action("Edit", "Sector")/' + $(this).data('id');
        let modalId = $(this).data('target');

        $.get(url)
            .done(function (data) {
                $(modalId + ' .modal-body').html(data);
            });
    });

    $(document).on("submit", "#edit-sector-form", function (e) {

        let form = $(this);

        // Se o campo 'Name' não estiver vazio, continua com a submissão do formulário via AJAX
        $.ajax({
            url: form.attr("action"),
            method: form.attr("method"),
            data: form.serialize(),
            success: function (output) {
                if (output.stats === "OK") {
                    $(location).attr('href', '/Sector/Index');
                } else if (output.stats === "ERROR") {
                    if (output.sectorI <= 0) {
                        $(".select-validation-error").html('Selecione uma seção !');
                    } else {
                        $(".select-validation-error").empty();
                    }
                    if (output.sectorName == null) {
                        $(".field-validation-error").html('Informe uma descrição para o setor !');
                    } else {
                        $(".field-validation-error").empty();
                    }
                }
            },
            error: function () {
                alert("Ocorreu um erro!");
            }
        });

        e.preventDefault();
    });
});

// Evento para o botão de edição
$('#sector-table').on('click', '.edit-item', function () {
    var sectorId = $(this).data('id');

    $.ajax({
        url: '/Sector/EditModal',
        type: 'GET',
        data: { id: sectorId },
        success: function (response) {
            $('#dynamic-modal').html(response);
            $('#edit-sectorModal' + sectorId).modal('show');
        }
    });
});

// Evento para o botão de exclusão
$('#sector-table').on('click', '.delete-item', function () {
    var sectorId = $(this).data('id');

    $.ajax({
        url: '/Sector/DeleteModal',
        type: 'GET',
        data: { id: sectorId },
        success: function (response) {
            $('#dynamic-modal').html(response);
            $('#deleteSector' + sectorId).modal('show');
        }
    });
});
