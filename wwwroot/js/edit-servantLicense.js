// Função para calcular a duração da licença com base nas datas selecionadas
function calculateDuration(startDate, endDate) {
    startDate = new Date(startDate);
    endDate = new Date(endDate);
    if (startDate && endDate && startDate <= endDate) {
        var differenceInTime = endDate.getTime() - startDate.getTime();
        return (differenceInTime / (1000 * 3600 * 24)).toFixed(0) + ' dias';
    }
    return '';
}

// Função para validar a duração da licença
function validateLicenseDuration(startDate, endDate, maxDuration) {
    if (startDate && endDate && startDate <= endDate) {
        var differenceInTime = endDate.getTime() - startDate.getTime();
        var differenceInDays = differenceInTime / (1000 * 3600 * 24);
        if (differenceInDays > maxDuration) {
            return 'O prazo estabelecido excede o tempo máximo permitido para a licença selecionada!';
        }
    }
    return '';
}

// Evento change para calcular a duração da licença e validar as datas
$(document).on('change', '.edit-servantLicense-startDate, .edit-servantLicense-endDate', function () {
    var startDate = new Date($(this).closest('.modal').find('.edit-servantLicense-startDate').val());
    var endDate = new Date($(this).closest('.modal').find('.edit-servantLicense-endDate').val());
    $(this).closest('.modal').find('.edit-license-duration').val(calculateDuration(startDate, endDate));
    var maxDuration = parseInt($(this).closest('.modal').find('.edit-license-duration-hidden').val());
    var validationError = validateLicenseDuration(startDate, endDate, maxDuration);
    $(this).closest('.modal').find(".field-Time-validation-error").html(validationError);
    // Verificar se a data de término é anterior à data de início
    if (endDate < startDate) {
        $(this).closest('.modal').find(".field-Time-validation-error").html('A data de término não pode ser anterior à data de início!');
    }
});

// Evento change para o select de licenças
$(document).on('change', '.edit-servantLicense-LicensesId', function () {
    var licenseId = $(this).val();
    var modal = $(this).closest('.modal');
    if (!licenseId) {
        modal.find('.edit-license-duration-hidden').val('');
        return;
    }
    $.ajax({
        url: '/ServantLicense/GetLicenseDuration',
        type: 'GET',
        data: { id: licenseId },
        success: function (response) {
            modal.find('.edit-license-duration-hidden').val(response + ' dias');
            var startDate = new Date(modal.find('.edit-servantLicense-startDate').val());
            var endDate = new Date(modal.find('.edit-servantLicense-endDate').val());
            modal.find('.edit-license-duration').val(calculateDuration(startDate, endDate));
            var maxDuration = parseInt(response);
            var validationError = validateLicenseDuration(startDate, endDate, maxDuration);
            modal.find(".field-Time-validation-error").html(validationError);
            // Verificar se a data de término é anterior à data de início
            if (endDate < startDate) {
                modal.find(".field-Time-validation-error").html('A data de término não pode ser anterior à data de início!');
            }
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText, error);
        }
    });
});

// Evento de clique para abrir o modal de edição de licenças
$(document).on("click", "[data-toggle='modal'][data-target^='#edit-servantLicenseModal']", function () {
    let url = '@Url.Action("Edit", "ServantLicense")/' + $(this).data('id');
    let modalId = $(this).data('target');

    $.get(url)
        .done(function (data) {
            $(modalId + ' .modal-body').html(data);

            // Carregar dinamicamente a duração da licença ao abrir o modal
            var licenseId = $(modalId + ' #edit-licenseType').val();
            if (licenseId) {
                $.ajax({
                    url: '/ServantLicense/GetLicenseDuration',
                    type: 'GET',
                    data: { id: licenseId },
                    success: function (response) {
                        $(modalId + ' #license-duration-helper').val(response + ' dias');
                    },
                    error: function (xhr, status, error) {
                        console.error(xhr.responseText, error);
                    }
                });
            }
        });
});

// Evento de submit para o formulário de edição de licenças
$(document).on("submit", "#edit-servantLicense-form", function (e) {

    let form = $(this);

    $.ajax({
        url: form.attr("action"),
        method: form.attr("method"),
        data: form.serialize(),
        success: function (output) {
            if (output.stats === 1) {
                $(location).attr('href', '/ServantLicense/Index');
            } else if (output.stats === 2) {
                if (output.civilServantId <= 0) {
                    $(".select-servant-validation-error").html('Selecione um servidor!');
                } else {
                    $(".select-servant-validation-error").empty();
                }
                if (output.licenseName <= 0) {
                    $(".select-license-validation-error").html('Selecione o tipo da licença!');
                } else {
                    $(".select-license-validation-error").empty();
                }
                if (output.sDate == null) {
                    $(".field-sdate-validation-error").html('Informe a data de início!');
                } else {
                    $(".field-sdate-validation-error").empty();
                }
                if (output.eDate == null) {
                    $(".field-edate-validation-error").html('Informe a data de término!');
                } else {
                    $(".field-edate-validation-error").empty();
                }
            }
        },
        error: function () {
            alert("Ocorreu um erro!");
        }
    });

    e.preventDefault();
});