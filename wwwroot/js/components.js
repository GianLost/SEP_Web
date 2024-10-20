/* Máscara para inputs de telefone celular */

$('.mask-phone').on('input', function (e) {
    let input = e.target;
    input.value = phoneMask(input.value);
});

const phoneMask = (value) => {
    if (!value) return "";
    value = value.replace(/\D/g, '').replace(/(\d{2})(\d)/, "($1) $2").replace(/(\d)(\d{4})$/, "$1-$2");

    return value;
}

/* Evento responsável por fechar os alerts de secesso ou erro das requisições */
$('.close-alert').click(function () {
    $('.alert').hide('hide');
});

/* Esta função também realiza o fechamneto dos alerts, porém de forma automática utilizando o setTimeout(); */
setTimeout(function () {
    $('.alert').hide('hide').fadeOut(500);
}, 4000);