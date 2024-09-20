/* JavaScript responsável pelo POST do formulário de cadastro de divisões */

function RegisterDivision() {
    let properties = {
        Name: $("#division-name").val(),
        InstituitionId: $("#division-instituitionid").val(),
    };
    $.post("/Division/Register", properties)

        .done(function (output) {
            if (output.stats == "OK") {
                $(location).attr('href', '/Division/Index');

            } else if (output.stats == "ERROR") {
                if (properties.InstituitionId <= 0) {
                    $(".select-validation-error").html('Selecione um órgão !');
                } else {
                    $(".select-validation-error").empty();
                }
                if (properties.Name <= 0) {
                    $(".field-validation-error").html('Informe uma descrição para a divisão !');
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
    $("#division-form").submit(function (e) {
        e.preventDefault();
        RegisterDivision();
    });
});


/* JavaScript responsável pelo POST do formulário de edição de divisões */

$(document).ready(function () {

    $(document).on("click", "[data-toggle='modal'][data-target^='#edit-divisionModal']", function () {
        let url = '@Url.Action("Edit", "Division")/' + $(this).data('id');
        let modalId = $(this).data('target');

        $.get(url)
            .done(function (data) {
                $(modalId + ' .modal-body').html(data);
            });
    });

    $(document).on("submit", "#edit-division-form", function (e) {

        let form = $(this);

        // Se o campo 'Name' não estiver vazio, continua com a submissão do formulário via AJAX
        $.ajax({
            url: form.attr("action"),
            method: form.attr("method"),
            data: form.serialize(),
            success: function (output) {
                if (output.stats === "OK") {
                    $(location).attr('href', '/Division/Index');
                } else if (output.stats === "ERROR") {
                    if (output.divisionI <= 0) {
                        $(".select-validation-error").html('Selecione um órgão !');
                    } else {
                        $(".select-validation-error").empty();
                    }
                    if (output.divisionName == null) {
                        $(".field-validation-error").html('Informe uma descrição para a divisão !');
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
