function handleSuccess(properties, output) {
    if (output.stats === 1) {
        $(location).attr('href', '../Edit/' + properties.Id);
    } else if (output.stats === 2) {
        handleValidationErrors(properties);
    } else if (output.stats === 3) {
        $(".field-validation-error").html('');
        $(".compPass").html('As senhas são diferentes !');
        return false;
    } else if (output.stats === 4) {
        $(location).attr('href', "/Login/Index");
        alert('A senha foi alterada com sucesso e você deve realizar o login novamente !');
    }
}

function handleValidationErrors(properties) {
    if (properties.Password === "") {
        $(".field-validation-error").html('Informe uma senha!');
    } else if (properties.Password !== "" && properties.Password.length < 8) {
        $(".field-validation-error").html('A senha deve conter 8 ou mais caracteres!');
    } else {
        $(".field-validation-error").empty();
    }

    if (properties.ComparePassword === "") {
        $(".compPass").html('Confirme sua senha!');
    } else if (properties.ComparePassword !== "" && properties.ComparePassword.length < 8) {
        $(".compPass").html('Confirme sua senha com 8 ou mais caracteres!');
    } else {
        $(".compPass").empty();
    }
}

function ChangeUserPass() {
    let properties = {
        Id: $(".password-Id").val(),
        Masp: $(".password-Masp").val(),
        Password: $(".changedPass").val(),
        ComparePassword: $(".confPass").val(),
    };

    $.post("/ChangePassword/ChangeUserPassword", properties)
        .done(function (output) {
            handleSuccess(properties, output);
        })
        .fail(function () {
            alert("Ocorreu um erro!");
        });
}

$('.passwordForm').show(function () {
    var masp = document.querySelector(".userMasp").value;
    document.querySelector(".password-Masp").value = masp;
});

$(document).ready(function () {
    $(".passwordForm").submit(function (e) {
        e.preventDefault();
        ChangeUserPass();
    });
});