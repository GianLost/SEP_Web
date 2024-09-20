/* JavaScript responsável pelo POST do formulário de cadastro de divisões */
function RegisterSection() {
    let properties = {
        Name: $("#section-name").val(),
        DivisionId: $("#section-divisionid").val(),
    };
    $.post("/Section/Register", properties)

        .done(function (output) {
            if (output.stats == "OK") {
                $(location).attr('href', '/Section/Index');

            } else if (output.stats == "ERROR") {
                if (properties.DivisionId <= 0) {
                    $(".select-validation-error").html('Selecione uma divisão !');
                } else {
                    $(".select-validation-error").empty();
                }
                if (properties.Name <= 0) {
                    $(".field-validation-error").html('Informe uma descrição para a seção !');
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
    $("#section-form").submit(function (e) {
        e.preventDefault();
        RegisterSection();
    });
});

/* JavaScript responsável pelo POST do formulário de edição de divisões */

$(document).ready(function () {

    $(document).on("click", "[data-toggle='modal'][data-target^='#edit-sectionModal']", function () {
        let url = '@Url.Action("Edit", "Section")/' + $(this).data('id');
        let modalId = $(this).data('target');

        $.get(url)
            .done(function (data) {
                $(modalId + ' .modal-body').html(data);
            });
    });

    $(document).on("submit", "#edit-section-form", function (e) {

        let form = $(this);

        // Se o campo 'Name' não estiver vazio, continua com a submissão do formulário via AJAX
        $.ajax({
            url: form.attr("action"),
            method: form.attr("method"),
            data: form.serialize(),
            success: function (output) {
                if (output.stats === "OK") {
                    $(location).attr('href', '/Section/Index');
                } else if (output.stats === "ERROR") {
                    if (output.sectionI <= 0) {
                        $(".select-validation-error").html('Selecione uma divisão !');
                    } else {
                        $(".select-validation-error").empty();
                    }
                    if (output.sectionName == null) {
                        $(".field-validation-error").html('Informe uma descrição para a seção !');
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
$('#section-table').on('click', '.edit-item', function () {
    var sectionId = $(this).data('id');

    $.ajax({
        url: '/Section/EditModal',
        type: 'GET',
        data: { id: sectionId },
        success: function (response) {
            $('#dynamic-modal').html(response);
            $('#edit-sectionModal' + sectionId).modal('show');
        }
    });
});

// Evento para o botão de exclusão
$('#section-table').on('click', '.delete-item', function () {
    var sectionId = $(this).data('id');

    $.ajax({
        url: '/Section/DeleteModal',
        type: 'GET',
        data: { id: sectionId },
        success: function (response) {
            $('#dynamic-modal').html(response);
            $('#deleteSection' + sectionId).modal('show');
        }
    });
});