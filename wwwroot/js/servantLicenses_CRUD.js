/* JavaScript responsável pelo POST do formulário de cadastro de licenças */

function RegisterServantLicense() {

    let civilServantId = $("#servantLicense-civilServantId").val();
    let licensesId = $("#servantLicense-LicensesId").val();
    let startDate = $("#servantLicense-startDate").val();
    let endDate = $("#servantLicense-endDate").val();

    // Verifica se os campos obrigatórios estão preenchidos
    if (!civilServantId || !licensesId || !startDate || !endDate) {
        $(".select-servant-validation-error").html(!civilServantId ? 'Selecione um servidor!' : '');
        $(".select-license-validation-error").html(!licensesId ? 'Selecione o tipo de licença!' : '');
        $(".field-sdate-validation-error").html(!startDate ? 'Informe a data de início!' : '');
        $(".field-edate-validation-error").html(!endDate ? 'Informe a data de término!' : '');
        return;
    }

    // Verifica se a data de término é anterior à data de início
    if (new Date(endDate) <= new Date(startDate)) {
        $(".field-Time-validation-error").html('A data de término não pode ser anterior à data de início!');
        return;
    }

    $.post("/ServantLicense/Register", {
        CivilServantId: civilServantId,
        LicensesId: licensesId,
        StartDate: startDate,
        EndDate: endDate
    }).done(function (output) {
        if (output.stats === 1 || output.stats === 7) {
            $(location).attr('href', '/ServantLicense/Index');
        }
    }).fail(function () {
        alert("Ocorreu um erro ao aplicar a licença!");
    });
}

function CalculateDurationLicenses() {
    var startDate = new Date($('#servantLicense-startDate').val());
    var endDate = new Date($('#servantLicense-endDate').val());

    // Verifica se as datas são válidas
    if (startDate && endDate && startDate <= endDate) {
        var differenceInTime = endDate.getTime() - startDate.getTime();
        var differenceInDays = differenceInTime / (1000 * 3600 * 24); // Convertendo milissegundos para dias
        $('#license-duration').val(differenceInDays.toFixed(0) + ' dias');
    } else {
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