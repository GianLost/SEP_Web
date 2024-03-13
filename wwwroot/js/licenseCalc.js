function CalculateDurationLicenses()
{
    
}

$(document).ready(function () {
    $('#servantLicense-LicensesId').change(function () {
        var licenseId = $(this).val();
        $.ajax({
            url: '/License/GetLicenseDuration', // Substitua pelo URL adequado
            type: 'GET',
            data: { id: licenseId },
            success: function (response) {
                // Atualizar o campo oculto com a duração da licença
                $('#license-duration-hidden').val(response + ' dias');
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
            }
        });
    });
});