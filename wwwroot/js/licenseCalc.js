function CalculateDurationLicenses()
{
    
}

$(document).ready(function () {
    $('#servantLicense-LicensesId').change(function () {
        var licenseId = $(this).val();
        
        if (!licenseId) {
            $('#license-duration-hidden').val('');
            return;
        }
        
        $.ajax({
            url: '/License/GetLicenseDuration',
            type: 'GET',
            data: { id: licenseId },
            success: function (response) {
                $('#license-duration-hidden').val(response + ' dias');
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText, nullTime);
            }
        });
    });
});