$(document).ready(function () {
    var checkboxes = $('.enableDisableAccount');
    var confirmed = false;

    checkboxes.change(function () {
        if (checkboxes.is(':checked') && !confirmed) {
            $('#confirmationModal').modal('show');

            $('.confirmAction').click(function () {
                // Marque o checkbox manualmente
                checkboxes.prop('checked', true);

                // Defina a variável de confirmação como verdadeira
                confirmed = true;

                // Feche o modal de confirmação
                $('#confirmationModal').modal('hide');
            });

            $('#confirmationModal').on('hidden.bs.modal', function () {
                // Se o usuário fechar o modal sem confirmar, desmarque o checkbox
                if (!confirmed) {
                    checkboxes.prop('checked', false);
                }
            });
        } else {
            // Se o checkbox estiver desmarcado, redefina a variável de confirmação
            checkboxes.prop('checked', true);
        }
    });
});