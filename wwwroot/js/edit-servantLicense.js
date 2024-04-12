// function EditServantLicense() {
//     let servantLicenseId = $("#edit-servantLicense-Id").val();
//     let civilServantId = $("#edit-civilServantId").val();
//     let licensesId = $("#edit-licenseType").val();
//     let startDate = $("#edit-startDate").val();
//     let endDate = $("#edit-endDate").val();

//     // Verifica se os campos obrigatórios estão preenchidos
//     let errors = false;
//     if (!civilServantId) {
//         $(".select-servant-validation-error").html('Selecione um servidor!');
//         errors = true;
//     } else {
//         $(".select-servant-validation-error").html('');
//     }
//     if (!licensesId) {
//         $(".select-license-validation-error").html('Selecione o tipo de licença!');
//         errors = true;
//     } else {
//         $(".select-license-validation-error").html('');
//     }
//     if (!startDate) {
//         $(".field-sdate-validation-error").html('Informe a data de início!');
//         errors = true;
//     } else {
//         $(".field-sdate-validation-error").html('');
//     }
//     if (!endDate) {
//         $(".field-edate-validation-error").html('Informe a data de término!');
//         errors = true;
//     } else {
//         $(".field-edate-validation-error").html('');
//     }

//     if (errors) {
//         return; // Impede o envio do formulário se houver erros
//     }

//     // Verifica se a data de término é anterior à data de início
//     if (new Date(endDate) <= new Date(startDate)) {
//         $(".field-Time-validation-error").html('A data de término não pode ser anterior à data de início!');
//         return;
//     }

//     // Se não houver erros, prosseguir com o envio do formulário via AJAX
//     $.post("/ServantLicense/Edit", {
//         Id: servantLicenseId,
//         CivilServantId: civilServantId,
//         LicensesId: licensesId,
//         StartDate: startDate,
//         EndDate: endDate
//     }).done(function (output) {
//         if (output.stats === 1 || output.stats === 7) {
//             $(location).attr('href', '/ServantLicense/Index');
//         }
//     }).fail(function () {
//         alert("Ocorreu um erro ao aplicar a licença!");
//     });
// }

// $(document).ready(function () {

//     // Função para calcular a duração da licença com base nas datas selecionadas
//     function calculateDuration(startDate, endDate) {
//         if (startDate && endDate && startDate <= endDate) {
//             var differenceInTime = endDate.getTime() - startDate.getTime();
//             return (differenceInTime / (1000 * 3600 * 24)).toFixed(0) + ' dias';
//         }
//         return '';
//     }

//     // Função para validar a duração da licença
//     function validateLicenseDuration(startDate, endDate, maxDuration) {
//         if (startDate && endDate && startDate <= endDate) {
//             var differenceInTime = endDate.getTime() - startDate.getTime();
//             var differenceInDays = differenceInTime / (1000 * 3600 * 24);

//             if (differenceInDays > maxDuration) {
//                 return 'O prazo estabelecido excede o tempo máximo permitido para a licença selecionada!';
//             }
//         }
//         return '';
//     }

//     // Evento change para calcular a duração da licença e validar as datas
//     $(document).on('change', '.edit-servantLicense-startDate, .edit-servantLicense-endDate', function () {
//         var startDate = new Date($(this).closest('.modal').find('.edit-servantLicense-startDate').val());
//         var endDate = new Date($(this).closest('.modal').find('.edit-servantLicense-endDate').val());
//         $(this).closest('.modal').find('.edit-license-duration').val(calculateDuration(startDate, endDate));

//         var maxDuration = parseInt($(this).closest('.modal').find('.edit-license-duration-hidden').val());
//         var validationError = validateLicenseDuration(startDate, endDate, maxDuration);
//         $(this).closest('.modal').find(".field-Time-validation-error").html(validationError);

//         // Verificar se a data de término é anterior à data de início
//         if (endDate < startDate) {
//             $(this).closest('.modal').find(".field-Time-validation-error").html('A data de término não pode ser anterior à data de início!');
//         }
//     });

//     // Evento change para o select de licenças
//     $(document).on('change', '.edit-servantLicense-LicensesId', function () {
//         var licenseId = $(this).val();
//         var modal = $(this).closest('.modal');
//         if (!licenseId) {
//             modal.find('.edit-license-duration-hidden').val();
//             return;
//         }

//         $.ajax({
//             url: '/ServantLicense/GetLicenseDuration',
//             type: 'GET',
//             data: { id: licenseId },
//             success: function (response) {
//                 modal.find('.edit-license-duration-hidden').val(response + ' dias');
//                 var startDate = new Date(modal.find('.edit-servantLicense-startDate').val());
//                 var endDate = new Date(modal.find('.edit-servantLicense-endDate').val());
//                 modal.find('.edit-license-duration').val(calculateDuration(startDate, endDate));

//                 var maxDuration = parseInt(response);
//                 var validationError = validateLicenseDuration(startDate, endDate, maxDuration);
//                 modal.find(".field-Time-validation-error").html(validationError);

//                 // Verificar se a data de término é anterior à data de início
//                 if (endDate < startDate) {
//                     modal.find(".field-Time-validation-error").html('A data de término não pode ser anterior à data de início!');
//                 }
//             },
//             error: function (xhr, status, error) {
//                 console.error(xhr.responseText, error);
//             }
//         });
//     });

//     // Evento submit do formulário via AJAX
//     $('#edit-servantLicense-form').submit(function (e) {
//         e.preventDefault();
//         EditServantLicense();
//     });

