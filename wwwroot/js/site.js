
/* Start Phone-Mask */

const handlePhone = (event) => {
    let input = event.target
    input.value = phoneMask(input.value)
}

const phoneMask = (value) => {
    if (!value) return "";
    value = value.replace(/\D/g, '').replace(/(\d{2})(\d)/, "($1) $2").replace(/(\d)(\d{4})$/, "$1-$2");

    return value;
}

/* End Phone-Mask */

/* Evento responsável por fechar os alerts de secesso ou erro das requisições */
$('.close-alert').click(function () {
    $('.alert').hide('hide');
});
// /* Esta função também realiza o fechamneto dos alerts, porém de forma automática utilizando o setTimeout(); */
// setTimeout(function () {
//     $('.alert').hide('hide').fadeOut(500);
// }, 3000);


/* Fnção que realiza a formatação da data enquanto é digitada no input em data de admissão de CivilServant */

$('#servant-admission').on('input', function (e) {
    let value = e.target.value.replace(/\D/g, '');

    if (value.length > 8) {
        value = value.substring(0, 8); // Limita o tamanho da entrada a 8 caracteres
    }

    if (value.length >= 2) {
        const day = value.substring(0, 2);
        let month = value.substring(2, 4);
        let year = value.substring(4, 8);

        // Verifica se o dia é válido
        if (parseInt(day) < 1 || parseInt(day) > 31) {
            // Se não for, mantém apenas o dia e retorna
            e.target.value = day;
            return;
        }

        // Ajusta o valor do mês se o comprimento for maior que 2
        if (value.length >= 4) {
            month = value.substring(2, 4);
            // Verifica se o mês é válido
            if (parseInt(month) < 1 || parseInt(month) > 12) {
                // Se não for, mantém dia/mês e retorna
                e.target.value = day + '/' + month;
                return;
            }
        }

        // Garante que o ano seja exibido se estiver completo
        if (value.length >= 6) {
            year = value.substring(4, 8);
        }

        // Formata a data no formato dd/mm/yyyy
        value = day + (month ? '/' + month : '') + (year ? '/' + year : '');
    }

    e.target.value = value;
});



