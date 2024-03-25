$(document).ready(function () {
    $('#avaliacaoTab a').on('click', function (e) {
        e.preventDefault();
        $(this).tab('show');
        var targetTab = $(this).attr('data-bs-target');
        $(targetTab + ' select').empty(); // Limpa as listas suspensas ao trocar de aba
        // Preencha aqui as listas suspensas com as opções de notas para cada subitem
    });

    // Adiciona evento de mudança para todos os campos de seleção
    $('#avaliacaoTabContent select').change(function () {
        // Obtém o valor selecionado
        var selectedValue = parseInt($(this).val());
        // Obtém o campo de entrada de texto correspondente
        var justificationInput = $(this).siblings('.justification');

        // Define o limite de valor para habilitar a justificativa
        var limitValue;

        // Verifica o valor máximo permitido
        if ($(this).find('option').length === 11) {
            // Se houver 11 opções, o limite é 6
            limitValue = 6;
        } else {
            // Caso contrário, o limite é 60% do valor máximo possível
            limitValue = 12;
        }

        // Habilita ou desabilita o campo de justificativa com base no valor selecionado
        if (selectedValue < limitValue) {
            justificationInput.prop('disabled', false);
            // Adiciona a classe de validação obrigatória
            justificationInput.addClass('required');
            // Verifica se o campo está vazio
            if (justificationInput.val().trim() === '') {
                // Adiciona uma classe de estilo para indicar erro
                justificationInput.addClass('invalid');
            }
        } else {
            justificationInput.prop('disabled', true);
            justificationInput.val(''); // Limpa o conteúdo do campo de justificativa
            // Remove a classe de validação obrigatória
            justificationInput.removeClass('required');
            // Remove a classe de estilo de erro se o campo estiver vazio
            justificationInput.removeClass('invalid');
        }
    });

    // Adiciona validação durante a digitação no campo de justificativa
    $('.justification').on('input', function () {
        // Remove a classe de estilo de erro se o campo estiver preenchido
        if ($(this).val().trim() !== '') {
            $(this).addClass('required');
            $(this).removeClass('invalid');
        } else {
            $(this).addClass('required');
            $(this).addClass('invalid');
        }
    });

    // Adiciona validação ao enviar o formulário
    $('.assessments-form').submit(function (event) {
        // Itera sobre todos os campos de justificativa obrigatórios
        var anyInvalid = false;
        $('.justification .required').each(function () {
            // Verifica se o campo está vazio
            if ($(this).val().trim() === '') {
                // Adiciona uma classe de estilo para indicar erro
                $(this).addClass('invalid');
                anyInvalid = true;
            } else {
                // Remove a classe de estilo de erro se o campo estiver preenchido
                $(this).removeClass('invalid');
            }
        });

        if (anyInvalid) {
            $('#errorMessage').show();
            event.preventDefault(); // Impede o envio do formulário
        } else {
            $('#errorMessage').hide();
        }
    });

    // Desabilita apenas os campos de justificativa que não foram avaliados anteriormente
    $('.justification').each(function () {
        // Verifica se o campo está vazio e desabilita apenas se estiver
        if ($(this).val().trim() === '') {
            $(this).prop('disabled', true);
        }
    });

});