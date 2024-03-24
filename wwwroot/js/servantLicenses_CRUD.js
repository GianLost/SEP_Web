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
            if (output.stats == 1) {
                $(location).attr('href', '/ServantLicense/Index');

            } else if (output.stats == 2) {
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
            } else if (output.stats == 7) {
                $(location).attr('href', '/ServantLicense/Index');
            } else if (output.stats == 6) {
                if (new Date(properties.EndDate).getDate() <= new Date(properties.StartDate).getDate()) {
                    $(".field-Time-validation-error").html('A data de término não pode ser anterior à data de início !');
                } else {
                    $(".field-Time-validation-error").empty();
                    $(".alert").empty();
                }
            }
        })

        .fail(function () {
            alert("Ocorreu um erro ao aplicar a licença!");
        });
}

function CalculateDurationLicenses() {
    var startDate = new Date($('#servantLicense-startDate').val());
    var endDate = new Date($('#servantLicense-endDate').val());

    // Verificar se as datas são válidas
    if (startDate && endDate && startDate <= endDate) {
        var differenceInTime = endDate.getTime() - startDate.getTime();
        var differenceInDays = differenceInTime / (1000 * 3600 * 24); // Convertendo milissegundos para dias

        // Exibir o resultado no campo
        $('#license-duration').val(differenceInDays.toFixed(0) + ' dias');
    } else {
        // Limpar o campo se as datas forem inválidas
        $('#license-duration').val('');
    }
}

$(document).ready(function () {

    // Configuração do evento change para os campos de data
    $('#servantLicense-startDate, #servantLicense-endDate').change(function () {
        CalculateDurationLicenses(); // Chamada da função para calcular a duração
        validateLicenseDuration(); // Chamada da função para validar a duração da licença
    });

    // Função para validar a duração da licença
    function validateLicenseDuration() {
        var startDate = new Date($('#servantLicense-startDate').val());
        var endDate = new Date($('#servantLicense-endDate').val());

        if (startDate && endDate && startDate <= endDate) {
            var differenceInTime = endDate.getTime() - startDate.getTime();
            var differenceInDays = differenceInTime / (1000 * 3600 * 24); // Convertendo milissegundos para dias

            // Verificando a duração da licença selecionada
            var licenseDuration = parseInt($('#license-duration-hidden').val());

            // Se a diferença entre as datas for maior que a duração da licença
            if (differenceInDays > licenseDuration) {
                $(".field-Time-validation-error").html('O prazo estabelecido excede o tempo máximo permitido para a licença selecionada !');
                // Limpando os campos de datas
                $('#servantLicense-startDate').val('');
                $('#servantLicense-endDate').val('');
            } else {
                $(".field-Time-validation-error").empty();
            }
        }
    }

    // Evento change para o select de licenças
    $('#servantLicense-LicensesId').change(function () {
        var licenseId = $(this).val();

        if (!licenseId) {
            $('#license-duration-hidden').val('');
            return;
        }

        $.ajax({
            url: '/ServantLicense/GetLicenseDuration',
            type: 'GET',
            data: { id: licenseId },
            success: function (response) {
                $('#license-duration-hidden').val(response + ' dias');
                CalculateDurationLicenses(); // Chama a função após obter a duração da licença
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText, nullTime);
            }
        });
    });

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