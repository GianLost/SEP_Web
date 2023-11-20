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

function EditInstituition() {
    
    let properties = {
        Id: $("#edit-instituition-id").val(),
        Name: $("#edit-instituition-name").val(),
        UserAdministratorId: $("#edit-instituition-admId").val()
    };
    $.post("/Instituition/Edit", properties)

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

$(document).ready(function () {
    $("#edit-instituition-form").submit(function (e) {
        e.preventDefault();
        EditInstituition();
    });
});