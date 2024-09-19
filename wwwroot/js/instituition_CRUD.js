/* JavaScript responsável pelo POST do formulário de cadastro de órgãos */

function RegisterInstituition() {
    let properties = {
        Name: $("#instituition-name").val(),
    };
    $.post("/Instituition/Register", properties)

        .done(function (output) {
            if (output.stats == "OK") {
                $(location).attr('href', '/Instituition/Index');

            } else if (output.stats == "ERROR") {
                $(".field-validation-error").html('Informe uma descrição para o órgão !');
            }
        })

        .fail(function () {
            alert("Ocorreu um erro!");
        });
}

$(document).ready(function () {
    $("#instituition-form").submit(function (e) {
        e.preventDefault();
        RegisterInstituition();
    });
});


/* JavaScript responsável pelo POST do formulário de edição de órgãos */

$(document).ready(function () {

    $(document).on("click", "[data-toggle='modal'][data-target^='#edit-instituitionModal']", function () {
        let url = '@Url.Action("Edit", "Instituition")/' + $(this).data('id');
        let modalId = $(this).data('target');

        $.get(url)
            .done(function (data) {
                $(modalId + ' .modal-body').html(data);
            });
    });

    $(document).on("submit", "#edit-instiuition-form", function (e) {

        let form = $(this);

        // Se o campo 'Name' não estiver vazio, continua com a submissão do formulário via AJAX
        $.ajax({
            url: form.attr("action"),
            method: form.attr("method"),
            data: form.serialize(),
            success: function (output) {
                if (output.stats === "OK") {
                    $(location).attr('href', '/Instituition/Index');
                } else if (output.stats === "ERROR") {
                    $(".field-validation-error").html('Informe uma descrição para o órgão !');
                }
            },
            error: function () {
                alert("Ocorreu um erro!");
            }
        });

        e.preventDefault();
    });
});