//     // Evento click para abrir o modal de edição
//     $(document).on('click', '.btn-edit-license', function () {
//         var licenseId = $(this).data('route-id');
//         var modalId = '#edit-servantLicenseModal-' + licenseId;
//         $(modalId).modal('show');
//     });
// });


// Função para calcular a duração da licença com base nas datas selecionadas
function calculateDuration(startDate, endDate) {
    if (startDate && endDate && startDate <= endDate) {
        const differenceInTime = endDate.getTime() - startDate.getTime();
        return (differenceInTime / (1000 * 3600 * 24)).toFixed(0) + ' dias';
    }
    return '';
}

// Função para validar se algum campo obrigatório está vazio
function validateRequiredFields(properties) {
    const errors = {};

    if (!properties.CivilServantId) {
        errors.selectServant = 'Selecione um servidor!';
    }

    if (!properties.LicensesId) {
        errors.selectLicense = 'Selecione o tipo de licença!';
    }

    if (!properties.StartDate) {
        errors.startDate = 'Informe a data de início!';
    }

    if (!properties.EndDate) {
        errors.endDate = 'Informe a data de término!';
    }

    return errors;
}

// Função para validar a duração da licença
function validateLicenseDuration(startDate, endDate, maxDuration) {
    const start = new Date(startDate);
    const end = new Date(endDate);
    const duration = (end - start) / (1000 * 60 * 60 * 24); // Duração em dias

    if (duration > maxDuration) {
        return "A duração da licença não pode exceder " + maxDuration + " dias.";
    }

    return null; // Retorna null se a duração for válida
}

// Função para preencher o campo de duração da licença e realizar validações ao abrir o modal
function fillLicenseDuration(modal) {
    const licenseId = modal.find('.edit-servantLicense-LicensesId').val();
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
            const startDate = new Date(modal.find('.edit-servantLicense-startDate').val());
            const endDate = new Date(modal.find('.edit-servantLicense-endDate').val());
            modal.find('.edit-license-duration').val(calculateDuration(startDate, endDate));

            const maxDuration = parseInt(response);
            const validationError = validateLicenseDuration(startDate, endDate, maxDuration);
            modal.find(".field-Time-validation-error").html(validationError);
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText, error);
        }
    });
}

// Função para atualizar dinamicamente a diferença de datas
function updateDateDifference(modal) {
    const startDate = new Date(modal.find('.edit-servantLicense-startDate').val());
    const endDate = new Date(modal.find('.edit-servantLicense-endDate').val());
    modal.find('.edit-license-duration').val(calculateDuration(startDate, endDate));
}

$(document).ready(function () {
    // Evento click para abrir o modal de edição e preencher os campos
    $('.btn-edit-license').click(function () {
        const licenseId = $(this).data('route-id');
        const modalId = '#edit-servantLicenseModal-' + licenseId;
        const modal = $(modalId);

        // Preencher campos ao abrir o modal
        fillLicenseDuration(modal);

        modal.modal('show');
    });

    // Evento change para o select de licenças
    $('.edit-servantLicense-LicensesId').change(function () {
        const modal = $(this).closest('.modal');
        fillLicenseDuration(modal);
    });

    // Evento change para as datas de início e término
    $('.edit-servantLicense-startDate, .edit-servantLicense-endDate').change(function () {
        const modal = $(this).closest('.modal');
        updateDateDifference(modal);
    });

});

function EditServantLicense() {
    const properties = {
        Id: $("#edit-servantLicense-Id").val(),
        CivilServantId: $("#edit-civilServantId").val(),
        LicensesId: $("#edit-licenseType").val(),
        StartDate: $("#edit-startDate").val(),
        EndDate: $("#edit-endDate").val()
    };

    // Limpar mensagens de erro antes de realizar novas validações
    $(".validate-fields span").html('');

    // Validar campos obrigatórios
    const requiredErrors = validateRequiredFields(properties);
    Object.keys(requiredErrors).forEach(key => {
        $(`.${key}-validation-error`).html(requiredErrors[key]);
    });

    // Verificar se a data de término é anterior à data de início
    if (new Date(properties.EndDate) <= new Date(properties.StartDate)) {
        $(".field-Time-validation-error").html('A data de término não pode ser anterior à data de início!');
        return;
    }

    // Se não houver erros, prosseguir com o envio do formulário via AJAX
    $.ajax({
        url: "/ServantLicense/Edit",
        type: "POST",
        data: properties,
        success: function (output) {
            if (output.stats === "OK") {
                window.location.href = "/ServantLicense/Index"; // Redireciona para a página de índice em caso de sucesso
            } else if (output.stats === "ERROR") {
                // Tratamento de erros do servidor
            } else if (output.stats === "INVALID") {
                alert("Ocorreu um erro ao aplicar a licença!");
            }
        },
        error: function () {
            alert("Ocorreu um erro ao aplicar a licença!");
        }
    });
}

function CalculateDurationLicenses() {
    var startDate = new Date($('#edit-startDate').val());
    var endDate = new Date($('#edit-endDate').val());

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
    $('#edit-startDate, #edit-endDate').change(function () {
        CalculateDurationLicenses(); // Chamada da função para calcular a duração
        validateLicenseDuration(); // Chamada da função para validar a duração da licença
    });

    // Evento submit para o formulário de edição
    $("#edit-servant-license-form").submit(function (e) {
        e.preventDefault();
        EditServantLicense();
    });
});