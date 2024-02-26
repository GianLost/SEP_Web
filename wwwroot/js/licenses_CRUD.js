/* JavaScript responsável pelo POST do formulário de cadastro de licenças */

function RegisterLicense() {
    let properties = {
        Name: $("#license-name").val(),
        Time: $("#license-time").val(),
    };
    $.post("/License/Register", properties)

        .done(function (output) {
            if (output.stats == "OK") {
                $(location).attr('href', '/License/Index');

            } else if (output.stats == "ERROR") {
                if (properties.Name <= 0) {
                    $(".select-validation-error").html('Informe o nome para a licença!');
                } else {
                    $(".select-validation-error").empty();
                }
                if (properties.Time <= 0) {
                    $(".field-validation-error").html('Informe tempo máx. de validade da liceça !');
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
    $("#license-form").submit(function (e) {
        e.preventDefault();
        RegisterLicense();
    });
});


/* JavaScript responsável pelo POST do formulário de edição de licenças */

$(document).ready(function () {

    $(document).on("click", "[data-toggle='modal'][data-target^='#edit-licenseModal']", function () {
        let url = '@Url.Action("Edit", "License")/' + $(this).data('id');
        let modalId = $(this).data('target');

        $.get(url)
            .done(function (data) {
                $(modalId + ' .modal-body').html(data);
            });
    });

    $(document).on("submit", "#edit-license-form", function (e) {

        let form = $(this);

        // Se o campo 'Name' não estiver vazio, continua com a submissão do formulário via AJAX
        $.ajax({
            url: form.attr("action"),
            method: form.attr("method"),
            data: form.serialize(),
            success: function (output) {
                if (output.stats === "OK") {
                    $(location).attr('href', '/License/Index');
                } else if (output.stats == "ERROR") {
                    if (output.licenseName == null) {
                        $(".select-validation-error").html('Informe o nome para a licença!');
                    } else {
                        $(".select-validation-error").empty();
                    }
                    if (output.licenseTime <= 0) {
                        $(".field-validation-error").html('Informe tempo máx. de validade da liceça !');
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
