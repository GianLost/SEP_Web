/* JavaScript responsável pelo POST do formulário de cadastro de licenças */

function RegisterServantLicense() {
    let properties = {
        CivilServantId: $("#servantLicense-civilServantId").val(),
        LicensesId: $("#servantLicense-LicensesId").val(),
        StartDate: $("#servantLicense-startDate").val(),
        EndDate: $("#servantLicense-endDate").val(),
    };
    $.post("/ServantLicense/Register", properties)

        .done(function (output) {
            if (output.stats == "OK") {
                $(location).attr('href', '/ServantLicense/Index');

            } else if (output.stats == "ERROR") {
                if (properties.CivilServantId <= 0) {
                    $(".select-servant-validation-error").html('selecione um servidor !');
                } else {
                    $(".select-servant-validation-error").empty();
                }
                if (properties.LicensesId <= 0) {
                    $(".select-license-validation-error").html('selecione o tipo de licença !');
                } else {
                    $(".select-license-validation-error").empty();
                }
                if (properties.StartDate <= 0) {
                    $(".field-sdate-validation-error").html('informe a data de início !');
                } else {
                    $(".field-sdate-validation-error").empty();
                }
                if (properties.EndDate <= 0) {
                    $(".field-edate-validation-error").html('informe a data de término !');
                } else {
                    $(".field-edate-validation-error").empty();
                }
            }
        })

        .fail(function () {
            alert("Ocorreu um erro!");
        });
}

$(document).ready(function () {
    $("#servant-license-form").submit(function (e) {
        e.preventDefault();
        RegisterServantLicense();
    });
});


/* JavaScript responsável pelo POST do formulário de edição de licenças 

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
}); */
