/* Função que realiza a formatação da data enquanto é digitada no input em data de admissão de CivilServant */

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